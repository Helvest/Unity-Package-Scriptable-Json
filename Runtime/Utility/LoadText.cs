using System.IO;
using UnityEngine;

namespace ScriptableJson
{
	public static class LoadText
	{
		public enum PathSystem
		{
			DirectPath,
			StreamingAssets,
			Resources
		}

		public static bool TryLoadText(PathSystem pathSystem, string path, string extension, out string text)
		{
			switch (pathSystem)
			{
				default:
				case PathSystem.DirectPath:
					return TryLoadTextFromPatch(path + extension, out text);

				case PathSystem.StreamingAssets:
					return TryLoadTextFromStreamingAssets(path + extension, out text);

				case PathSystem.Resources:
					return TryLoadTextFromRessource(path, out text);
			}
		}


		public static bool TryLoadTextFromPatch(string path, out string text)
		{
			if (File.Exists(path))
			{
				using (StreamReader streamReader = File.OpenText(path))
				{
					text = streamReader.ReadToEnd();

					return true;
				}
			}

			text = string.Empty;

			return false;
		}

		public static bool TryLoadTextFromStreamingAssets(string path, out string text)
		{
			return TryLoadTextFromPatch(Path.Combine(Application.streamingAssetsPath, path), out text);
		}

		public static bool TryLoadTextFromRessource(string path, out string text)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(path);

			if (textAsset != null)
			{
				text = textAsset.text;

				Resources.UnloadAsset(textAsset);

				return true;
			}

			Debug.LogWarning($"File not found at path: {path}");

			text = string.Empty;

			return false;
		}
	}
}
