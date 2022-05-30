using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromPath<T> : ScriptableJsonGeneric<T> where T : class, new()
	{

		#region Fields

		public PathData pathData = new PathData()
		{
			pathSystem = PathSystem.DirectPath,
			extension = ".json"
		};

		[Tooltip("If true, format the json for readability. If false, format the json for minimum size.")]
		public bool prettyPrint = false;

		public DebugLevel throwDebugIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		public override void LoadData()
		{
			string path = pathData.GetFullPath();

			if (TextFile.TryLoadText(pathData.pathSystem, path, out string json))
			{
				JsonUtility.FromJsonOverwrite(json, Data);
			}
			else
			{
				if (throwDebugIfNotFind != DebugLevel.None)
				{
					string getDebugText() => $"File: {pathData.fileName} not found at path: {path}";

					switch (throwDebugIfNotFind)
					{
						default:
						case DebugLevel.None:
							break;
						case DebugLevel.Normal:
							Debug.Log(getDebugText(), this);
							break;
						case DebugLevel.Warning:
							Debug.LogWarning(getDebugText(), this);
							break;
						case DebugLevel.Error:
							Debug.LogError(getDebugText(), this);
							break;
					}
				}
			}
		}

		#endregion

		#region SaveData

		public void SaveData()
		{
			if (pathData.pathSystem == PathSystem.Resources)
			{
				Debug.LogError("Can't write in Resources's folder");
				return;
			}

			string json = JsonUtility.ToJson(Data, prettyPrint);

			TextFile.TrySaveText(pathData.pathSystem, pathData.GetFullPath(), json);


			#endregion

		}
	}
}
