using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomType.SimpleJSON;
using TW.Utility.Extension;
using UnityEngine;

[CreateAssetMenu(fileName = "LocalizeData", menuName = "ScriptableObjects/Localize/LocalizeData", order = 1)]
public class LocalizeData : ScriptableObject
{
    [TableList(ShowIndexLabels = true)] public LocalizeItem[] Localizes;

#if UNITY_EDITOR

    [Button]
    private void OnInitData()
    {
        //Lay tat ca gia tri enum
        LocalizeType[] values = (LocalizeType[])System.Enum.GetValues(typeof(LocalizeType));
        LocalizeItem[] locals = new LocalizeItem[Utilities.GetEnumCount<LocalizeType>()];
        //init
        for (int i = 0; i < values.Length; i++)
        {
            locals[i] = GetLocalize(values[i]);
            locals[i].type = values[i];
        }

        Localizes = locals; 
    }

    private LocalizeItem GetLocalize(LocalizeType localizeType)
    {
        for (int i = 0; i < Localizes.Length; i++)
        {
            if (Localizes[i].type == localizeType)
            {
                return Localizes[i];
            }
        }

        return new LocalizeItem();
    }


    const string ENUMTYPE = "ENUMTYPE";
    const string ID = "ID";
    const string ENG = "ENG";
    const string VN = "VN";
    const string CHINA = "CHINA";
    const string KOREA = "KOREA";

    public string link;
    public string tab;

    [SerializeField] TextAsset localizeAsset;
    [Button]
    private async void InitEnum()
    {
        string data = await ABakingSheet.GetJson(link, tab);
        List <Dictionary<string, string>> itemConfig = new List<Dictionary<string, string>>();
        JSONArray jsonArray = JSON.Parse(data).AsArray;
        foreach (KeyValuePair<string, JSONNode> keyValuePair in jsonArray)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, JSONNode> valuePair in keyValuePair.Value)
            {
                dic.Add(valuePair.Key, valuePair.Value);
            }
            itemConfig.Add(dic);
        }

        //khoi tao list enum
        //List<Dictionary<string, string>> itemConfig = CSVReader.Read(localizeAsset, ",");

        //Debug.Log($"Init {itemConfig[1][ENUMTYPE]} - {itemConfig[1][ID]} - {itemConfig[1][ENG]} - {itemConfig[1][VN]}");
        //ket hop tat ca enum thanh 1 string
        string elementsString = "";
        for (int i = 0; i < itemConfig.Count; i++)
        {
            //neu dong nay trong thi enum cach them ra 1 dong
            if (itemConfig[i].ContainsKey(ENUMTYPE) && itemConfig[i][ENUMTYPE].Length > 0)
            {
                elementsString += $"{itemConfig[i][ENUMTYPE]} = {itemConfig[i][ID]},\n";
            }
            else
            {
                elementsString += "\n";
            }
        }

        GenerateEnum("LocalizeEnumName", "LocalizeType", elementsString);


        Debug.Log("Init Enum Done " + itemConfig.Count);
    }

    //Save file
    private void GenerateEnum(string fileName, string enumType, string enumElementsString)
    {
        string script =
        $@"
                using UnityEngine;

                public enum {enumType}
                {{
                {enumElementsString}
                }}
            ";

        string path = UnityEditor.EditorUtility.SaveFilePanel("Save Enum Script", "Assets/_Game/ScriptableObject/Localize", $"{fileName}.cs", "cs");
        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllText(path, script);
            UnityEditor.AssetDatabase.Refresh();
            Debug.Log($"Enum script generated at: {path}");
        }
    }


    [Button]
    private async void ConvertCsvToScriptable()
    {
        string data = await ABakingSheet.GetJson(link, tab);
        List<Dictionary<string, string>> itemConfig = new List<Dictionary<string, string>>();
        JSONArray jsonArray = JSON.Parse(data).AsArray;
        foreach (KeyValuePair<string, JSONNode> keyValuePair in jsonArray)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (KeyValuePair<string, JSONNode> valuePair in keyValuePair.Value)
            {
                dic.Add(valuePair.Key, valuePair.Value);
            }
            itemConfig.Add(dic);
        }

        //List<Dictionary<string, string>> itemConfig = CSVReader.Read(localizeAsset, ",");
        //tao data
        List<LocalizeItem> localizeItems = new List<LocalizeItem>();

        for (int i = 0; i < itemConfig.Count; i++)
        {
            //itemConfig[i][ENUMTYPE]
            //itemConfig[i][ID]
            //itemConfig[i][ENG]
            //itemConfig[i][VN]
            if (!itemConfig[i].ContainsKey(ENG)) itemConfig[i].Add(ENG, "");
            if (!itemConfig[i].ContainsKey(VN)) itemConfig[i].Add(VN, ""); 
            if (!itemConfig[i].ContainsKey(KOREA)) itemConfig[i].Add(KOREA, "");
            if (!itemConfig[i].ContainsKey(CHINA)) itemConfig[i].Add(CHINA, "");

            if (itemConfig[i].ContainsKey(ENUMTYPE) && itemConfig[i][ENUMTYPE].Length > 0)
            {
                //localizeItems.Add(new LocalizeItem() { type = (LocalizeType)(int.Parse(itemConfig[i][ID])), ENG = itemConfig[i][ENG], VN = itemConfig[i][VN] });
                localizeItems.Add(new LocalizeItem() { type = (LocalizeType)(int.Parse(itemConfig[i][ID])), trans = new string[4] { itemConfig[i][ENG], itemConfig[i][VN], itemConfig[i][KOREA], itemConfig[i][CHINA] } });
            }
        }

        Localizes = new LocalizeItem[localizeItems.Count];
        for (int i = 0; i < Localizes.Length; i++)
        {
            Localizes[i] = localizeItems[i];
        }

        //save scriptable
        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Save level data");
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }



#endif

}


[System.Serializable]
public struct LocalizeItem
{
    public LocalizeType type;
    public string[] trans;

    public LocalizeItem(LocalizeType type, string[] trans)
    {
        this.type = type;
        this.trans = trans;
    }
}