using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CatAndHuman.Configs.Runtime
{
    public class DesrUtils
    {
        public static List<string> ParseList(string s)
        {
            return string.IsNullOrEmpty(s)
                ? new List<string>()
                : s.Split(';').ToList();
        }

        public static int ParseInt(Dictionary<string, string> data, string k)
        {
            try
            {
                return int.Parse(data[k]);
            }
            catch (Exception e)
            {
                Debug.LogError($"Deserialization failed for {k}: {e.Message}");
                throw e;
            }
        }
        
        public static float ParseFloat(Dictionary<string, string> data, string k)
        {
            try
            {
                return float.Parse(data[k]);
            }
            catch (Exception e)
            {
                Debug.LogError($"Deserialization failed for {k}: {e.Message}");
                throw e;
            }
        }
        
        public static List<string> ParseList(Dictionary<string, string> data, string k)
        {
            try
            {
                var list = ParseList(data[k]);
                return list;
            }
            catch (Exception e)
            {
                Debug.LogError($"Deserialization failed for {k}: {e.Message}");
                throw e;
            }
        }
    }
}