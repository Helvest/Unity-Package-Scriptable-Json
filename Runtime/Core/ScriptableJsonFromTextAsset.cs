using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromTextAsset<T> : ScriptableJsonGeneric<T>
	{

		#region Fields

		[Space]
		[SerializeField]
		private TextAsset _textAsset = default;

		[Space]
		public DebugLevel throwDebugLogIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		/// <summary>
		/// Load the json data from the text file
		/// </summary>
		public override void LoadData()
		{
			if (_textAsset != null)
			{
				JsonUtility.FromJsonOverwrite(_textAsset.text, Data);
			}
			else
			{
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
			}
		}

		#endregion

	}
}