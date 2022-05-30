using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonGeneric<T> : ScriptableObject
	{

		#region Fields

		public bool useFile = true;

		public bool useFileInEditor = true;

		[SerializeField]
		protected T defaultData = default;

		[NonSerialized]
		protected T _data = default;

		public T Data
		{
			get
			{
				if (_data == null)
				{
					SetDataToDefault();
#if UNITY_EDITOR
					if (useFileInEditor)
					{
						LoadData();

						if (useFile)
						{ }
					}
#else
				if (useFile)
				{
					LoadData();
				}
#endif
				}

				return _data;
			}

			set => _data = value;
		}

		#endregion

		#region Set

		public void SetDataToDefault()
		{
			if (typeof(T).IsClass)
			{
				_data = DeepCopy(defaultData);
			}
			else
			{
				_data = defaultData;
			}
		}

		#endregion

		#region Load

		public abstract void LoadData();

		#endregion

		#region DeepCopy

		protected static T DeepCopy(T other)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();

				formatter.Serialize(ms, other);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}

		#endregion

	}
}