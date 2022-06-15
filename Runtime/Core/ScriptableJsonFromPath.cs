using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromPath<T> : ScriptableJsonGeneric<T>
	{

		#region Fields

		[Space]
		public PathData pathData = new PathData()
		{
			pathSystem = PathSystem.DirectPath,
			extension = ".json"
		};

		[Space]
		public DebugLevel throwDebugLogIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		/// <summary>
		/// Load the json data from the path define in pathData
		/// </summary>
		public override void LoadData()
		{
			string path = pathData.GetFullPath();

			if (TextFile.TryLoadText(pathData.pathSystem, path, out string json))
			{
				JsonUtility.FromJsonOverwrite(json, Data);
			}
			else
			{
				if (throwDebugLogIfNotFind != DebugLevel.None)
				{
					string getDebugText() => $"File: {pathData.fileName} not found at path: {path}";

					switch (throwDebugLogIfNotFind)
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

		/// <summary>
		/// Save the data to json file
		/// </summary>
		/// <param name="prettyPrint">If true, format the json for readability. If false, format the json for minimum size</param>
		public void SaveData(bool prettyPrint = true)
		{
			if (pathData.pathSystem == PathSystem.Resources)
			{
				Debug.LogError("Can't write in Resources's folder");
				return;
			}

			string json = JsonUtility.ToJson(Data, prettyPrint);

			TextFile.TrySaveText(pathData.pathSystem, pathData.GetFullPath(), json);
		}

		#endregion

		#region ToString

		public string PathToString()
		{
			return pathData.GetFullPath();
		}

		#endregion

	}
}
