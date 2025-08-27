using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace CatAndHuman.Editor.Scripts
{
    public static class AddressableUtils
    {
        public static void EnsureSetup(string groupName, string label)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                settings = AddressableAssetSettings.Create(SettingsPath(), "AddressableAssetSettings", true, true);
                AddressableAssetSettingsDefaultObject.Settings = settings;
            }


            var group = settings.FindGroup(groupName) ?? settings.CreateGroup(groupName, false, false, true, null,
                typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
            
            if (!settings.GetLabels().Contains(label)) settings.AddLabel(label);


            var schema = group.GetSchema<BundledAssetGroupSchema>();
            if (schema != null)
            {
                schema.BundleMode = BundledAssetGroupSchema.BundlePackingMode.PackTogether;
            }


            AssetDatabase.SaveAssets();
        }
        
        public static void AddOrMoveToGroup(string assetPath, string groupName, string label)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.FindGroup(groupName);
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            var entry = settings.FindAssetEntry(guid);
            if (entry == null) entry = settings.CreateOrMoveEntry(guid, group);
            else settings.MoveEntry(entry, group);
            if (!entry.labels.Contains(label)) entry.SetLabel(label, true, true);
        }
        
        private static string SettingsPath()
        {
            const string root = "Assets/AddressableAssetsData";
            if (!AssetDatabase.IsValidFolder(root)) AssetDatabase.CreateFolder("Assets", "AddressableAssetsData");
            return root;
        }
    }
}