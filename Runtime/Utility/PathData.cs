using System;
using System.IO;
using UnityEngine;

namespace ScriptableJson
{

	#region Enum

	/// <summary>
	/// Enum representing different path systems.
	/// </summary>
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
	public class PathData : IPath
	{

		#region Fields

		/// <summary>
		/// The path system to use
		/// </summary>
		[field: SerializeField, Tooltip("The path system to use.")]
		public virtual PathSystem PathSystem { get; set; } = PathSystem.StreamingAssets;

		/// <summary>
		/// Custom path system if PathSystem is set to CustomPathSystem
		/// </summary>
		[field: SerializeField, Tooltip("Custom path system if PathSystem is set to CustomPathSystem.")]
		public virtual string CustomPathSystem { get; set; } = string.Empty;

		/// <summary>
		/// Subpath within the selected path system
		/// </summary>
		[field: SerializeField, Tooltip("Subpath within the selected path system.")]
		public virtual string SubPath { get; set; } = string.Empty;

		/// <summary>
		/// File name without extension
		/// </summary>
		[field: SerializeField, Tooltip("File name without extension.")]
		public virtual string FileName { get; set; } = string.Empty;

		/// <summary>
		/// File extension, dot not required
		/// </summary>
		[field: SerializeField, Tooltip("File extension, dot not required.")]
		public virtual string Extension { get; set; } = string.Empty;

		public virtual string FileNameWithExtension
		{
			get
			{
				return string.IsNullOrWhiteSpace(Extension) ? 
					FileName : Path.ChangeExtension(FileName, Extension);
			}
		}

		#endregion

		#region Init

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PathData() { }

		/// <summary>
		/// Constructor to initialize from a full path.
		/// </summary>
		/// <param name="fullPath">Full path to initialize from.</param>
		public PathData(string fullPath)
		{
			SetFromFullPath(fullPath);
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="pathData">PathData instance to copy.</param>
		public PathData(PathData pathData)
		{
			Copy(pathData);
		}

		#endregion

		#region PathToPathSystem

		/// <summary>
		/// Array of path systems for path conversion checks.
		/// </summary>
		public static readonly PathSystem[] pathSystemArray = new PathSystem[]
		{
			PathSystem.StreamingAssets,
			PathSystem.GameData,
			PathSystem.PersistentData,
			PathSystem.TemporaryCache,
			PathSystem.ConsoleLog,
			PathSystem.AbsoluteURL,
			PathSystem.CustomPathSystem
		};

		/// <summary>
		/// Converts a full path to a PathSystem based on predefined path systems.
		/// </summary>
		/// <param name="path">Full path to convert.</param>
		/// <returns>Converted PathSystem.</returns>
		public PathSystem PathToPathSystem(string path)
		{
			foreach (var system in pathSystemArray)
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

		/// <summary>
		/// Gets the system path based on the current PathSystem.
		/// </summary>
		/// <returns>System path.</returns>
		public virtual string GetSytemPath()
		{
			return GetSytemPath(PathSystem);
		}

		/// <summary>
		/// Gets the system path based on the provided PathSystem.
		/// </summary>
		/// <param name="system">PathSystem to get the path for.</param>
		/// <returns>System path.</returns>
		public virtual string GetSytemPath(PathSystem system)
		{
			return system == PathSystem.CustomPathSystem ? CustomPathSystem : GetSytemPathDefault(system);
		}

		/// <summary>
		/// Gets the default system path for a given PathSystem.
		/// </summary>
		/// <param name="system">PathSystem to get the default path for.</param>
		/// <returns>Default system path.</returns>
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

		/// <summary>
		/// Gets the full path based on the current PathSystem.
		/// </summary>
		/// <returns>Full path.</returns>
		public virtual string GetFullPath()
		{
			return PathSystem switch
			{
				PathSystem.None => Path.Combine(SubPath, FileNameWithExtension),
				PathSystem.Resources => Path.Combine(SubPath, FileName),
				_ => Path.Combine(GetSytemPath(), SubPath, FileNameWithExtension)
			};
		}

		/// <summary>
		/// Gets the directory path based on the current PathSystem.
		/// </summary>
		/// <returns>Directory path.</returns>
		public virtual string GetDirectoryPath()
		{
			return PathSystem switch
			{
				PathSystem.None => SubPath,
				PathSystem.Resources => SubPath,
				_ => Path.Combine(GetSytemPath(), SubPath)
			};
		}

		/// <summary>
		/// Gets the partial path based on the current PathSystem.
		/// </summary>
		/// <returns>Partial path.</returns>
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

		/// <summary>
		/// Sets the fields based on a full path.
		/// </summary>
		/// <param name="fullPath">Full path to set from.</param>
		public virtual void SetFromFullPath(params string[] fullPath)
		{
			SetFromFullPath(Path.Combine(fullPath));
		}

		/// <summary>
		/// Sets the fields based on a full path.
		/// </summary>
		/// <param name="fullPath">Full path to set from.</param>
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

		/// <summary>
		/// Sets the fields based on a partial path.
		/// </summary>
		/// <param name="partialPath">Partial path to set from.</param>
		public virtual void SetFromPartialPath(string partialPath)
		{
			SubPath = Path.GetDirectoryName(partialPath);
			FileName = Path.GetFileNameWithoutExtension(partialPath);
			Extension = Path.HasExtension(partialPath) ? Path.GetExtension(partialPath) : string.Empty;
		}

		#endregion

		#region Copy

		/// <summary>
		/// Copies the values from another PathData instance.
		/// </summary>
		/// <param name="pathData">PathData instance to copy from.</param>
		public virtual void Copy(PathData pathData)
		{
			PathSystem = pathData.PathSystem;
			CustomPathSystem = pathData.CustomPathSystem;
			SubPath = pathData.SubPath;
			FileName = pathData.FileName;
			Extension = pathData.Extension;
		}

		#endregion

		#region Exist

		/// <summary>
		/// Checks if the directory exists.
		/// </summary>
		/// <returns>True if the directory exists, false otherwise.</returns>
		public bool DirectoryExist()
		{
			return Directory.Exists(GetDirectoryPath());
		}

		/// <summary>
		/// Checks if the file exists.
		/// </summary>
		/// <returns>True if the file exists, false otherwise.</returns>
		public bool FileExist()
		{
			return File.Exists(GetFullPath());
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
