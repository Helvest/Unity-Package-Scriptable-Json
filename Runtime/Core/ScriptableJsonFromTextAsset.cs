using UnityEngine;

namespace ScriptableJson
{
	public abstract class ScriptableJsonFromTextAsset<T> : ScriptableJsonGeneric<T> where T : class, new()
	{

		#region Variables

		[SerializeReference]
		private TextAsset _textAsset = null;

		[SerializeField]
		private DebugLevel _throwDebugIfNotFind = DebugLevel.Warning;

		#endregion

		#region LoadData

		public override void LoadData()
		{
			if (_textAsset)
			{
				JsonUtility.FromJsonOverwrite(_textAsset.text, data);
			}
			else
			{
				string getDebugText() => $"TextAsset is null";

				switch (_throwDebugIfNotFind)
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
		}

		#endregion

	}
}
