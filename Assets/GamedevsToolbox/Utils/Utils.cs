using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace GamedevsToolbox.Utils
{
	public class Utils {
		public static int GetTimestamp() {
			System.DateTime epochStart = new System.DateTime (1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			int currentEpochTime = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
			return currentEpochTime;
		}

		public static long GetTimestampInMillis() {
			System.DateTime epochStart = new System.DateTime (1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
			long currentEpochTime = (long)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;
            return currentEpochTime;
		}

		public static string Path(string filename) {
#if UNITY_EDITOR
            return Application.dataPath + "/../" + filename;
			//return System.IO.Path.Combine(Application.dataPath, "/../", filename);
#elif UNITY_STANDALONE
			return Application.dataPath + "/../" + filename;
            //return System.IO.Path.Combine(Application.dataPath, "/../", filename);
#endif
		}

		public static bool Exists(string filename) {
			string path = Path (filename);
			return System.IO.File.Exists (path);
		}
			
		public static void SaveFile(string filename, string data) {
			string path = Utils.Path (filename);
			try {
				System.IO.File.WriteAllText(path, data);
			} catch (System.Exception e) {
				Logger.Logger.LogError ("Error writting file @" + path);
                Logger.Logger.LogError ("Error: " + e.Message);
                Logger.Logger.LogError ("Stack trace: " + e.StackTrace);
			}
		}

		public static void SaveFile(string filename, byte[] data) {
			string path = Utils.Path (filename);
			try {
				System.IO.File.WriteAllBytes(path, data);
			} catch (System.Exception e) {
                Logger.Logger.LogError ("Error writting file @" + path);
                Logger.Logger.LogError ("Error: " + e.Message);
                Logger.Logger.LogError ("Stack trace: " + e.StackTrace);
			}
		}

		public static void AppendText(string filename, string text) {
			string path = Utils.Path (filename);
			try {
				System.IO.File.AppendAllText(path, text + "\n");
			} catch (System.Exception e) {
                Logger.Logger.LogError ("Error writting file @" + path);
                Logger.Logger.LogError ("Error: " + e.Message);
                Logger.Logger.LogError ("Stack trace: " + e.StackTrace);
			}
		}

		public static string LoadFile(string filename, string defaultValue = "") {
			string path = Utils.Path (filename);
			bool found = System.IO.File.Exists(path);
			if (found) {
				try {
					string data = System.IO.File.ReadAllText(path);
					return data;
				} catch (System.Exception e) {
                    Logger.Logger.LogError ("Error loading file @" + path);
                    Logger.Logger.LogError ("Error: " + e.Message);
                    Logger.Logger.LogError ("Stack trace: " + e.StackTrace);
					return defaultValue;
				}
			} else {
                Logger.Logger.LogWarning ("File @" + path + " not found.");
				return defaultValue;
			}
		}

		public static void DeleteFile(string filename) {
			string path = Utils.Path (filename);
			bool found = System.IO.File.Exists(path);
			if (found) {
				try {
					System.IO.File.Delete(path);
				} catch (System.Exception e) {
                    Logger.Logger.LogError ("Error deleting file @" + path);
                    Logger.Logger.LogError ("Error: " + e.Message);
                    Logger.Logger.LogError ("Stack trace: " + e.StackTrace);
				}
			} else {
                Logger.Logger.LogWarning ("File @" + path + " not found.");
			}
		}

		public static string GetUniqueID() {
			#if UNITY_EDITOR
			return SystemInfo.deviceUniqueIdentifier + " --- " + SystemInfo.deviceName;
			#else
			return SystemInfo.deviceUniqueIdentifier;
			#endif
		}
        
		public static string GetHash(string data) {
			System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed ();
			System.Text.StringBuilder hash = new System.Text.StringBuilder ();
			byte[] crypto = crypt.ComputeHash (System.Text.Encoding.UTF8.GetBytes (data), 0, System.Text.Encoding.UTF8.GetByteCount (data));
			foreach (byte theByte in crypto) {
				hash.Append (theByte.ToString ("x2"));
			}
			return hash.ToString ();
		}

        public static void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public static string GetVersion()
        {
            return Application.version;
        }

        public static int VersionComparison(string versionLocal, string targetVersion)
        {
            string[] partsLocal = versionLocal.Split('.');
            string[] partsTarget = targetVersion.Split('.');

            int i = 0;
            int comparison = 0;
            while (i < partsLocal.Length && i < partsTarget.Length && comparison == 0)
            {
                if (partsLocal[i].IndexOf('d') != -1)
                {
                    partsLocal[i] = partsLocal[i].Substring(0, partsLocal[i].Length - 1);
                }
                if (partsTarget[i].IndexOf('d') != -1)
                {
                    partsTarget[i] = partsTarget[i].Substring(0, partsTarget[i].Length - 1);
                }
                int num1 = int.Parse(partsLocal[i]);
                int num2 = int.Parse(partsTarget[i]);
                if (num1 == num2)
                {
                    comparison = 0;
                } else if (num1 > num2)
                {
                    comparison = 1;
                } else
                {
                    comparison = -1;
                }
                i++;
            }
            if (comparison == 0 && partsTarget.Length > partsLocal.Length)
            {
                comparison = -1;
            }
            return comparison;
        }

        public static void OpenUrl(string url)
        {
            Application.OpenURL(url);
        }
    }
}