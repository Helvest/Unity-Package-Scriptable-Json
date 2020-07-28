using UnityEngine;

namespace ScriptableJsons
{
	public abstract class ScriptableJson : ScriptableObject
	{
		public abstract object GetDataGeneric();

		public T GetData<T>()
		{
			return (T)GetDataGeneric();
		}
	}
}
