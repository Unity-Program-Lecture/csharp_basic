using System;
using System.IO;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Netcode.Transports.UTP;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.TextCore.LowLevel;

namespace NgoLab.Editor
{
    // 학생용 런타임 코드와 분리한 씬 구성 도구입니다. Editor API가 GUID와 참조를 저장합니다.
    public static class NgoLabBuilder
    {
        public const string ScenePath = "Assets/Scenes/NetworkPlay.unity";
        private const string PrefabPath = "Assets/Prefabs/PlayerNetworkPrefab.prefab";

        [MenuItem("NGO Lab/Create Practice Scene")]
        public static void CreatePracticeScene()
        {
            if (Application.isPlaying || File.Exists(ScenePath) || File.Exists(PrefabPath))
            {
                throw new InvalidOperationException("기존 실습 씬/프리팹을 덮어쓰지 않습니다.");
            }
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                throw new InvalidOperationException("현재 씬의 변경을 먼저 저장하세요.");
            }

            EnsureFolder("Assets/Scenes");
            EnsureFolder("Assets/Prefabs");
            EnsureFolder("Assets/Materials");
            EnsureFolder("Assets/Fonts");
            EnsureFolder("Assets/Settings");
            QualitySettings.SetQualityLevel(Array.IndexOf(QualitySettings.names, "PC"), true);
            UniversalRenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
            if (pipeline == null)
            {
                throw new InvalidOperationException("URP 템플릿의 렌더 파이프라인을 먼저 확인하세요.");
            }
            pipeline.supportsHDR = true;
            EditorUtility.SetDirty(pipeline);

            TMP_FontAsset font = CreateKoreanFont();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateWorld();
            GameObject playerPrefab = CreatePlayerPrefab();

            var managerObject = new GameObject("NetworkManager");
            UnityTransport transport = managerObject.AddComponent<UnityTransport>();
            transport.SetConnectionData("127.0.0.1", 7777, "127.0.0.1");
            NetworkManager manager = managerObject.AddComponent<NetworkManager>();
            manager.NetworkConfig.NetworkTransport = transport;
            manager.NetworkConfig.PlayerPrefab = playerPrefab;
            manager.NetworkConfig.EnableSceneManagement = true;
            manager.NetworkConfig.ConnectionApproval = false;
            manager.NetworkConfig.TickRate = 30;
            var list = ScriptableObject.CreateInstance<NetworkPrefabsList>();
            list.Add(new NetworkPrefab { Prefab = playerPrefab });
            AssetDatabase.CreateAsset(list, "Assets/Settings/PracticeNetworkPrefabs.asset");
            manager.NetworkConfig.Prefabs.NetworkPrefabsLists.Clear();
            manager.NetworkConfig.Prefabs.NetworkPrefabsLists.Add(list);

            var launcherObject = new GameObject("NetworkLauncher");
            NetworkLauncher launcher = launcherObject.AddComponent<NetworkLauncher>();
            CreateUi(launcher, font);
            PlayerSettings.productName = "NGO Network Practice";
            PlayerSettings.runInBackground = true;
            PlayerSettings.defaultScreenWidth = 1100;
            PlayerSettings.defaultScreenHeight = 720;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Selection.activeGameObject = managerObject;
            Debug.Log("[NgoLab] Scene created: " + ScenePath);
        }

        private static TMP_FontAsset CreateKoreanFont()
        {
            Font source = AssetDatabase.LoadAssetAtPath<Font>("Assets/Fonts/NotoSansKR.ttf");
            if (source == null)
            {
                throw new InvalidOperationException("NotoSansKR.ttf를 먼저 가져오세요.");
            }
            TMP_FontAsset asset = TMP_FontAsset.CreateFontAsset(source, 90, 9,
                GlyphRenderMode.SDFAA, 2048, 2048, AtlasPopulationMode.Dynamic, false);
            if (asset == null)
            {
                throw new InvalidOperationException("한국어 TMP 폰트를 만들지 못했습니다.");
            }
            asset.name = "NgoKoreanUI";
            string characters = "NGO 네트워크 실습 연결 대기 시작 실패 시도 완료 실행 참여자 명 Host Client WASD 또는 방향키 내 캐릭터만 조작 먼저 한 창에서 호스트 다른 클라이언트";
            for (int code = 32; code < 127; code++)
            {
                characters += (char)code;
            }
            string missing;
            if (!asset.TryAddCharacters(characters, out missing))
            {
                throw new InvalidOperationException("폰트에서 찾을 수 없는 글자: " + missing);
            }
            // 실습에서 사용할 글자를 미리 포함하므로 Player에서도 같은 글꼴을 표시합니다.
            asset.atlasPopulationMode = AtlasPopulationMode.Static;
            AssetDatabase.CreateAsset(asset, "Assets/Fonts/NgoKoreanUI.asset");
            AssetDatabase.AddObjectToAsset(asset.material, asset);
            foreach (Texture2D texture in asset.atlasTextures)
            {
                AssetDatabase.AddObjectToAsset(texture, asset);
            }
            EditorUtility.SetDirty(asset);
            return asset;
        }

        private static void CreateWorld()
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(2, 1, 2);
            ground.GetComponent<Renderer>().sharedMaterial = CreateMaterial("Ground", new Color(0.24f, 0.31f, 0.37f));
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = "Wall";
            wall.transform.position = new Vector3(0, 1, 4);
            wall.transform.localScale = new Vector3(6, 2, 1);
            wall.GetComponent<Renderer>().sharedMaterial = CreateMaterial("Wall", new Color(0.94f, 0.52f, 0.27f));

            var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0, 10, -10);
            cameraObject.transform.rotation = Quaternion.Euler(45, 0, 0);
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 8;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.08f, 0.11f, 0.16f);
            UniversalAdditionalCameraData data = cameraObject.AddComponent<UniversalAdditionalCameraData>();
            data.renderPostProcessing = true;
            data.volumeLayerMask = 1;

            var lightObject = new GameObject("Directional Light", typeof(Light));
            lightObject.transform.rotation = Quaternion.Euler(50, -30, 0);
            Light light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 2;
            light.shadows = LightShadows.Soft;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.45f, 0.5f, 0.6f);

            var profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.Add<Tonemapping>(true).mode.value = TonemappingMode.ACES;
            Bloom bloom = profile.Add<Bloom>(true);
            bloom.intensity.value = 0.5f;
            bloom.threshold.value = 0.9f;
            profile.Add<Vignette>(true).intensity.value = 0.25f;
            AssetDatabase.CreateAsset(profile, "Assets/Settings/PracticeVolume.asset");
            // 효과도 프로필의 하위 에셋으로 저장해야 재컴파일 후 참조가 유지됩니다.
            foreach (VolumeComponent component in profile.components)
            {
                AssetDatabase.AddObjectToAsset(component, profile);
            }
            EditorUtility.SetDirty(profile);
            var volumeObject = new GameObject("Global Volume", typeof(Volume));
            Volume volume = volumeObject.GetComponent<Volume>();
            volume.isGlobal = true;
            volume.sharedProfile = profile;
        }

        private static GameObject CreatePlayerPrefab()
        {
            var root = new GameObject("PlayerNetworkPrefab");
            root.AddComponent<NetworkObject>();
            CharacterController controller = root.AddComponent<CharacterController>();
            controller.height = 2;
            controller.radius = 0.5f;
            controller.center = new Vector3(0, 1, 0);
            controller.skinWidth = 0.08f;
            controller.stepOffset = 0.3f;
            controller.minMoveDistance = 0;
            NetworkTransform transform = root.AddComponent<NetworkTransform>();
            transform.AuthorityMode = NetworkTransform.AuthorityModes.Owner;
            transform.SyncPositionX = transform.SyncPositionY = transform.SyncPositionZ = true;
            transform.SyncRotAngleX = transform.SyncRotAngleZ = false;
            transform.SyncRotAngleY = true;
            transform.SyncScaleX = transform.SyncScaleY = transform.SyncScaleZ = false;
            transform.Interpolate = true;
            transform.InLocalSpace = false;
            root.AddComponent<NetworkCharacterPlayer>();
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(root.transform, false);
            body.transform.localPosition = new Vector3(0, 1, 0);
            UnityEngine.Object.DestroyImmediate(body.GetComponent<CapsuleCollider>());
            body.GetComponent<Renderer>().sharedMaterial = CreateMaterial("Player", new Color(0.2f, 0.7f, 0.96f));
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(root, PrefabPath);
            UnityEngine.Object.DestroyImmediate(root);
            return prefab;
        }

        private static void CreateUi(NetworkLauncher launcher, TMP_FontAsset font)
        {
            var canvasObject = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas),
                typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
            canvasObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            UnityEngine.UI.CanvasScaler scaler = canvasObject.GetComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1100, 720);
            scaler.matchWidthOrHeight = 0.5f;
            RectTransform panel = CreateRect("ConnectionPanel", canvasObject.transform, new Vector2(490, 205));
            panel.anchorMin = panel.anchorMax = panel.pivot = new Vector2(0, 1);
            panel.anchoredPosition = new Vector2(20, -20);
            var background = panel.gameObject.AddComponent<UnityEngine.UI.Image>();
            background.color = new Color(0.05f, 0.075f, 0.12f, 0.94f);
            background.raycastTarget = false;
            CreateText("Title", panel, font, "NGO 네트워크 실습", new Vector2(20, -12), new Vector2(450, 38), 27);
            TextMeshProUGUI status = CreateText("StatusText", panel, font, "연결 대기", new Vector2(20, -55), new Vector2(450, 32), 22);
            CreateText("HelpText", panel, font, "WASD 또는 방향키 / 내 캐릭터만 조작", new Vector2(20, -163), new Vector2(450, 30), 18);
            UnityEngine.UI.Button host = CreateButton("StartHostButton", panel, font, "Host 시작", new Vector2(20, -104), new Color(0.16f, 0.44f, 0.68f));
            UnityEngine.UI.Button client = CreateButton("StartClientButton", panel, font, "Client 시작", new Vector2(252, -104), new Color(0.45f, 0.27f, 0.62f));
            UnityEventTools.AddPersistentListener(host.onClick, launcher.StartHost);
            UnityEventTools.AddPersistentListener(client.onClick, launcher.StartClient);
            var settings = new SerializedObject(launcher);
            settings.FindProperty("statusText").objectReferenceValue = status;
            settings.ApplyModifiedPropertiesWithoutUndo();
            var events = new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
            events.GetComponent<InputSystemUIInputModule>().AssignDefaultActions();
        }

        private static UnityEngine.UI.Button CreateButton(string name, Transform parent, TMP_FontAsset font, string label, Vector2 position, Color color)
        {
            RectTransform rect = CreateRect(name, parent, new Vector2(218, 46));
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = position;
            var image = rect.gameObject.AddComponent<UnityEngine.UI.Image>();
            image.color = color;
            var button = rect.gameObject.AddComponent<UnityEngine.UI.Button>();
            button.targetGraphic = image;
            TextMeshProUGUI text = CreateText("Label", rect, font, label, Vector2.zero, new Vector2(218, 46), 22);
            text.alignment = TextAlignmentOptions.Center;
            return button;
        }

        private static TextMeshProUGUI CreateText(string name, Transform parent, TMP_FontAsset font, string value, Vector2 position, Vector2 size, float fontSize)
        {
            RectTransform rect = CreateRect(name, parent, size);
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = position;
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.font = font;
            text.fontSize = fontSize;
            text.text = value;
            text.color = Color.white;
            text.raycastTarget = false;
            return text;
        }

        private static RectTransform CreateRect(string name, Transform parent, Vector2 size)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            return rect;
        }

        private static Material CreateMaterial(string name, Color color)
        {
            Material template = GraphicsSettings.currentRenderPipeline.defaultMaterial;
            if (template == null)
            {
                throw new InvalidOperationException("URP 기본 Material이 없습니다.");
            }
            var material = new Material(template) { name = name };
            material.SetColor("_BaseColor", color);
            AssetDatabase.CreateAsset(material, "Assets/Materials/" + name + ".mat");
            return material;
        }

        private static void EnsureFolder(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(Path.GetDirectoryName(path).Replace('\\', '/'), Path.GetFileName(path));
            }
        }
    }
}
