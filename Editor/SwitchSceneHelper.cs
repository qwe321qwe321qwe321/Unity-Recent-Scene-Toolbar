using UnityEditor;
using UnityEditor.SceneManagement;

namespace RecentSceneToolbar {
    static class SwitchSceneHelper {
        static string scenePathToOpen;

        public static void StartScene(string scenePath) {
            if (EditorApplication.isPlaying) {
                EditorApplication.isPlaying = false;
            }

            scenePathToOpen = scenePath;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate() {
            if (scenePathToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }

            EditorApplication.update -= OnUpdate;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorSceneManager.OpenScene(scenePathToOpen);
            }
            scenePathToOpen = null;
        }
    }
}