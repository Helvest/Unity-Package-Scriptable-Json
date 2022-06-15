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

		#region Fields

		public PathSystem pathSystem = default;

		public string prefixPath = default;

		public string path = default;

		public string suffixPath = default;

		public string linuxOverrideDirectPath = default;

		public string OSXOverrideDirectPath = default;

		public string fileName = default;

		public string extension = default;

		#endregion

		#region Get

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
				PathSystem.GameData => Path.Combine(Application.dataPath, prefixPath, path, suffixPath, fileName + extension),
				PathSystem.StreamingAssets => Path.Combine(Application.streamingAssetsPath, prefixPath, path, suffixPath, fileName + extension),
				PathSystem.PersistentData => Path.Combine(Application.persistentDataPath, prefixPath, path, suffixPath, fileName + extension),
				PathSystem.TemporaryCache => Path.Combine(Application.temporaryCachePath, prefixPath, path, suffixPath, fileName + extension),
				PathSystem.Resources => Path.Combine(prefixPath, path, suffixPath, fileName),
				PathSystem.ConsoleLog => Path.Combine(Application.consoleLogPath, prefixPath, path, suffixPath, fileName + extension),
				PathSystem.AbsoluteURL => Path.Combine(Application.absoluteURL, prefixPath, path, suffixPath, fileName + extension),
				_ => Path.Combine(prefixPath, GetDirectPath(), suffixPath, fileName + extension)
			};
		}

		public string GetDirectoryPath()
		{
			return pathSystem switch
			{
				PathSystem.GameData => Path.Combine(Application.dataPath, prefixPath, path, suffixPath),
				PathSystem.StreamingAssets => Path.Combine(Application.streamingAssetsPath, prefixPath, path, suffixPath),
				PathSystem.PersistentData => Path.Combine(Application.persistentDataPath, prefixPath, path, suffixPath),
				PathSystem.TemporaryCache => Path.Combine(Application.temporaryCachePath, prefixPath, path, suffixPath),
				PathSystem.Resources => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.ConsoleLog => Path.Combine(Application.consoleLogPath, prefixPath, path, suffixPath),
				PathSystem.AbsoluteURL => Path.Combine(Application.absoluteURL, prefixPath, path, suffixPath),
				_ => Path.Combine(prefixPath, GetDirectPath(), suffixPath)
			};
		}

		public string GetPartialPath()
		{
			return pathSystem switch
			{
				PathSystem.GameData => Path.Combine(prefixPath, path, suffixPath, fileName + extension),
				PathSystem.StreamingAssets => Path.Combine(prefixPath, path, suffixPath, fileName + extension),
				PathSystem.PersistentData => Path.Combine(prefixPath, path, suffixPath, fileName + extension),
				PathSystem.TemporaryCache => Path.Combine(prefixPath, path, suffixPath, fileName + extension),
				PathSystem.Resources => Path.Combine(prefixPath, path, suffixPath, fileName),
				PathSystem.ConsoleLog => Path.Combine(prefixPath, path, suffixPath, fileName + extension),
				PathSystem.AbsoluteURL => Path.Combine(prefixPath, path, suffixPath, fileName + extension),
				_ => Path.Combine(prefixPath, GetDirectPath(), suffixPath, fileName + extension)
			};
		}

		public string GetPartialDirectoryPath()
		{
			return pathSystem switch
			{
				PathSystem.GameData => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.StreamingAssets => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.PersistentData => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.TemporaryCache => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.Resources => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.ConsoleLog => Path.Combine(prefixPath, path, suffixPath),
				PathSystem.AbsoluteURL => Path.Combine(prefixPath, path, suffixPath),
				_ => Path.Combine(prefixPath, GetDirectPath(), suffixPath)
			};
		}

		public string GetSytemPath()
		{
			return pathSystem switch
			{
				PathSystem.GameData => Application.dataPath,
				PathSystem.StreamingAssets => Application.streamingAssetsPath,
				PathSystem.PersistentData => Application.persistentDataPath,
				PathSystem.TemporaryCache => Application.temporaryCachePath,
				PathSystem.Resources => string.Empty,
				PathSystem.ConsoleLog => Application.consoleLogPath,
				PathSystem.AbsoluteURL => Application.absoluteURL,
				_ => string.Empty
			};
		}

		public string GetFileName()
		{
			return fileName + extension;
		}

		public string GetFileNameWithoutExtension()
		{
			return fileName;
		}

		public string GetExtension()
		{
			return extension;
		}

		#endregion

		#region Set

		public void SetFromPartialPath(string partialPath)
		{
			path = Path.GetDirectoryName(partialPath);

			fileName = Path.GetFileName(partialPath);

			if (Path.HasExtension(partialPath))
			{
				extension = Path.GetExtension(partialPath);
			}
		}

		#endregion

	}
}
