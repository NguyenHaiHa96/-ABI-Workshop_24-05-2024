using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;

public enum PresetType
{
    Title_1 = 0,
    Title_2 = 1,
    Title_3 = 2,
    Content_1 = 3,
    Content_2 = 4,
    Button_0 = 5,
    Button_1 = 6,
    Button_2 = 7,
    Quote_1 = 8,
    Quote_2 = 9,
    Quote_3 = 10,
    Quote_4 = 11,
    Normal = 12,
}

[CreateAssetMenu(fileName = "TextMeshConfig", menuName = "UIConfig/TextMeshConfig")]
public class TextMeshConfig : ScriptableObject
{
    //list config
    [field: SerializeField] public PresetData[] PresetDatas { get; private set; }
    public Preset[] Presets => PresetDatas[0].Presets;
    //list link
    [SerializeField] List<GameObject> UICanvas = new List<GameObject>();

    public Preset GetPreset(PresetType presetType)
    {
        return PresetDatas[(int)LocalizeManager.LocalizeType].GetPreset(presetType);
    }

#if UNITY_EDITOR
    [Button]
    public void ReloadAll()
    {
        //reload lai toan bo nhung text lien quan
        //UICustomTexts.RemoveAll(x => x == null);
        //for (int i = 0; i < UICustomTexts.Count; i++)
        //{
        //    //save prefab lai neu no dang o trong mot prefab khac
        //}

        //tim ca tren scene lan tat ca cac prefab xong reload lai
     
        //tim tat ca trong prefab root
        //neu la prefab thi save lai prefab do
        for (int i = 0; i < UICanvas.Count; i++)
        {
            UICustomText[] customs = UICanvas[i].GetComponentsInChildren<UICustomText>(true);
            for (int j = 0; j < customs.Length; j++)
            {
                customs[j].ReloadUI();
            }

            SaveAssetEditor(UICanvas[i]);
        }

        ////tim tat ca tren scene
        //UICustomText[] customTexts = FindObjectsOfType<UICustomText>();
        //for (int i = 0; i < customTexts.Length; i++)
        //{
        //    customTexts[i].ReloadUI();
        //}
    }

    public void AddLink(UICustomText customText)
    {
        ////add parent prefab
        ////check parent prefab khong phai chinh no thi moi add
        //GameObject prefabParent = customText.transform.root.GetChild(0).gameObject;
        //GameObject rootPrefab = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(customText);

        //if (prefabParent != null && prefabParent != rootPrefab)
        //{
        //    //lay prefab goc trong folder
        //    GameObject prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(prefabParent);
        //    //add vao link note
        //    UICanvas.Add(prefabParent);
        //}

    }

    public static void SaveAssetEditor(UnityEngine.Object go)
    {
        UnityEditor.Undo.RegisterCompleteObjectUndo(go, "Save level data");
        UnityEditor.EditorUtility.SetDirty(go);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
#endif
}


