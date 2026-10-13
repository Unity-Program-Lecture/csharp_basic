using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace NgoLab.Verification
{
    // 명령행으로 요청한 자동 검증에만 사용합니다. 일반 Play/버튼 실습에서는 생성되지 않습니다.
    // 학생용 NetworkLauncher, NetworkCharacterPlayer의 동작은 변경하지 않습니다.
    public class NetworkSmokeRunner : MonoBehaviour
    {
        private string role;
        private string output;
        private Keyboard keyboard;
        private InputSettings.BackgroundBehavior originalBackgroundBehavior;
#if UNITY_EDITOR
        private InputSettings.EditorInputBehaviorInPlayMode originalEditorInputBehavior;
#endif
        private bool inputSettingsChanged;
        private readonly List<string> errors = new List<string>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            string[] args = Environment.GetCommandLineArgs();
            string requestedRole = Argument(args, "--ngo-smoke-role");
            if (requestedRole != "host" && requestedRole != "client")
            {
                return;
            }
            string requestedOutput = Argument(args, "--ngo-smoke-output");
            if (string.IsNullOrEmpty(requestedOutput))
            {
                Debug.LogError("[NgoSmoke] Missing output path");
                return;
            }
            Begin(requestedRole, requestedOutput);
        }

        // Editor Host + 빌드 Client 구성에서도 같은 검증 절차를 실행할 수 있습니다.
        public static void Begin(string requestedRole, string requestedOutput)
        {
            if (requestedRole != "host" && requestedRole != "client")
            {
                throw new ArgumentException("Expected host or client", "requestedRole");
            }
            var go = new GameObject("NetworkSmokeRunner");
            var runner = go.AddComponent<NetworkSmokeRunner>();
            runner.role = requestedRole;
            runner.output = Path.GetFullPath(requestedOutput);
        }

        private static string Argument(string[] args, string name)
        {
            for (int index = 0; index + 1 < args.Length; index++)
            {
                if (args[index] == name)
                {
                    return args[index + 1];
                }
            }
            return null;
        }

        private IEnumerator Start()
        {
            Application.logMessageReceived += OnLog;
            Application.runInBackground = true;
            // 가상 입력은 테스트 전용입니다. 실제 키보드 입력이나 사용자 창 초점에 의존하지 않습니다.
            originalBackgroundBehavior = InputSystem.settings.backgroundBehavior;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            originalEditorInputBehavior = InputSystem.settings.editorInputBehaviorInPlayMode;
            InputSystem.settings.editorInputBehaviorInPlayMode =
                InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
#endif
            inputSettingsChanged = true;
            yield return null;
            GameObject buttonObject = GameObject.Find(role == "host" ? "StartHostButton" : "StartClientButton");
            if (buttonObject == null)
            {
                Complete(false, "Start button missing", 0);
                yield break;
            }
            // 저장된 Button 콜백 연결을 통해 원본 Launcher를 실행합니다.
            buttonObject.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
            float deadline = Time.realtimeSinceStartup + 60;
            while (Players().Length != 2 && Time.realtimeSinceStartup < deadline)
            {
                yield return null;
            }
            if (Players().Length != 2)
            {
                Complete(false, "Two players did not spawn", 0);
                yield break;
            }

            yield return new WaitForSecondsRealtime(0.6f);
            keyboard = InputSystem.AddDevice<Keyboard>("NGO Smoke Keyboard");
            Key key = role == "host" ? Key.W : Key.D;
            // Host는 벽을 향해 충분히 오래 이동하고 Client는 오른쪽으로 이동합니다.
            float end = Time.realtimeSinceStartup + (role == "host" ? 1.5f : 1.0f);
            int inputFrames = 0;
            while (Time.realtimeSinceStartup < end)
            {
                InputSystem.QueueStateEvent(keyboard, new KeyboardState(key));
                inputFrames++;
                yield return null;
            }
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return null;
            InputSystem.RemoveDevice(keyboard);
            keyboard = null;
            // 마지막 위치가 다른 피어의 보간된 복제본까지 도착할 시간을 줍니다.
            yield return new WaitForSecondsRealtime(3f);

            NetworkCharacterPlayer hostPlayer = null;
            NetworkCharacterPlayer clientPlayer = null;
            int owners = 0;
            bool controllersCorrect = true;
            foreach (NetworkCharacterPlayer player in Players())
            {
                if (player.OwnerClientId == NetworkManager.ServerClientId)
                {
                    hostPlayer = player;
                }
                else
                {
                    clientPlayer = player;
                }
                if (player.IsOwner)
                {
                    owners++;
                }
                controllersCorrect &= player.GetComponent<CharacterController>().enabled == player.IsOwner;
            }
            bool positionsCorrect = hostPlayer != null && clientPlayer != null
                && Mathf.Abs(hostPlayer.transform.position.x + 2) < 0.2f
                && hostPlayer.transform.position.z > 2.5f && hostPlayer.transform.position.z < 3.3f
                && clientPlayer.transform.position.x > 4.5f
                && Mathf.Abs(clientPlayer.transform.position.z) < 0.2f
                && Mathf.Abs(hostPlayer.transform.position.y) < 0.25f
                && Mathf.Abs(clientPlayer.transform.position.y) < 0.25f;
            bool passed = positionsCorrect && owners == 1 && controllersCorrect
                && inputFrames >= 10 && errors.Count == 0;
            Complete(passed, passed ? "Connected, owned input, replicated movement and wall collision passed" : "Runtime checks failed", inputFrames);
            // 다른 프로세스가 결과를 기록하기 전에 연결이 끊기지 않도록 잠시 유지합니다.
            yield return new WaitForSecondsRealtime(20f);
            if (!Application.isEditor)
            {
                Application.Quit(passed ? 0 : 1);
            }
        }

        private NetworkCharacterPlayer[] Players()
        {
            return UnityEngine.Object.FindObjectsByType<NetworkCharacterPlayer>();
        }

        private void Complete(bool passed, string message, int inputFrames)
        {
            var snapshots = new List<PlayerSnapshot>();
            foreach (NetworkCharacterPlayer player in Players())
            {
                snapshots.Add(new PlayerSnapshot
                {
                    objectId = player.NetworkObjectId,
                    ownerId = player.OwnerClientId,
                    isOwner = player.IsOwner,
                    controllerEnabled = player.GetComponent<CharacterController>().enabled,
                    position = player.transform.position
                });
            }
            var report = new SmokeReport
            {
                passed = passed,
                role = role,
                message = message,
                frameCount = Time.frameCount,
                inputFrames = inputFrames,
                players = snapshots.ToArray(),
                errors = errors.ToArray()
            };
            Directory.CreateDirectory(Path.GetDirectoryName(output));
            File.WriteAllText(output, JsonUtility.ToJson(report, true));
            Debug.Log("[NgoSmoke] " + role + ": " + message);
        }

        private void OnLog(string message, string stackTrace, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
            {
                errors.Add(message);
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLog;
            if (inputSettingsChanged)
            {
                InputSystem.settings.backgroundBehavior = originalBackgroundBehavior;
#if UNITY_EDITOR
                InputSystem.settings.editorInputBehaviorInPlayMode = originalEditorInputBehavior;
#endif
            }
            if (keyboard != null)
            {
                InputSystem.RemoveDevice(keyboard);
            }
        }

        [Serializable]
        private class SmokeReport
        {
            public bool passed;
            public string role;
            public string message;
            public int frameCount;
            public int inputFrames;
            public PlayerSnapshot[] players;
            public string[] errors;
        }

        [Serializable]
        private class PlayerSnapshot
        {
            public ulong objectId;
            public ulong ownerId;
            public bool isOwner;
            public bool controllerEnabled;
            public Vector3 position;
        }
    }
}
