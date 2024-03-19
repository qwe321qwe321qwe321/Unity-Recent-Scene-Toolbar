using UnityEditor;
using UnityEditor.SceneManagement;

namespace RecentSceneToolbar {
    static class SwitchSceneHelper {
        private static string scenePathToOpen;
        private static bool isAdditiveLoad;

        public static void StartScene(string scenePath, bool additiveLoad = false) {
            if(EditorApplication.isPlaying) {
                EditorApplication.isPlaying = false;
            }

            scenePathToOpen = scenePath;
            isAdditiveLoad = additiveLoad;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate() {
            if (scenePathToOpen == null ||
                EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode) {
                return;
            }

            EditorApplication.update -= OnUpdate;

            SwitchScene();
			
            scenePathToOpen = null;
            isAdditiveLoad = false;
        }

        static void SwitchScene() {
            if (isAdditiveLoad) {
                // not open but load additional scene into current scene.
                EditorSceneManager.OpenScene(scenePathToOpen, OpenSceneMode.Additive);
                return;
            }
			
            if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePathToOpen);
            }
        }
    }
}