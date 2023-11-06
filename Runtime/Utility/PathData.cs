using System;
using System.IO;
using UnityEngine;

namespace ScriptableJson
{

	#region Enum

	public enum PathSystem
	{
		None,
		GameData,
		StreamingAssets,
		PersistentData,
		TemporaryCache,
		Resources,
		ConsoleLog,
		AbsoluteURL,
		CustomPathSystem
	}

	#endregion

	#region PathData

	[Serializable]
	public class PathData
	{

		#region Fields

		[field: SerializeField]
		public virtual PathSystem PathSystem { get; set; } = PathSystem.StreamingAssets;

		[field: SerializeField]
		public virtual string CustomPathSystem { get; set; } = string.Empty;

		[field: SerializeField]
		public virtual string SubPath { get; set; } = string.Empty;

		[field: SerializeField]
		public virtual string FileName { get; set; } = string.Empty;

		[SerializeField]
		private string _extension = string.Empty;

		public virtual string Extension
		{
			get => _extension;
			set
			{
				if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
				{
					_extension = string.Empty;
					return;
				}

				_extension = value[0] == '.' ? value : $".{value}";
			}
		}

		public virtual string FileNameWithExtension { get => FileName + Extension; }

		#endregion

		#region Init

		public PathData() { }

		public PathData(string fullPath)
		{
			SetFromFullPath(fullPath);
		}

		public PathData(PathData pathData)
		{
			Copy(pathData);
		}

		#endregion

		#region PathToPathSystem

		private static readonly PathSystem[] _pathSystemCheckArray = new PathSystem[]
		{
			PathSystem.StreamingAssets,
			PathSystem.GameData,
			PathSystem.PersistentData,
			PathSystem.TemporaryCache,
			PathSystem.ConsoleLog,
			PathSystem.AbsoluteURL,
			PathSystem.CustomPathSystem
		};

		public PathSystem PathToPathSystem(string path)
		{
			foreach (var system in _pathSystemCheckArray)
			{
				var systemPath = GetSytemPath(system);

				if (systemPath.Length != 0 && path.Contains(systemPath))
				{
					return system;
				}
			}

			return PathSystem.None;
		}

		#endregion

		#region Get

		public virtual string GetSytemPath()
		{
			return GetSytemPath(PathSystem);
		}

		public virtual string GetSytemPath(PathSystem system)
		{
			return system == PathSystem.CustomPathSystem ? CustomPathSystem : GetSytemPathDefault(system);
		}

		public static string GetSytemPathDefault(PathSystem system)
		{
			return system switch
			{
				PathSystem.GameData => Application.dataPath,
				PathSystem.StreamingAssets => Application.streamingAssetsPath,
				PathSystem.PersistentData => Application.persistentDataPath,
				PathSystem.TemporaryCache => Application.temporaryCachePath,
				PathSystem.ConsoleLog => Application.consoleLogPath,
				PathSystem.AbsoluteURL => Application.absoluteURL,
				_ => string.Empty
			};
		}

		public virtual string GetFullPath()
		{
			return PathSystem switch
			{
				PathSystem.None => Path.Combine(SubPath, FileNameWithExtension),
				PathSystem.Resources => Path.Combine(SubPath, FileName),
				_ => Path.Combine(GetSytemPath(), SubPath, FileNameWithExtension)
			};
		}

		public virtual string GetDirectoryPath()
		{
			return PathSystem switch
			{
				PathSystem.None => SubPath,
				PathSystem.Resources => SubPath,
				_ => Path.Combine(GetSytemPath(), SubPath)
			};
		}

		public virtual string GetPartialPath()
		{
			return PathSystem switch
			{
				PathSystem.Resources => Path.Combine(SubPath, FileName),
				_ => Path.Combine(SubPath, FileNameWithExtension)
			};
		}

		#endregion

		#region Set

		public virtual void SetFromFullPath(params string[] fullPath)
		{
			SetFromFullPath(Path.Combine(fullPath));
		}

		public virtual void SetFromFullPath(string fullPath)
		{
			PathSystem = PathToPathSystem(fullPath);

			if (PathSystem != PathSystem.None)
			{
				var systemPath = GetSytemPath(PathSystem).Length;
				fullPath = fullPath.Remove(0, systemPath + 1);
			}

			SetFromPartialPath(fullPath);
		}

		public virtual void SetFromPartialPath(string partialPath)
		{
			SubPath = Path.GetDirectoryName(partialPath);
			FileName = Path.GetFileNameWithoutExtension(partialPath);

			if (Path.HasExtension(partialPath))
			{
				Extension = Path.GetExtension(partialPath);
			}
		}

		#endregion

		#region Copy

		public virtual void Copy(PathData pathData)
		{
			PathSystem = pathData.PathSystem;
			CustomPathSystem = pathData.CustomPathSystem;
			SubPath = pathData.SubPath;
			FileName = pathData.FileName;
			Extension = pathData.Extension;
		}

		#endregion

	}

	#endregion

	#region PathDataSystemOverride

	[Serializable]
	public class PathDataSystemOverride : PathData
	{

		public string linuxOverrideDirectPath = default;

		public string OSXOverrideDirectPath = default;

		public override string SubPath
		{
			get
			{
#if DEVELOPMENT_BUILD_LINUX || UNITY_STANDALONE_LINUX
				return string.IsNullOrEmpty(linuxOverrideDirectPath) ? path : linuxOverrideDirectPath;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
				return string.IsNullOrEmpty(OSXOverrideDirectPath) ? path : OSXOverrideDirectPath;
#else
				return base.SubPath;
#endif
			}
			set
			{
#if DEVELOPMENT_BUILD_LINUX || UNITY_STANDALONE_LINUX
				linuxOverrideDirectPath = value;
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
				OSXOverrideDirectPath = value;
#else
				base.SubPath = value;
#endif
			}
		}
	}

	#endregion

}
