using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableJson
{
	[CreateAssetMenu(menuName = "ScriptablePath/With Build Target")]
	public class ScriptablePathBuildTarget : ScriptablePath
	{
		[Serializable]
		public class PathDataBuild : PathData
		{

			public List<RuntimePlatform> buildTargets = new List<RuntimePlatform>() { RuntimePlatform.WindowsPlayer };

			public ScriptablePathBuildTarget parent;

			private PathData Parent => parent?.GetPathData();

			public override PathSystem PathSystem
			{
				get
				{
					if (base.PathSystem == PathSystem.None && Parent != null)
					{
						return Parent.PathSystem;
					}

					return base.PathSystem;
				}
			}

			public override string CustomPathSystem
			{
				get
				{
					if (string.IsNullOrWhiteSpace(base.CustomPathSystem) && Parent != null)
					{
						return Parent.CustomPathSystem;
					}

					return base.CustomPathSystem;
				}
			}

			public override string SubPath
			{
				get
				{
					if (string.IsNullOrWhiteSpace(base.SubPath) && Parent != null)
					{
						return Parent.SubPath;
					}

					return base.SubPath;
				}
			}

			public override string FileName
			{
				get
				{
					if (string.IsNullOrWhiteSpace(base.FileName) && Parent != null)
					{
						return Parent.FileName;
					}

					return base.FileName;
				}
			}

			public override string Extension
			{
				get
				{
					if (string.IsNullOrWhiteSpace(base.Extension) && Parent != null)
					{
						return Parent.Extension;
					}

					return base.Extension;
				}
			}

		}

		public List<PathDataBuild> pathDataBuilds = new List<PathDataBuild>()
		{
			new PathDataBuild()
		};

		public override PathSystem PathSystem => GetPathData().PathSystem;

		public override string FileName => GetPathData().FileName;

		public override string GetFullPath()
		{
			return GetPathData().GetFullPath();
		}

		public override PathData GetPathData()
		{
			foreach (var item in pathDataBuilds)
			{
				if (item.buildTargets.Contains(Application.platform))
				{
					return item;
				}
			}

			return null;
		}
	}
}
