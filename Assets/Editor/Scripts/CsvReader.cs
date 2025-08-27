using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;

namespace CatAndHuman.Editor.Scripts
{
    public static class CsvReader
    {
        // Delimiter for splitting columns, accounts for commas inside quotes
        private const string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";

        public static List<Dictionary<string, string>> Read(string filePath)
        {
            var list = new List<Dictionary<string, string>>();
            TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);

            if (data == null)
            {
                Debug.LogError($"Cannot find file at '{filePath}'");
                return list;
            }

            // Split the text into lines, handling both Windows and Unix line endings
            var lines = data.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length <= 1)
            {
                Debug.LogError($"CSV file at '{filePath}.csv' is empty or has only a header.");
                return list;
            }

            // Use the first line as headers
            var headers = Regex.Split(lines[0], SPLIT_RE);
            for (int i = 0; i < headers.Length; i++)
            {
                // Clean up headers (remove quotes and whitespace)
                headers[i] = headers[i].Trim('"', ' ');
            }

            // Process data rows
            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length != headers.Length)
                {
                    Debug.LogWarning($"Row {i + 1} has a different number of columns than the header. Skipping.");
                    continue;
                }

                var entry = new Dictionary<string, string>();
                for (var j = 0; j < headers.Length; j++)
                {
                    // Clean up values
                    string value = values[j].Trim('"', ' ');
                    entry[headers[j]] = value;
                }
                list.Add(entry);
            }

            return list;
        }
    }

}