using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR

public class FindObjectsWithComponent : EditorWindow
{
    private Dictionary<string, Type> componentType =  new Dictionary<string, Type>()
    {
        { nameof(Transform), typeof(Transform) },
        { nameof(Image), typeof(Image) },
        { nameof(Text), typeof(Text) },
        { nameof(TextMeshProGUIOv), typeof(TextMeshProGUIOv) },
        { nameof(TextMeshProUGUI), typeof(TextMeshProUGUI) },
        { nameof(Button), typeof(Button) },
    };

    private GameObject textmeshproUGUI;
    private GameObject textmeshproUGUIov;
    private GameObject button;

    private string[] stringOptions;
    private bool isDeactive = false;

    private SerializedObject serializedObject;
    private SerializedProperty componentsProperty;

    [SerializeField] List<Component> components = new List<Component>();
    private string componentName;

    [MenuItem("Auto/Find Objects with Component")]
    public static void ShowWindow()
    {
        GetWindow<FindObjectsWithComponent>("Find Objects with Component");
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        componentsProperty = serializedObject.FindProperty("components");

        //setup select dropdown
        stringOptions = new string[componentType.Count];
        int index = 0;
        foreach (var item in componentType)
        {
            stringOptions[index] = item.Key;
            index++;
        }
    }

    private void OnGUI()
    {
        //TODO: ke them tool
        textmeshproUGUI = (GameObject)EditorGUILayout.ObjectField("textmeshproUGUI", textmeshproUGUI, typeof(GameObject), true);
        textmeshproUGUIov = (GameObject)EditorGUILayout.ObjectField("textmeshproUGUIov", textmeshproUGUIov, typeof(GameObject), true);
        button = (GameObject)EditorGUILayout.ObjectField("Button", button, typeof(GameObject), true);

        //TOOL:
        GUILayout.Space(10);

        GUILayout.Label("Find Objects with Component", EditorStyles.boldLabel);
        GUILayout.Label("Select an object and search component type \nin childrens in that object", EditorStyles.label);

        GUILayout.Space(10);
        //select string
        int selectedIndex = EditorGUILayout.Popup(GetSelectedIndex(), stringOptions);
        if (selectedIndex >= 0 && selectedIndex < stringOptions.Length)
        {
            componentName = stringOptions[selectedIndex];
        }

        GUILayout.Space(10);

        isDeactive = GUILayout.Toggle(isDeactive, "With deactive object");

        GUILayout.Space(10);

        if (GUILayout.Button("Find Objects"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                CheckAllComponent(Selection.gameObjects, componentName, isDeactive);
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Add Textmeshpro Custom"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                FindAllComponent(Selection.gameObjects, componentName, isDeactive);
                RemoveComponent<UICustomText>();
                //TODO: ke them tool
                AddTextMeshPro();
            }
        }
        
        if (GUILayout.Button("Add Button Custom"))
        {
            if (Selection.gameObjects.Length > 0)
            {
                FindAllComponent(Selection.gameObjects, componentName, isDeactive);
                RemoveComponent<UICustomButton>();
                //TODO: ke them tool
                AddButton();
            }
        }
        

        GUILayout.Space(20);

        // show serializefied
        serializedObject.Update();

        EditorGUILayout.PropertyField(componentsProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    public void FindAllComponent(GameObject[] goes, string componentName, bool isDeactive)
    {
        components.Clear();
        foreach (GameObject go in goes)
        {
            //get tat ca cac component cung ten
            components.AddRange(go.GetComponentsInChildren(componentType[componentName], isDeactive));
        }

    }   

    public void RemoveComponent<T>() where T : Component    
    {
        for (int i = components.Count - 1; i >= 0; i--)
        {
            if (components[i].GetComponent<T>() != null)
            {
                components.RemoveAt(i);
            }
        }
    }   


    
    public void CheckAllComponent(GameObject[] goes, string componentName, bool isDeactive)
    {
        components.Clear();
        foreach (GameObject go in goes)
        {
            //get tat ca cac component cung ten
            components.AddRange(go.GetComponentsInChildren(componentType[componentName], isDeactive));
        }

        for (int i = components.Count - 1; i >= 0 ; i--)
        {
            if (components[i].GetComponent<UICustomText>() != null)
            {
                components.RemoveAt(i);
            }
        }
    }

    private int GetSelectedIndex()
    {
        for (int i = 0; i < stringOptions.Length; i++)
        {
            if (componentName == stringOptions[i])
            {
                return i;
            }
        }

        return 0; // Default selection
    }

    private void AddTextMeshPro()
    {

        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] is TextMeshProGUIOv)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(textmeshproUGUIov, components[i].transform.parent);
                CopyPasteComponent.PasteTransformValues<RectTransform>(components[i].gameObject, go);
                CopyPasteComponent.PasteTransformValues<TextMeshProUGUI>(components[i].gameObject, go);
                go.name = components[i].name;
            }
            else
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(textmeshproUGUI, components[i].transform.parent);
                CopyPasteComponent.PasteTransformValues<RectTransform>(components[i].gameObject, go);
                CopyPasteComponent.PasteTransformValues<TextMeshProUGUI>(components[i].gameObject, go);
                go.name = components[i].name;
            }
        }
    }

    private void AddButton()
    {

        for (int i = 0; i < components.Count; i++)
        {
            if (components[i] is Button)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(button, components[i].transform.parent);
                CopyPasteComponent.PasteTransformValues<RectTransform>(components[i].gameObject, go);
                CopyPasteComponent.PasteTransformValues<Button>(components[i].gameObject, go);
                CopyPasteComponent.PasteTransformValues<Image>(components[i].gameObject, go);
                go.SetActive(components[i].gameObject.activeSelf);
                go.name = components[i].name;
            }
        }
    }
}
#endif
