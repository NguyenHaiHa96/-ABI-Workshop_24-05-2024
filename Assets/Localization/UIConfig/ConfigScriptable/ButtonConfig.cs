using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum SpriteButtonPresetType
{
    None = -1,
    Button_0 = 0,
    Button_1 = 1,
    Button_2 = 2,
    Button_3 = 3,
    Button_4 = 4,
    Button_5 = 5,
    Button_6 = 6,
    Button_7 = 7,
    Button_8 = 8,
    Button_9 = 9,
    Button_10 = 10,
    Button_11 = 11,
}
[CreateAssetMenu(fileName = "ButtonConfig", menuName = "UIConfig/ButtonConfig")]
public class ButtonConfig : ScriptableObject
{
    [Serializable]
    public class Preset
    {
        public string Name = "Normal";
        public Vector2 Size = new Vector2(250, 150);
        public AudioClip ClickSound;
    }
    [Serializable]
    public class SpriteButtonPreset
    {
        public SpriteButtonPresetType Name = SpriteButtonPresetType.None;
        public Sprite Sprite;
    }
    [field: SerializeField] public Preset[] Presets { get; private set; }
    [field: SerializeField] public SpriteButtonPreset[] SpriteButtonPresets { get; private set; }

    //list link
    [field: SerializeField, ReadOnly] List<GameObject> UICanvas = new List<GameObject>();
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
            UICustomButton[] customs = UICanvas[i].GetComponentsInChildren<UICustomButton>();
            for (int j = 0; j < customs.Length; j++)
            {
                customs[j].ReloadUI();
            }

            SaveAssetEditor(UICanvas[i]);
        }

        //tim tat ca tren scene
        UICustomButton[] customButtons = FindObjectsOfType<UICustomButton>();
        for (int i = 0; i < customButtons.Length; i++)
        {
            customButtons[i].ReloadUI();
        }
    }

    public void AddLink(UICustomButton customButton)
    {
        //add parent prefab
        //check parent prefab khong phai chinh no thi moi add
        GameObject prefabParent = UnityEditor.PrefabUtility.GetOutermostPrefabInstanceRoot(customButton);

        if (prefabParent != null && prefabParent != customButton)
        {
            //lay prefab goc trong folder
            GameObject rootPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(prefabParent);
            //add vao link note
            UICanvas.Add(rootPrefab);
        }

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
