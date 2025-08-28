using System;
using CatAndHuman.Configs.Runtime;
using UnityEditor;
using UnityEngine;

namespace CatAndHuman.Editor.Scripts
{
    [Serializable]
    public class TableEntry
    {
        [Tooltip("CSV 源文件名（不含扩展名），位于 Assets/GameConfigs/Raw/")]
        public string csvName;

        [Tooltip("生成的 Addressable 资产名（不含扩展名）")]
        public string assetName;

        [Tooltip("目标表 ScriptableObject 类型（如 ItemTable、MonsterTable）")]
        public MonoScript script;
    }

    [CreateAssetMenu(menuName = "Config/Config Registry", fileName = "ConfigRegistry")]
    public class ConfigRegistry : ScriptableObject
    {
        public TableEntry[] entries;
    }
}