using System.IO;
using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromPath<T> : ScriptableJsonGeneric<T> where T : class, new()
	{
		[SerializeField]
		private LoadText.PathSystem _pathSystem = LoadText.PathSystem.DirectPath;

		[SerializeField]
		private string _path = string.Empty;

		[SerializeField]
		private string _linuxOverridePath = string.Empty;

		[SerializeField]
		private string _OSXOverridePath = string.Empty;

		[SerializeField]
		private string _fileName = string.Empty;

		private const string _extension = ".json";

		[SerializeField]
		private DebugLevel _throwDebugIfNotFind = DebugLevel.Warning;

		private enum DebugLevel
		{
			None,
			Normal,		
			Warning,
			Error
		}

		protected override void LoadData()
		{
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			var path = string.IsNullOrEmpty(_linuxOverridePath) ? _path : _linuxOverridePath;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
			var path = string.IsNullOrEmpty(_OSXOverridePath) ? _path : _OSXOverridePath;
#else
			var path = _path;
#endif

			if (LoadText.TryLoadText(_pathSystem, Path.Combine(path, _fileName), _extension, out string json))
			{
				JsonUtility.FromJsonOverwrite(json, loadedData);
			}
			else
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
}
