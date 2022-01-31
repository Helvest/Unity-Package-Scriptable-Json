using System;
using System.IO;
using UnityEngine;

namespace ScriptableJson
{
	public static class TextFile
	{
		public static bool TryLoadText(PathSystem pathSystem, string path, out string text)
		{
			return pathSystem switch
			{
				PathSystem.Resources => TryLoadTextFromRessource(path, out text),
				_ => TryLoadTextFromPath(path, out text),
			};
		}

		public static bool TrySaveText(PathSystem pathSystem, string path, string text)
		{
			return pathSystem switch
			{
				PathSystem.Resources => false,
				_ => TrySaveTextToPath(path, text),
			};
		}

		public static bool TryLoadTextFromPath(string path, out string text)
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

		public static bool TrySaveTextToPath(string path, string text)
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