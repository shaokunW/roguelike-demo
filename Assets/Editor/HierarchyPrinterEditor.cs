using UnityEngine;
using UnityEditor;
using System.Text;

public class HierarchyPrinterEditor
{
    [MenuItem("Tools/Print Hierarchy as Tree")]
    static void PrintHierarchyTree()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var root in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            AppendHierarchy(sb, root.transform, 0);
        }

        Debug.Log(sb.ToString());
    }

    static void AppendHierarchy(StringBuilder sb, Transform t, int indent)
    {
        string prefix = new string(' ', indent * 2);
        string comps = "";

        foreach (var comp in t.GetComponents<Component>())
        {
            if (comp == null) continue;
            comps += comp.GetType().Name + ", ";
        }

        if (comps.EndsWith(", ")) comps = comps.Substring(0, comps.Length - 2);

        sb.AppendLine($"{prefix}- {t.name} [{comps}]");

        foreach (Transform child in t)
        {
            AppendHierarchy(sb, child, indent + 1);
        }
    }
}
