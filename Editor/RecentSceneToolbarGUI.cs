using UnityEditor;
using UnityEditor.SceneManagement;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityToolbarExtender;

namespace RecentSceneToolbar {

	[InitializeOnLoad]
	public class RecentSceneToolbarGUI {
		private static string s_CurrentSceneName;

		static RecentSceneToolbarGUI() {
			EditorSceneManager.activeSceneChangedInEditMode += EditorSceneManager_activeSceneChangedInEditMode;
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
		}

		private static void EditorSceneManager_activeSceneChangedInEditMode(Scene current, Scene next) {
			s_CurrentSceneName = $"{next.name}.unity";
			RecentSceneList.Instance.Add(next);
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnReloadScripts() {
			Scene currentScene = EditorSceneManager.GetActiveScene();
			s_CurrentSceneName = $"{currentScene.name}.unity";
		}

		internal static void OnToolbarGUI() {
			GUILayout.Space(80f);
			string title = string.IsNullOrEmpty(s_CurrentSceneName) ? "Unknown Scene" : s_CurrentSceneName;
			GUIStyle style = new GUIStyle(GUI.skin.button);
			Texture tex = EditorGUIUtility.IconContent(@"d_BuildSettings.SelectedIcon").image;
			style.stretchWidth = false;
			if (GUILayout.Button(new GUIContent(title, tex, $"Switch recent scenes"), style)) {
				var currentScene = EditorSceneManager.GetActiveScene();
				RecentSceneList.Instance.Add(currentScene);
				PopupRecentScene();
			}
		}

		private static void PopupRecentScene() {
			var menu = new GenericMenu();
			var rs = RecentSceneList.Instance;
			rs.RemoveInvalid();
			var count = rs.Count;
			if (count <= 1) {
				menu.AddDisabledItem(new GUIContent("Empty"));
			}
			// Ignore the first one.
			for (int i = 1; i < count; i++) {
				var index = i;
				var name = rs[i];
				if (!string.IsNullOrEmpty(name)) {
					var label = $"{i}. {name}";
					menu.AddItem(new GUIContent(label), false, () => { RecentSceneList.Instance.LoadScene(index); });
				} else {
					menu.AddDisabledItem(new GUIContent($"{i}. None"));
                }
			}
			menu.ShowAsContext();
		}
	}
}