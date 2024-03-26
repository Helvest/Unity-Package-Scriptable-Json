using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableJson
{
	[CreateAssetMenu(menuName = "ScriptableJson/Scriptable Path Build Target")]
	public class ScriptablePathBuildTarget : ScriptablePath
	{
		[Serializable]
		public class PathDataWithBuildTarget : PathDataWithParent
		{
			public List<RuntimePlatform> buildTargets = new List<RuntimePlatform>() { RuntimePlatform.WindowsPlayer };
		}

		public List<PathDataWithBuildTarget> pathDataBuilds = new List<PathDataWithBuildTarget>()
		{
			new PathDataWithBuildTarget()
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
