using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableJson
{
	public abstract class ScriptableJsonGeneric<T> : ScriptableObject
	{

		#region Fields

		[FormerlySerializedAs("useFile")]
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
				if (data == null || _needReset)
#else
				if (data == null)
#endif
				{
					Initialise();
				}

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
		private bool _needReset = false;
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
			if (typeof(T).IsValueType)
			{
				Data = defaultData;
			}
			else
			{
				Data = DeepCopy(defaultData);
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

		#region OnPlayModeStateChanged

#if UNITY_EDITOR
		protected virtual void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.ExitingPlayMode)
			{
				_needReset = true;
			}
		}
#endif

		#endregion

	}
}