using System;
using UnityEngine;

namespace ScriptableJsons
{
	public abstract class ScriptableJsonFromTextAsset<T> : ScriptableJsonGeneric<T> where T : class, new()
	{
		[SerializeReference]
		private TextAsset _textAsset = null;

		protected override void LoadData()
		{
			if (_textAsset)
			{
				JsonUtility.FromJsonOverwrite(_textAsset.text, loadedData);
			}
			else
			{
				Debug.LogError($"textAsset is null", this);
			}
		}
	}
}
