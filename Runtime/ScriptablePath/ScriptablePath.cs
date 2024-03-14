using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptablePath : ScriptableObject, IPath
	{
		public abstract PathSystem PathSystem { get; }

		public abstract string FileName { get; }

		public abstract string GetFullPath();

		public abstract PathData GetPathData();
	}
}
