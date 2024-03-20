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
			var sceneIconTex = EditorGUIUtility.IconContent(@"d_BuildSettings.SelectedIcon").image;
			style.stretchWidth = false;
			if (GUILayout.Button(new GUIContent(title, sceneIconTex, $"Switch recent scenes"), style)) {
				PopupRecentScene(
					(sceneIndex) => {
						RecentSceneList.Instance.LoadScene(sceneIndex);
					});
			}
			
			
			var plusButtonTex = EditorGUIUtility.IconContent(@"Toolbar Plus").image;
			if (GUILayout.Button(new GUIContent(null, plusButtonTex, $"Additively load recent scenes"), style)) {
				PopupRecentScene(
					(sceneIndex) => {
						RecentSceneList.Instance.LoadScene(sceneIndex, true);
					});
			}
		}

		private static void PopupRecentScene(System.Action<int> onSceneSelected) {
			var menu = new GenericMenu();
			var rs = RecentSceneList.Instance;
			rs.RemoveInvalid();
			int count = rs.Count;
			if (count <= 1) {
				menu.AddDisabledItem(new GUIContent("Empty"));
			}
			// Ignore the first one.
			for (int i = 1; i < count; i++) {
				int index = i;
				string name = rs[i];
				if (!string.IsNullOrEmpty(name)) {
					var label = $"{i}. {name}";
					menu.AddItem(new GUIContent(label), false, () => { onSceneSelected.Invoke(index); });
				} else {
					menu.AddDisabledItem(new GUIContent($"{i}. None"));
                }
			}
			menu.ShowAsContext();
		}
	}
}