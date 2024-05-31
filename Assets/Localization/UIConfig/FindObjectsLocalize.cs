using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

#if UNITY_EDITOR

public class FindObjectsLocalize : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty componentsProperty;

    [SerializeField, TableList(ShowIndexLabels = true)] List<Localize> components = new List<Localize>();

    [Serializable]
    public struct Localize
    {
        public UICustomText component;
        public LocalizeType type;
        public string text;
        public PresetType presetType;

    }

    private GameObject textmeshproUGUI;

    [MenuItem("Auto/Find LocalLize")]
    public static void ShowWindow()
    {
        GetWindow<FindObjectsLocalize>("Find Objects Localize");
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        componentsProperty = serializedObject.FindProperty("components");
    }

    private void OnGUI()
    {
        //TOOL:
        GUILayout.Space(10);

        GUILayout.Label("Find Objects Localize", EditorStyles.boldLabel);
        textmeshproUGUI = (GameObject)EditorGUILayout.ObjectField("textmeshproUGUI", textmeshproUGUI, typeof(GameObject), true);

        GUILayout.Space(10);

        if (GUILayout.Button("Find Objects"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                FindAllComponent(Selection.gameObjects, nameof(UICustomText), true);
            }
        }

        if (GUILayout.Button("Reload"))
        {
            for (int i = 0; i < components.Count; i++)
            {
                components[i].component.presetType = ConvertPresetType(components[i].component.TextPreset);
                components[i].component.Reload();
            }
        }

        if (GUILayout.Button("Change UICustomText"))
        {
            ChangeTextMeshPro();
        }

        GUILayout.Space(20);

        // show serializefied
        serializedObject.Update();

        EditorGUILayout.PropertyField(componentsProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    private PresetType ConvertPresetType(string type)
    {
        return type switch
        {
            "Title 1" => PresetType.Title_1,
            "Title 2" => PresetType.Title_2,
            "Title 3" => PresetType.Title_3,
            "Content 1" => PresetType.Content_1,
            "Content 2" => PresetType.Content_2,
            "Button 0" => PresetType.Button_0,
            "Button 1" => PresetType.Button_1,
            "Button 2" => PresetType.Button_2,
            "Quote 1" => PresetType.Quote_1,
            "Quote 2" => PresetType.Quote_2,
            "Quote 3" => PresetType.Quote_3,
            "Quote 4" => PresetType.Quote_4,
            "Normal" => PresetType.Normal,
            _ => throw new NotImplementedException(),
        };
    }

    public void FindAllComponent(GameObject[] goes, string componentName, bool isDeactive)
    {
        components.Clear();
        foreach (GameObject go in goes)
        {
            UICustomText[] localizes = go.GetComponentsInChildren<UICustomText>(true);

            foreach (UICustomText l in localizes)
            {
                Localize ll = new Localize()
                {
                    component = l,
                    text = l.TextField.text,
                    type = l.localizeType,
                    presetType = l.presetType,
                };
                components.Add(ll);
            }
        }
    }

    private void ChangeTextMeshPro()
    {
        TextMeshProGUIOv[] goes = Selection.gameObjects[0].GetComponentsInChildren<TextMeshProGUIOv>(true);

        foreach (var gos in goes)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(textmeshproUGUI, gos.transform.parent);
            CopyPasteComponent.PasteTransformValues<RectTransform>(gos.gameObject, go);
            CopyPasteComponent.PasteTransformValues<TextMeshProUGUI>(gos.gameObject, go);
            CopyPasteComponent.PasteTransformValues<UICustomText>(gos.gameObject, go);
            go.name = gos.name;
        }
    }
}
#endif
