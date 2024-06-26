﻿using EasyPath;
using UnityEngine;

namespace ScriptableJson
{

	public interface IHavePath
	{
		public string PathToString();

		public bool LoadData();

		public bool SaveData(bool prettyPrint = true);

		public void SetDataToDefault();
	}

	public abstract class ScriptableJsonFromPath<T, Path> : ScriptableJsonGeneric<T>, IHavePath where Path : IPath
	{

		#region Fields

		[field: SerializeField, Space]
		public virtual Path PathData { get; set; }

		[Space]
		public DebugLevel throwDebugLogIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		/// <summary>
		/// Load the json data from the path define in pathData
		/// </summary>
		public override bool LoadData()
		{
			string path = PathData.GetFullPath();

			if (PathData.PathSystem == PathSystem.None)
			{
				return true;
			}

			if (TextFile.TryLoadText(PathData.PathSystem, path, out string json))
			{
				if (IsValueType)
				{
					Data = JsonUtility.FromJson<T>(json);
				}
				else
				{
					JsonUtility.FromJsonOverwrite(json, Data);
				}

				return true;
			}

			if (throwDebugLogIfNotFind != DebugLevel.None)
			{
				string getDebugText() => $"File: {PathData.FileName} not found at path: {path}";

				switch (throwDebugLogIfNotFind)
				{
					case DebugLevel.Normal:
						Debug.Log(getDebugText(), this);
						break;
					case DebugLevel.Warning:
						Debug.LogWarning(getDebugText(), this);
						break;
					case DebugLevel.Error:
						Debug.LogError(getDebugText(), this);
						break;
				}
			}

			return false;
		}

		#endregion

		#region SaveData

		/// <summary>
		/// Save the data to json file
		/// </summary>
		/// <param name="prettyPrint">If true, format the json for readability. If false, format the json for minimum size</param>
		public bool SaveData(bool prettyPrint = true)
		{
			if (PathData.PathSystem == PathSystem.None)
			{
				return true;
			}

			if (PathData.PathSystem == PathSystem.Resources)
			{
				Debug.LogError("Can't write in Resources's folder");
				return false;
			}

			string json = JsonUtility.ToJson(Data, prettyPrint);

			return TextFile.TrySaveText(PathData.PathSystem, PathData.GetFullPath(), json);
		}

		#endregion

		#region ToString

		public string PathToString()
		{
			return PathData.GetFullPath();
		}

		#endregion

	}

	public abstract class ScriptableJsonFromPath<T> : ScriptableJsonFromPath<T, PathData> { }

}
