using System.IO;
using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromPath<T> : ScriptableJsonGeneric<T> where T : class, new()
	{

		#region Variables

		[SerializeField]
		private PathSystem _pathSystem = PathSystem.DirectPath;

		[SerializeField]
		private string _path = string.Empty;

		[SerializeField]
		private string _linuxOverridePath = string.Empty;

		[SerializeField]
		private string _OSXOverridePath = string.Empty;

		[SerializeField]
		private string _fileName = string.Empty;

		private const string EXTENSION = ".json";

		[Tooltip("If true, format the json for readability. If false, format the json for minimum size.")]
		[SerializeField]
		protected bool prettyPrint = false;

		[SerializeField]
		private DebugLevel _throwDebugIfNotFind = DebugLevel.Warning;

		#endregion

		#region GetPath

		public string GetPath()
		{
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			return string.IsNullOrEmpty(_linuxOverridePath) ? _path : _linuxOverridePath;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			return string.IsNullOrEmpty(_OSXOverridePath) ? _path : _OSXOverridePath;
#else
			return _path;
#endif
		}

		#endregion

		#region LoadData

		public override void LoadData()
		{
			var path = GetPath();

			if (TextFile.TryLoadText(_pathSystem, Path.Combine(path, _fileName), EXTENSION, out string json))
			{
				JsonUtility.FromJsonOverwrite(json, Data);
			}
			else
			{
				if (_throwDebugIfNotFind != DebugLevel.None)
				{
					string getDebugText() => $"File: {_fileName} not found at path: {path}";

					switch (_throwDebugIfNotFind)
					{
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
			if (_pathSystem == PathSystem.Resources)
			{
				Debug.LogError("Can't write in Resources's folder");
				return;
			}

			var json = JsonUtility.ToJson(Data, prettyPrint);

			TextFile.TrySaveText(_pathSystem, Path.Combine(GetPath(), _fileName), EXTENSION, json);
		}

		#endregion

	}
}
