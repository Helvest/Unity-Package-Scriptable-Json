using System;
using UnityEngine;

namespace ScriptableJson
{
	[Serializable]
	public class PathDataWithParent : PathData
	{
		[Space]
		public ScriptablePathBuildTarget parent;

		public PathData ParentPath => parent?.GetPathData();

		public bool HasParent => parent != null;

		public override PathSystem PathSystem =>
			base.PathSystem == PathSystem.None && HasParent
			? ParentPath.PathSystem : base.PathSystem;

		public override string CustomPathSystem =>
			string.IsNullOrWhiteSpace(base.CustomPathSystem) && HasParent
			? ParentPath.CustomPathSystem : base.CustomPathSystem;

		public override string SubPath =>
			string.IsNullOrWhiteSpace(base.SubPath) && HasParent
			? ParentPath.SubPath : base.SubPath;

		public override string FileName =>
			string.IsNullOrWhiteSpace(base.FileName) && HasParent
			? ParentPath.FileName : base.FileName;

		public override string Extension =>
			string.IsNullOrWhiteSpace(base.Extension) && HasParent
			? ParentPath.Extension : base.Extension;

	}
}
