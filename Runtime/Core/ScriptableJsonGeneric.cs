#if UNITY_EDITOR
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
#endif

using System;
using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonGeneric<T> : ScriptableObject where T : class, new()
	{

		#region Variables

		[SerializeField]
		private bool _useFile = true;

#if UNITY_EDITOR
		[SerializeField]
		private bool _useFileInEditor = true;
#endif

		[SerializeField]
		protected T defaultData;

		[NonSerialized]
		public T data;

		#endregion

		#region OnEnable

		private void OnEnable()
		{
			if (data == null)
			{
				SetDataToDefault();

#if UNITY_EDITOR
				if (_useFileInEditor)
				{
					LoadData();

					if (_useFile)
					{

					}
				}
#else
				if (_useFile)
				{
					LoadData();
				}
#endif
			}
		}

		#endregion

		#region Set

		public void SetDataToDefault()
		{
#if UNITY_EDITOR
			data = DeepCopy(defaultData);
#else
			data = defaultData;
#endif
		}

		#endregion

		#region Load

		public abstract void LoadData();

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
