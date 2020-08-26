using System.IO;
using UnityEngine;

namespace ScriptableJsons
{
	public abstract class ScriptableJsonFromPath<T> : ScriptableJsonGeneric<T> where T : class, new()
	{
		[SerializeField]
		private LoadText.PathSystem _pathSystem = LoadText.PathSystem.DirectPath;

		[SerializeField]
		private string _path = string.Empty;

		[SerializeField]
		private string _fileName = string.Empty;

		private const string _extension = ".json";

		[SerializeField]
		private bool _throwErrorIfNotFind = false;

		protected override void LoadData()
		{
			if (LoadText.TryLoadText(_pathSystem, Path.Combine(_path, _fileName), _extension, out string json))
			{
				JsonUtility.FromJsonOverwrite(json, loadedData);
			}
			else if (_throwErrorIfNotFind)
			{
				Debug.LogError($"File: {_fileName} not found at path: {_path}", this);
			}
		}
	}
}
