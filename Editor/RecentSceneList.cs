using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace RecentSceneToolbar {
	public class RecentSceneList : ScriptableObject {
		public const int MaxRecentSceneCount = 10;

		private static RecentSceneList s_Instance;
		public static RecentSceneList Instance {
			get {
				if (!s_Instance) {
					var guids = AssetDatabase.FindAssets($"t: RecentSceneList");
					if (guids.Length <= 0) {
						Debug.Log("Cannot find any RecentSceneList.asset. Trying to create one...");
						var scriptGuids = AssetDatabase.FindAssets($"t: script RecentSceneList");
						if (scriptGuids.Length <= 0) {
							Debug.LogWarning("Cannot find RecentSceneList.cs!");
						} else {
							string scriptPath = AssetDatabase.GUIDToAssetPath(scriptGuids[0]);
							string path = scriptPath.Replace(".cs", ".asset");
							var newInstance = ScriptableObject.CreateInstance<RecentSceneList>();
							AssetDatabase.CreateAsset(newInstance, path);
							Debug.Log($"Created {path}!");
							s_Instance = AssetDatabase.LoadAssetAtPath<RecentSceneList>(path);

						}
					} else {
						string path = AssetDatabase.GUIDToAssetPath(guids[0]);
						s_Instance = AssetDatabase.LoadAssetAtPath<RecentSceneList>(path);
					}
				}
				return s_Instance;
			}
		}

		public string this[int index] { get { return GetSceneName(index); } }
		public int Count { get { return m_List.Count; } }

		[SerializeField]
		private List<string> m_List = new List<string>();

		private string GetSceneName(int index) {
			var guid = m_List[index];
			var path = AssetDatabase.GUIDToAssetPath(guid);
			if (string.IsNullOrEmpty(path)) return null;
			var name = Path.GetFileNameWithoutExtension(path);
			return name;
		}

		public void Add(Scene scene, bool setDirty = true) {
			if (string.IsNullOrEmpty(scene.path)) return;
			var guid = AssetDatabase.AssetPathToGUID(scene.path);

			while (m_List.Contains(guid)) {
				m_List.Remove(guid);
			}
			if (m_List.Count >= MaxRecentSceneCount) {
				m_List.RemoveAt(m_List.Count - 1);
			}
			m_List.Insert(0, guid);

			if (setDirty) {
				EditorUtility.SetDirty(this);
			}
		}

		public void LoadScene(int index) {
			var guid = m_List[index];
			var path = AssetDatabase.GUIDToAssetPath(guid);
			if (!PathIsValid(path)) return;
			SwitchSceneHelper.StartScene(path);
		}

		public void RemoveInvalid() {
			m_List = m_List.Where(id => PathIsValid(AssetDatabase.GUIDToAssetPath(id))).ToList();
		}

		private bool PathIsValid(string path) {
			if (string.IsNullOrEmpty(path)) return false;
			return File.Exists(path);
		}
	}
}
