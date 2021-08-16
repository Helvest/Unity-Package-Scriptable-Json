using System;
using System.IO;
using UnityEngine;

namespace ScriptableJson
{
	public static class TextFile
	{
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

		public static bool TrySaveText(PathSystem pathSystem, string path, string extension, string text)
		{
			switch (pathSystem)
			{
				default:
				case PathSystem.DirectPath:
					return TrySaveTextToPatch(path + extension, text);

				case PathSystem.StreamingAssets:
					return TrySaveTextToStreamingAssets(path + extension, text);

				case PathSystem.Resources:
					return false;
			}
		}

		public static bool TryLoadTextFromPatch(string path, out string text)
		{
			if (File.Exists(path))
			{
				using (var streamReader = File.OpenText(path))
				{
					text = streamReader.ReadToEnd();

					return true;
				}
			}

			text = string.Empty;

			return false;
		}

		public static bool TrySaveTextToPatch(string path, string text)
		{
			try
			{
				File.WriteAllText(path, text);
			}
			catch (Exception e)
			{
				Debug.LogError(e);
				return false;
			}

			return true;
		}

		public static bool TryLoadTextFromStreamingAssets(string path, out string text)
		{
			return TryLoadTextFromPatch(Path.Combine(Application.streamingAssetsPath, path), out text);
		}

		public static bool TrySaveTextToStreamingAssets(string path, string text)
		{
			return TrySaveTextToPatch(Path.Combine(Application.streamingAssetsPath, path), text);
		}

		public static bool TryLoadTextFromRessource(string path, out string text)
		{
			var textAsset = Resources.Load<TextAsset>(path);

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
