using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR

public class CopyPasteComponent  : EditorWindow
{

    //--------------------------------------------------------
    private static GameObject[] copys;

    //Shortcut
    [MenuItem("Auto/CopyPaste/Copy Transform Values", false, -101)]
    private static void CopyTransformValues()
    {
        //luu danh sach can copy
        if (Selection.gameObjects.Length == 0) return;
        copys = Selection.gameObjects;
    }

    [MenuItem("Auto/CopyPaste/Paste Transform Values", false, -101)]
    private static void PasteTransformValues()
    {
        //paste danh sach
        if (Selection.gameObjects.Length == 0) return;

        GameObject[] pastes = Selection.gameObjects;

        int amount = Mathf.Min(copys.Length, pastes.Length);

        for (int i = 0; i < amount; i++)
        {
            //pastes[i].name = copys[i].name;
            PasteTransformValues<Transform>(copys[i], pastes[i]);
        }
    }   
    
    [MenuItem("Auto/CopyPaste/Paste Component Values &n", false, -101)]
    private static void PasteAllComponent()
    {
        //paste danh sach
        if (Selection.gameObjects.Length == 0) return;

        GameObject[] pastes = Selection.gameObjects;

        int amount = Mathf.Min(copys.Length, pastes.Length);

        for (int i = 0; i < amount; i++)
        {
            pastes[i].name = copys[i].name;
            PasteTransformValues<Transform>(copys[i], pastes[i]);
            AddAllComponent(copys[i], pastes[i]);
        }
    }

    private void PasteTransformValues(Component copy, Component paste)
    {
        UnityEditorInternal.ComponentUtility.CopyComponent(copy);
        UnityEditorInternal.ComponentUtility.PasteComponentValues(paste);
    }

    public static void PasteTransformValues<T>(GameObject copy, GameObject paste) where T : Component
    {
            UnityEditorInternal.ComponentUtility.CopyComponent(copy.GetComponent<T>());
            UnityEditorInternal.ComponentUtility.PasteComponentValues(paste.GetComponent<T>());
    }
    
    public static void AddAllComponent(GameObject copy, GameObject paste)
    {
        Component[] components = copy.GetComponents<Component>();

        paste.name = copy.name;

        for (int i = 0; i < components.Length; i++)
        {
            Component pasteComponent = null;

            if (paste.GetComponent(components[i].GetType()) == null)
            {
                pasteComponent = paste.AddComponent(components[i].GetType());
            }

            UnityEditorInternal.ComponentUtility.CopyComponent(components[i]);
            UnityEditorInternal.ComponentUtility.PasteComponentValues(pasteComponent);
        }
    }

}

#endif
