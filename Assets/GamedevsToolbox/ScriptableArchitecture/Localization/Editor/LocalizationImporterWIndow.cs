using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace GamedevsToolbox.ScriptableArchitecture.Localization
{
    public class LocalizationImporterWIndow : EditorWindow
    {
        private string filePath = default;
        private string outputPath = default;
        private string projectOutputPath = default;
        private int rowStart = 0;
        private int idColumn = 0;
        private int numberOfLanguages = 1;
        private int firstLanguageColumn = 2;

        private List<SystemLanguage> languagesToImport = new List<SystemLanguage>();

        [MenuItem("Gamedevs Toolbox/Import localization data")]
        public static void OpenWindow()
        {
            LocalizationImporterWIndow window = GetWindow<LocalizationImporterWIndow>("Localization importer");
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Open File"))
            {
                filePath = EditorUtility.OpenFilePanelWithFilters("Select CSV file", Application.dataPath, new string[] { "FileExtension","csv" });
            }
            if (string.IsNullOrEmpty(filePath))
            {
                GUILayout.Label("Select a CSV file");
            } else
            {
                GUILayout.Label(filePath);
            }

            GUILayout.Space(30f);

            if (GUILayout.Button("Select output path"))
            {
                outputPath = EditorUtility.OpenFolderPanel("Select output path", Application.dataPath, "Localization data");
            }
            if (string.IsNullOrEmpty(outputPath))
            {
                GUILayout.Label("Select an output path");
            }
            else
            {
                if (!outputPath.Contains(Application.dataPath))
                {
                    GUILayout.Label("Use a path inside the project");
                }
                else
                {
                    projectOutputPath = "Assets"+outputPath.Remove(outputPath.IndexOf(Application.dataPath), Application.dataPath.Length);
                    GUILayout.Label(projectOutputPath);
                }
            }

            GUILayout.Space(30f);
            
            GUILayout.BeginHorizontal();
            rowStart = EditorGUILayout.IntField("Starting row", rowStart);
            EditorGUILayout.LabelField("(The row at which the data WITHOUT HEADERS begin)");
            GUILayout.EndHorizontal();

            GUILayout.Space(30f);
            
            GUILayout.BeginHorizontal();
            idColumn = EditorGUILayout.IntField("Text id column", idColumn);
            EditorGUILayout.LabelField("(Set the index on the CSV file in which the text ID is stored)");
            GUILayout.EndHorizontal();

            GUILayout.Space(30f);

            GUILayout.BeginHorizontal();
            numberOfLanguages = EditorGUILayout.IntField("Number of languages", numberOfLanguages);
            if (languagesToImport.Count != numberOfLanguages)
            {
                if (languagesToImport.Count > numberOfLanguages)
                {
                    languagesToImport.RemoveRange(numberOfLanguages, languagesToImport.Count - numberOfLanguages);
                }
                else
                {
                    while (languagesToImport.Count != numberOfLanguages)
                    {
                        languagesToImport.Add(SystemLanguage.Afrikaans);
                    }
                }
            }
            EditorGUILayout.LabelField("(Set the quantity of languages you are going to import)");
            GUILayout.EndHorizontal();

            GUILayout.Space(30f);

            GUILayout.BeginHorizontal();
            firstLanguageColumn = EditorGUILayout.IntField("First language column", firstLanguageColumn);
            GUILayout.Label("(Set the index on the CSV file in which the first language is stored)");
            GUILayout.EndHorizontal();

            for (int i = 0; i < numberOfLanguages; ++i)
            {
                languagesToImport[i] = (SystemLanguage)EditorGUILayout.EnumPopup(languagesToImport[i]);
            }

            if (GUILayout.Button("Generate data"))
            {
                string fileData = System.IO.File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileData))
                {
                    string[] lines = fileData.Split('\n');
                    for (int i = rowStart; i < lines.Length; ++i)
                    {
                        ProcessLine(lines[i]);
                    }
                }
            }
        }

        private void ProcessLine(string line)
        {
            line = line.Replace(";", "");
            if (string.IsNullOrEmpty(line))
                return;
            string[] parts = line.Split(',');
            string id = parts[idColumn];
            string[] texts = new string[numberOfLanguages];
            for(int i = firstLanguageColumn; i < firstLanguageColumn+numberOfLanguages; ++i)
            {
                texts[i - firstLanguageColumn] = parts[i];
            }

            ScriptableLocalizedText slt = GetLocalizedText(id);
            for (int i = 0; i < numberOfLanguages; ++i)
            {
                ScriptableLocalizedText.LocalizedTextPair ltp = new ScriptableLocalizedText.LocalizedTextPair();
                ltp.language = languagesToImport[i];
                ltp.text = texts[i];
                slt.AddText(ltp);
            }
        }

        private ScriptableLocalizedText GetLocalizedText(string id)
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(ScriptableLocalizedText).Name);
            ScriptableLocalizedText[] a = new ScriptableLocalizedText[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<ScriptableLocalizedText>(path);
                if (a[i].GetTextId() == id)
                {
                    return a[i];
                }
            }

            return CreateLocalizedText(id);
        }

        private ScriptableLocalizedText CreateLocalizedText(string id)
        {
            ScriptableLocalizedText asset = ScriptableObject.CreateInstance<ScriptableLocalizedText>();
            asset.SetId(id);
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(projectOutputPath + "/" + id + ".asset");
            AssetDatabase.CreateAsset(asset, assetPathAndName);
            return asset;
        }

        private void FinishCreatingAssets()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}