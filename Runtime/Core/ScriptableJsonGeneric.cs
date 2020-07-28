#if UNITY_EDITOR
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace ScriptableJsons
{
	public abstract class ScriptableJsonGeneric<T> : ScriptableJson where T : class, new()
	{
		public override object GetDataGeneric()
		{
			return GetData();
		}

		public abstract T GetData();

#if UNITY_EDITOR
		protected static U DeepCopy<U>(U other)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(ms, other);
				ms.Position = 0;
				return (U)formatter.Deserialize(ms);
			}
		}
#endif
	}
}
