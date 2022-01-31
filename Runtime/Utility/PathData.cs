using System;
using System.IO;
using UnityEngine;

namespace ScriptableJson
{

	#region Enum

	public enum PathSystem
	{
		DirectPath,
		GameData,
		StreamingAssets,
		PersistentData,
		TemporaryCache,
		Resources,
		ConsoleLog,
		AbsoluteURL
	}

	#endregion

	[Serializable]
	public class PathData
	{

		#region Variables

		public PathSystem pathSystem = default;

		public string path = default;

		public string linuxOverrideDirectPath = default;

		public string OSXOverrideDirectPath = default;

		public string fileName = default;

		public string extension = default;

		#endregion

		#region Methods

		private string GetDirectPath()
		{
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
		return string.IsNullOrEmpty(linuxOverrideDirectPath) ? path : linuxOverrideDirectPath;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		return string.IsNullOrEmpty(OSXOverrideDirectPath) ? path : OSXOverrideDirectPath;
#else
			return path;
#endif
		}

		public string GetFullPath()
		{
			return pathSystem switch
			{
				PathSystem.GameData => Path.Combine(Application.dataPath, path, fileName + extension),
				PathSystem.StreamingAssets => Path.Combine(Application.streamingAssetsPath, path, fileName + extension),
				PathSystem.PersistentData => Path.Combine(Application.persistentDataPath, path, fileName + extension),
				PathSystem.TemporaryCache => Path.Combine(Application.temporaryCachePath, path, fileName + extension),
				PathSystem.Resources => Path.Combine(path, fileName),
				PathSystem.ConsoleLog => Path.Combine(Application.consoleLogPath, path, fileName + extension),
				PathSystem.AbsoluteURL => Path.Combine(Application.absoluteURL, path, fileName + extension),
				_ => Path.Combine(GetDirectPath(), fileName + extension)
			};
		}

		public string GetDirectoryPath()
		{


			return pathSystem switch
			{
				PathSystem.GameData => Path.Combine(Application.dataPath, path),
				PathSystem.StreamingAssets => Path.Combine(Application.streamingAssetsPath, path),
				PathSystem.PersistentData => Path.Combine(Application.persistentDataPath, path),
				PathSystem.TemporaryCache => Path.Combine(Application.temporaryCachePath, path),
				PathSystem.Resources => path,
				PathSystem.ConsoleLog => Path.Combine(Application.consoleLogPath, path),
				PathSystem.AbsoluteURL => Path.Combine(Application.absoluteURL, path),
				_ => GetDirectPath(),
			};
		}

		#endregion

	}
}