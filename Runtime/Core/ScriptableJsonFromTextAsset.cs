using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromTextAsset<T> : ScriptableJsonGeneric<T>
	{

		#region Fields

		[Space]
		[SerializeField]
		private TextAsset _textAsset;

		[Space]
		public DebugLevel throwDebugLogIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		/// <summary>
		/// Load the json data from the text file
		/// </summary>
		public override bool LoadData()
		{
			if (_textAsset != null)
			{
				string json = _textAsset.text;

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

			const string DEBUG_TEXT = "TextAsset is null";

			switch (throwDebugLogIfNotFind)
			{
				case DebugLevel.Normal:
					Debug.Log(DEBUG_TEXT, this);
					break;
				case DebugLevel.Warning:
					Debug.LogWarning(DEBUG_TEXT, this);
					break;
				case DebugLevel.Error:
					Debug.LogError(DEBUG_TEXT, this);
					break;
			}

			return false;
		}

		#endregion

	}
}