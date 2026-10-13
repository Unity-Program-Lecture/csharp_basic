using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace NgoLab.Editor
{
    // Package Manager 요청은 비동기이므로 Editor update를 기다린 뒤 종료합니다.
    // 실행 시 -quit을 붙이지 않습니다. 이 클래스가 완료 후 직접 종료합니다.
    public static class PackageInstaller
    {
        private static AddAndRemoveRequest request;
        private static double deadline;

        public static void Install()
        {
            string[] packages = { "com.unity.netcode.gameobjects@2.13.3" };
            Debug.Log("[NgoLabPackages] Installing " + string.Join(", ", packages));
            request = Client.AddAndRemove(packagesToAdd: packages,
                packagesToRemove: Array.Empty<string>());
            deadline = EditorApplication.timeSinceStartup + 600;
            EditorApplication.update += Poll;
        }

        private static void Poll()
        {
            if (request == null)
            {
                return;
            }
            if (!request.IsCompleted)
            {
                if (EditorApplication.timeSinceStartup > deadline)
                {
                    EditorApplication.update -= Poll;
                    Debug.LogError("[NgoLabPackages] Timed out");
                    EditorApplication.Exit(2);
                }
                return;
            }

            EditorApplication.update -= Poll;
            if (request.Status != StatusCode.Success)
            {
                Debug.LogError("[NgoLabPackages] Failed: " + request.Error.message);
                EditorApplication.Exit(1);
                return;
            }
            Debug.Log("[NgoLabPackages] Resolved: " + string.Join(", ",
                request.Result.Select(package => package.name + "@" + package.version)));
            EditorApplication.Exit(0);
        }
    }
}
