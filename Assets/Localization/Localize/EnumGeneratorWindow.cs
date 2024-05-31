#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class EnumGeneratorWindow : EditorWindow
{
    private string enumName = "MyDynamicEnum";
    private string enumElementsString = "Element1,Element2,Element3";

    [MenuItem("Tools/Enum Generator")]
    public static void ShowWindow()
    {
        GetWindow<EnumGeneratorWindow>("Enum Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Enum Generator", EditorStyles.boldLabel);

        enumName = EditorGUILayout.TextField("Enum Name:", enumName);
        enumElementsString = EditorGUILayout.TextField("Enum Elements (comma-separated):", enumElementsString);

        if (GUILayout.Button("Generate Enum"))
        {
            GenerateEnum();
        }
    }

    private void GenerateEnum()
    {
        string[] enumElements = enumElementsString.Split(',');

        string script = $@"
using UnityEngine;

public enum {enumName}
{{
{GenerateEnumElements(enumElements)}
}}
";

        string path = EditorUtility.SaveFilePanel("Save Enum Script", "Assets", $"{enumName}Enum.cs", "cs");
        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllText(path, script);
            AssetDatabase.Refresh();
            Debug.Log($"Enum script generated at: {path}");
        }
    }

    private string GenerateEnumElements(string[] elements)
    {
        string elementsString = "";
        foreach (var element in elements)
        {
            elementsString += $"    {element},\n";
        }
        return elementsString;
    }
}

#endif
