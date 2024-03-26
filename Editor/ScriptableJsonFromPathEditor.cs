using UnityEditor;
using UnityEngine;

namespace ScriptableJson
{
	[CustomEditor(typeof(ScriptableJsonFromPath<,>), true)]
	public class ScriptableJsonFromPathEditor : Editor
	{
		private const float BUTTON_WIDTH = 110f;
		private const float BUTTON_HEIGHT = 20f;

		public override void OnInspectorGUI()
		{
			var scriptableJson = (IHavePath)target;

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Open Path Folder", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
				{
					OpenPathInExplorer(scriptableJson.PathToString());
				}

				if (GUILayout.Button("Save Data", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
				{
					scriptableJson.SaveData();
				}

				if (GUILayout.Button("Load Data", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
				{
					scriptableJson.LoadData();
				}

				if (GUILayout.Button("Set To Default", GUILayout.Width(BUTTON_WIDTH), GUILayout.Height(BUTTON_HEIGHT)))
				{
					scriptableJson.SetDataToDefault();
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(10f); // Ajoute un espace après les boutons

			base.OnInspectorGUI();
		}

		private void OpenPathInExplorer(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogWarning("Path is empty.");
				return;
			}

			path = path.Replace(@"/", @"\");
			Debug.Log("Open: " + path);

#if UNITY_EDITOR_WIN
			System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("explorer.exe", "/select," + path));
#elif UNITY_EDITOR_OSX
            System.Diagnostics.Process.Start("open", "-R " + fullPath);
#else
            Debug.LogWarning("Unsupported platform.");
#endif
		}
	}
}
