using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromTextAsset<T> : ScriptableJsonGeneric<T>
	{

		#region Variables

		[SerializeField]
		private TextAsset _textAsset = default;

		[SerializeField]
		private DebugLevel _throwDebugIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		public override void LoadData()
		{
			if (_textAsset)
			{
				JsonUtility.FromJsonOverwrite(_textAsset.text, Data);
			}
			else
			{
				const string DEBUG_TEXT = "TextAsset is null";

				switch (_throwDebugIfNotFind)
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
