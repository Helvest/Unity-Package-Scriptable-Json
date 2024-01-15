using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
#if UNITY_EDITOR
using UnityEditor;
#else
using System;
#endif

namespace ScriptableJson
{

	public abstract class ScriptableJsonGeneric<T> : ScriptableObject
	{

		#region Fields

		protected static bool IsValueType => typeof(T).IsValueType;

		public bool useFileInBuild = true;

		public bool useFileInDebug = true;

		public bool useFileInEditor = false;	

		protected bool UseFile
		{
			get
			{
				if (Application.isEditor)
				{
					return useFileInEditor;
				}

				if (Debug.isDebugBuild)
				{
					return useFileInDebug;
				}

				return useFileInBuild;
			}
		}

		[Space]
		[SerializeField]
		protected T defaultData = default;

#if UNITY_EDITOR
		[SerializeField]
#else
		[NonSerialized]
#endif
		protected T data = default;

		/// <summary>
		/// Cached data
		/// </summary>
		public T Data
		{
			get
			{
#if UNITY_EDITOR
				if (_needReset || data == null)
				{
					Initialise();
				}
#else
				if (data == null)
				{
					Initialise();
				}
#endif
				return data;
			}

			set
			{
#if UNITY_EDITOR
				_needReset = false;
#endif
				data = value;
			}
		}

#if UNITY_EDITOR
		private bool _isSubscribed = false;
		private bool _needReset = true;
#endif

#endregion

		#region Init

		/// <summary>
		/// Set data to default and than try to load data from file
		/// </summary>
		public void Initialise()
		{
			SetDataToDefault();

			if (UseFile)
			{
				LoadData();
			}
		}

		protected virtual void OnEnable()
		{
#if UNITY_EDITOR
			if (!_isSubscribed)
			{
				EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
				_isSubscribed = true;
			}
#endif
		}

		protected virtual void OnDisable()
		{
#if UNITY_EDITOR
			if (_isSubscribed)
			{
				EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
				_isSubscribed = false;
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
			Data = IsValueType ? defaultData : DeepCopy(defaultData);
		}

		#endregion

		#region Load

		public abstract bool LoadData();

		#endregion

		#region DeepCopy

		private static readonly BinaryFormatter _formatter = new BinaryFormatter();

		protected static T DeepCopy(T other)
		{
			using (var ms = new MemoryStream())
			{
				_formatter.Serialize(ms, other);
				ms.Position = 0;
				return (T)_formatter.Deserialize(ms);
			}
		}

		#endregion

		#region ToString

		public override string ToString()
		{
			return DataToJson();
		}

		public string DataToJson(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(Data, prettyPrint);
		}

		public string DefaultDataToJson(bool prettyPrint = true)
		{
			return JsonUtility.ToJson(defaultData, prettyPrint);
		}

		#endregion

		#region OnPlayModeStateChanged

#if UNITY_EDITOR
		protected virtual void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredPlayMode)
			{
				if (_needReset)
				{
					data = typeof(T).IsValueType ? defaultData : DeepCopy(defaultData);
				}
			}
			else if (state == PlayModeStateChange.ExitingPlayMode)
			{
				_needReset = true;
			}
		}
#endif

		#endregion

	}

}
