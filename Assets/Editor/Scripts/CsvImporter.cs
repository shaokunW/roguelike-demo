using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CatAndHuman.Editor.Scripts
{
    public static class CsvImporter
    {
        private const string RAW_DIR = "Assets/Blueprints/Raw";
        private const string OUT_DIR = "Assets/Blueprints/Bundles";
        private const string ADDRESSABLE_LABEL = "config";
        private const string ADDRESSABLE_GROUP = "Config";

        [MenuItem("Tools/Config/Import All (CSV -> SO -> Addressables")]
        public static void ImportAll()
        {
            try
            {
                var registry = LoadRegistry();
                EnsureFolders();
                AddressableUtils.EnsureSetup(ADDRESSABLE_GROUP, ADDRESSABLE_LABEL);
                foreach (var entry in registry.entries)
                {
                    ImportByRegistry(entry);
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static void ImportByRegistry(TableEntry entry)
        {
            Debug.Log($"Start loading {entry.assetName}");
            var csvPath = Path.Combine(RAW_DIR, entry.csvName + ".csv");
            if (!File.Exists(csvPath))
            {
                Debug.LogWarning($"[Config] CSV not found: {csvPath}");
                return;
            }

            var soType = entry.script.GetClass();   

            var rowsField = soType.GetField("rows",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var rowListType = rowsField.FieldType;
            var rowType = rowListType.IsGenericType ? rowListType.GetGenericArguments()[0] : null;
            if (rowType == null) throw new Exception($"rows must be List<T> in {soType.Name}");
            var rows = ReadCsv(csvPath, rowType);
            var outPath = Path.Combine(OUT_DIR, entry.assetName + ".asset");
            var asset = AssetDatabase.LoadAssetAtPath(outPath, soType);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance(soType) as ScriptableObject;
                AssetDatabase.CreateAsset(asset, outPath);
            }
            rowsField.SetValue(asset, rows);
            EditorUtility.SetDirty(asset);
            AddressableUtils.AddOrMoveToGroup(outPath, ADDRESSABLE_GROUP, ADDRESSABLE_LABEL);
        }
        private static object ReadCsv(string path, Type rowType)
        {
            var keyVals = CsvReader.Read(path);
            var listType = typeof(List<>).MakeGenericType(rowType);
            var resultList = (IList) Activator.CreateInstance(listType);
            foreach (var keyVal in keyVals)
            {
                resultList.Add(Deserializer.Deserialize(rowType, keyVal)); 
            }
            Debug.Log($"loading {path} size {resultList.Count}");
            return resultList;
        }
        
        private static void EnsureFolders()
        {
            if (!AssetDatabase.IsValidFolder("Assets/Blueprints")) AssetDatabase.CreateFolder("Assets", "Blueprints");
            if (!AssetDatabase.IsValidFolder(RAW_DIR)) AssetDatabase.CreateFolder("Assets/Blueprints", "Raw");
            if (!AssetDatabase.IsValidFolder(OUT_DIR)) AssetDatabase.CreateFolder("Assets/Blueprints", "Bundles");
        }

        private static ConfigRegistry LoadRegistry()
        {
            var guids = AssetDatabase.FindAssets("t:ConfigRegistry");
            if (guids.Length == 0) throw new Exception("No ConfigRegistry found");
            if (guids.Length > 1) throw new Exception("Multiple ConfigRegistry found");
            var path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<ConfigRegistry>(path);
        }

    }
}