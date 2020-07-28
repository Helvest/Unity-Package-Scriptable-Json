using System;
using UnityEngine;

namespace ScriptableJsons
{
	public abstract class ScriptableJsonFromTextAsset<T> : ScriptableJsonGeneric<T> where T : class, new()
	{
		public TextAsset _textAsset;

		[SerializeField]
		protected T defaultData;

		[NonSerialized]
		protected T loadedData;

		public override T GetData()
		{
			if (loadedData == null)
			{
#if UNITY_EDITOR
				loadedData = DeepCopy(defaultData);
#else
				loadedData = defaultData;
#endif

				if (_textAsset)
				{
					JsonUtility.FromJsonOverwrite(_textAsset.text, loadedData);
				}
			}

			return loadedData;
		}
	}
}
