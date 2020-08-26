#if UNITY_EDITOR
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

using System;
using UnityEngine;

namespace ScriptableJsons
{
	public abstract class ScriptableJsonGeneric<T> : ScriptableJson where T : class, new()
	{

		#region Variables

		[SerializeField]
		private bool _useFile = true;

		[SerializeField]
		protected T defaultData;

		[NonSerialized]
		protected T loadedData;

		#endregion

		#region Set

		public void SetData(T data)
		{
			loadedData = data;
		}

		public void SetDataToDefault()
		{
#if UNITY_EDITOR
			loadedData = DeepCopy(defaultData);
#else
			loadedData = defaultData;
#endif
		}

		#endregion

		#region Get

		public override object GetDataGeneric()
		{
			return GetData();
		}

		public T GetData()
		{
			if (loadedData == null)
			{
				SetDataToDefault();

				if (_useFile)
				{
					LoadData();
				}
			}

			return loadedData;
		}

		#endregion

		#region Load

		protected abstract void LoadData();

		#endregion

		#region Debug

#if UNITY_EDITOR
		protected static U DeepCopy<U>(U other)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, other);
				ms.Position = 0;

				return (U)formatter.Deserialize(ms);
			}
		}
#endif

		#endregion

	}
}
