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

		[Space]
		[SerializeField]
		protected T defaultData = default;

		[NonSerialized]
		protected T data = default;

		/// <summary>
		/// Cached data
		/// </summary>
		public T Data
		{
			get
			{
				if (data == null)
				{
					Initialise();
				}

				return data;
			}

			set => data = value;
		}

		#endregion

		#region Initialise

		/// <summary>
		/// Set data to default and than try to load data from file
		/// </summary>
		public void Initialise()
		{
			SetDataToDefault();

#if UNITY_EDITOR
			if (useFileInEditor)
			{
				LoadData();
			}
#else
			if (useFile)
			{
				LoadData();
			}
#endif
		}

		#endregion

		#region Set

		/// <summary>
		/// Copy default data in data
		/// </summary>
		public void SetDataToDefault()
		{
			if (typeof(T).IsValueType)
			{
				data = defaultData;
			}
			else
			{
				data = DeepCopy(defaultData);
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

		#region ToString

		public override string ToString()
		{
			return DataToString();
		}

		public string DataToString(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(Data, prettyPrint);
		}

		public string DefaultDataToString(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(defaultData, prettyPrint);
		}

		#endregion

		#region GetHashCode

		public override int GetHashCode()
		{
			return Data.GetHashCode();
		}

		#endregion

	}
}