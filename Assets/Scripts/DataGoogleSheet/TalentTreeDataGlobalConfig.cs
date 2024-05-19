using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;
using TW.Utility.CustomType;
using TW.Utility.Extension;

//using TW.Utility.CustomType.SimpleJSON;

[CreateAssetMenu(fileName = "TalentTreeDataGlobalConfig", menuName = "Global Configs/Talent Tree Data Global Config")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class TalentTreeDataGlobalConfig : GlobalConfig<TalentTreeDataGlobalConfig>
{
    private const string STR_SEPARATOR = ";";
    private const string STR_TALENT_ID = "TalentID";
    private const string STR_TALENT_TYPE = "Type";

    [Header("Talent Level Data")] 
    public string fullLinkSheet;
    public string linkSheet;
    public string tabName;
    public TalentNodesData talentNodesData;
    public TalentNodesData testTalentNodesData;

    [Header("Talent Position Data")] 
    public string talentPositionLinkSheet;
    public string talentPositionTabName;
    public List<TalentTreePositionData> talentTreePositionsData;

    [Header("Test")] 
    public string talentDataLinkSheet;
    public string talentDataTabName;
    public List<TalentTreeDataConfig> talentTreeDataConfigs = new();
    
    [Header("Talent Nodes Levels Data")] 
    public List<TalentNodeLevelDataConfig> talentNodesLevelDataConfig = new();
    
    [Header("Talent Node Sprite Data")] 
    public TalentNodeSpriteDataConfig talentNodeSpriteDataConfig;

    public BigNumber gemResetTalentTreeRequire; 
    
    private int _talentID;
    private ETalentType _talentType;
    private string _strTalentID;
    
    public int GetNumberOfTalentNodes() => talentNodesData.TalentNodeDataList.Count;
    
    public EBuffType GetTalentBuffTypeByTalentID(int id)
    {
        for (int i = 0; i < talentNodesData.TalentNodeDataList.Count; i++)
        {
            if (talentNodesData.TalentNodeDataList[i].TalentID != id) continue;
            return (EBuffType)talentNodesData.TalentNodeDataList[i].GetBuffType();
        }
        return EBuffType.None;
    }

    public float GetTotalTalentBuffTypeValueByTalentID(int id, int level)
    {
        float value = 0;
        for (int i = 0; i < talentNodesLevelDataConfig.Count; i++)
        {
            if (talentNodesLevelDataConfig[i].ID != id) continue;
            for (int j = 0; j < talentNodesLevelDataConfig[i].TalentNodesLevelData.Count; j++)
            {
                if (talentNodesLevelDataConfig[i].TalentNodesLevelData[j].Level > level) continue;
                value += talentNodesLevelDataConfig[i].TalentNodesLevelData[j].Value;
            }
        }
        return value;
    }
    
#if UNITY_EDITOR
    
    [Button]
    private async void LoadTalentLevelDataFromSheet()
    {
        EditorUtility.SetDirty(this);
        talentNodesLevelDataConfig.Clear();
        if (string.IsNullOrEmpty(linkSheet)) return;
        string data = await ABakingSheet.GetCsv(linkSheet, tabName);
        List<Dictionary<string, string>> dataDictionaries = CSVReader.ReadStringData(data);
        TalentNodeLevelDataConfig talentNodeLevelDataConfig = new();
        for (int i = 0; i < dataDictionaries.Count; i++)
        {
            Dictionary<string, string> dataLine = dataDictionaries[i];
            _strTalentID = dataLine[STR_TALENT_ID];
            if (!string.Equals(_strTalentID, STR_SEPARATOR))
            {
                _talentID = int.Parse(_strTalentID); 
                _talentType = (ETalentType)(int.Parse(dataLine[STR_TALENT_TYPE]) + 1);
                talentNodeLevelDataConfig = new(_talentID, _talentType, dataLine);
                talentNodesLevelDataConfig.Add(talentNodeLevelDataConfig);
            }
            talentNodeLevelDataConfig.LoadData(dataLine);
        }

        if (talentNodesData.TalentNodeDataList.Count <= 0) return;
        for (int i = 0; i < talentNodesLevelDataConfig.Count; i++)
        {
            for (int j = 0; j < talentNodesData.TalentNodeDataList.Count; j++)
            {
                if (talentNodesLevelDataConfig[i].ID != talentNodesData.TalentNodeDataList[j].TalentID) continue;
                talentNodesData.TalentNodeDataList[j].LoadData(talentNodesLevelDataConfig[i]);
            }
        }
        AssetDatabase.SaveAssets();
    }

    [Button]
    private async void LoadTalentPositionFromSheet()
    {
        EditorUtility.SetDirty(this);
        talentTreePositionsData.Clear();
        if (string.IsNullOrEmpty(talentPositionLinkSheet)) return;
        string data = await ABakingSheet.GetCsv(talentPositionLinkSheet, talentPositionTabName);
        List<Dictionary<string, string>> dataDictionaries = CSVReader.ReadStringData(data);
        for (int i = 0; i < dataDictionaries.Count; i++)
        {
            Dictionary<string, string> dataLine = dataDictionaries[i];
            TalentTreePositionData talentTreePositionData = new();
            talentTreePositionData.LoadData(dataLine);
            talentTreePositionsData.Add(talentTreePositionData);
        }
        if (talentNodesData.TalentNodeDataList.Count <= 0) return;
        for (int i = 0; i < talentTreePositionsData.Count; i++)
        {
            for (int j = 0; j < talentTreePositionsData[i].Columns.Count; j++)
            {
                for (int k = 0; k < talentNodesData.TalentNodeDataList.Count; k++)
                {
                    if (string.IsNullOrEmpty(talentTreePositionsData[i].Columns[j])) continue;
                    if (int.Parse(talentTreePositionsData[i].Columns[j]) != talentNodesData.TalentNodeDataList[k].TalentID) continue;
                    talentNodesData.TalentNodeDataList[k].NodeRow = talentTreePositionsData[i].Row;
                    talentNodesData.TalentNodeDataList[k].NodeColumn = j;
                }
            }
        }
        AssetDatabase.SaveAssets();
    }
    
    [Button]
    private async void LoadTestTalentPositionFromSheet()
    {
        EditorUtility.SetDirty(this);
        talentTreeDataConfigs.Clear();
        if (string.IsNullOrEmpty(talentDataLinkSheet)) return;
        string data = await ABakingSheet.GetCsv(talentDataLinkSheet, talentDataTabName);
        List<Dictionary<string, string>> dataDictionaries = CSVReader.ReadStringData(data);
        for (int i = 0; i < dataDictionaries.Count; i++)
        {
            Dictionary<string, string> dataLine = dataDictionaries[i];
            TalentTreeDataConfig talentTreeDataConfig = new();
            talentTreeDataConfig.LoadData(dataLine);
            talentTreeDataConfigs.Add(talentTreeDataConfig);
        }
        
        if (testTalentNodesData.TalentNodeDataList.Count <= 0) return;
        for (int i = 0; i < testTalentNodesData.TalentNodeDataList.Count; i++)
        {
            testTalentNodesData.TalentNodeDataList[i].NodesIDRequired.Clear();
        }
        for (int i = 0; i < talentTreeDataConfigs.Count; i++)
        {
            for (int j = 0; j < talentTreeDataConfigs[i].Columns.Count; j++)
            {
                List<int> ids = talentTreeDataConfigs[i].Columns[j];
                for (int k = 0; k < testTalentNodesData.TalentNodeDataList.Count; k++)
                {
                    if (ids.Count <= 1) continue;
                    if (ids[0] != testTalentNodesData.TalentNodeDataList[k].TalentID) continue;
                    testTalentNodesData.TalentNodeDataList[k].NodesIDRequired.AddRange(Helper.GetNewListExcludeIndex(ids, ids.Count - 1));
                }
            }
        }
        
        AssetDatabase.SaveAssets();
    }
    
    public void AddTalentNode(TalentNodeData talentNodeData)
    {
        talentNodesData.TalentNodeDataList.Add(talentNodeData);
    }

    public void ReOrderTalentNodesData()
    {
        talentNodesData.TalentNodeDataList = talentNodesData.TalentNodeDataList.OrderBy(x => x.TalentID).ToList();
    }
    
    public Sprite GetTalentSpriteBasicAttribute(EBuffType buffType) =>
        (from t in talentNodeSpriteDataConfig.basicAttributeTalentNodesData where t.buffType == buffType select t.sprIconLock).FirstOrDefault();
    
    public int GetMaxRow()
    {
        if (talentNodesData.TalentNodeDataList == null || talentNodesData.TalentNodeDataList.Count == 0) return 0;
        int maxRow = talentNodesData.TalentNodeDataList[0].NodeRow;
        for (int i = 0; i < talentNodesData.TalentNodeDataList.Count; i++)
        {
            if (maxRow < talentNodesData.TalentNodeDataList[i].NodeRow)
            {
                maxRow = talentNodesData.TalentNodeDataList[i].NodeRow;
            }
        }
        return maxRow;
    }

    public TalentNodeData GetGetTalentNodeDataByRowAndColumn(int row, int column)
    {
        for (int i = 0; i < talentNodesData.TalentNodeDataList.Count; i++)
        {
            if (talentNodesData.TalentNodeDataList[i].NodeRow == row && talentNodesData.TalentNodeDataList[i].NodeColumn == column)
            {
                return talentNodesData.TalentNodeDataList[i];
            }
        }
        return null;
    }
        
    
#endif
    
}

[Serializable]
public class TalentNodeLevelDataConfig
{
    private const string STR_TALENT_TYPE = "Type";
    private const string STR_BUFF_TYPE = "BuffType";
    private const string STR_LEVEL = "Level";
    private const string STR_VALUE = "Value";
    private const string STR_RED_FLASK_REQUIRE = "FlaskRequire";
    private const string STR_DESCRIPTION = "Description";
    
    [Space]
    [SerializeField] private int iD;
    public ETalentType talentType;
    public EBuffType buffType;
    public ESpecialTalent specialTalent;
    [SerializeField] private List<TalentNodeLevelData> talentNodesLevelData = new();
    private int _level;
    private float _value;
    private int _refFlaskRequire;
    private string _strDescription;
    
    #region Getter Setter

    public int ID
    {
        get => iD;
        set => iD = value;
    }

    public List<TalentNodeLevelData> TalentNodesLevelData
    {
        get => talentNodesLevelData;
        set => talentNodesLevelData = value;
    }
    
    #endregion
    
    public TalentNodeLevelDataConfig()
    {
        
    }
    
    public TalentNodeLevelDataConfig(int iD, ETalentType type, Dictionary<string, string> data)
    {
        this.iD = iD;
        talentType = type;
        switch (talentType)
        {
            case ETalentType.None:
                break;
            case ETalentType.BasicAttribute:
                buffType = Helper.ConvertStringToEnum<EBuffType>(data[STR_BUFF_TYPE], EBuffType.None);
                break;
            case ETalentType.Special:
                specialTalent = Helper.ConvertStringToEnum<ESpecialTalent>(data[STR_BUFF_TYPE], ESpecialTalent.None);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void LoadData(Dictionary<string, string> data)
    {
        if (talentNodesLevelData == null) return;
        TalentNodeLevelData talentNodeLevelData = new();
        _level = int.Parse(data[STR_LEVEL]);
        _value = float.Parse(data[STR_VALUE]);
        _strDescription = data[STR_DESCRIPTION];
        if (string.IsNullOrEmpty(_strDescription))
        {
            _refFlaskRequire = int.Parse(data[STR_RED_FLASK_REQUIRE]);
            talentNodeLevelData = new(_level, _value, _refFlaskRequire, _strDescription);
        }
        else
        {
            _refFlaskRequire = int.Parse(data[STR_RED_FLASK_REQUIRE]);
            talentNodeLevelData = new(_level, _value, _refFlaskRequire, _strDescription);
        }
        talentNodesLevelData.Add(talentNodeLevelData);
    }
}

[Serializable]
public class TalentNodeLevelData
{
    [SerializeField] [LabelText("Level")] private int l;
    [SerializeField] [LabelText("Value")] private float v;
    [SerializeField] [LabelText("Red Flask")] private int rF;
    [SerializeField] private string strDescription;

    #region Get Set
    
    public int Level
    {
        get => l;
        set => l = value;
    }

    public float Value
    {
        get => v;
        set => v = value;
    }

    public int RedFlask
    {
        get => rF;
        set => rF = value;
    }
    
    #endregion
    
    public TalentNodeLevelData()
    {
        
    }

    public TalentNodeLevelData(int level, float value, int redFlaskRequire)
    {
        l = level;
        v = value;
        rF = redFlaskRequire;
    }

    public TalentNodeLevelData(int level, float value, int redFlaskRequire, string description)
    {
        l = level;
        v = value;
        rF = redFlaskRequire;
        strDescription = description;
    }

    public string GetDescription() => string.IsNullOrEmpty(strDescription) ? "" : string.Format(strDescription, Value);
}

[Serializable]
public class TalentTreePositionData
{
    private const string STR_ROW = "Row";
    private const string STR_COLUMN_00 = "Column00";
    private const string STR_COLUMN_01 = "Column01";
    private const string STR_COLUMN_02 = "Column02";
    
    [field: SerializeField, LabelText("Row")] public int Row { get; set; }
    [field: SerializeField, LabelText("Columns")] public List<string> Columns { get; set; } = new();

    public void LoadData(Dictionary<string, string> data)
    {
        Row = int.Parse(data[STR_ROW]);
        Columns.Add(string.IsNullOrEmpty(data[STR_COLUMN_00]) ? Constants.EmptyString : data[STR_COLUMN_00]);
        Columns.Add(string.IsNullOrEmpty(data[STR_COLUMN_01]) ? Constants.EmptyString : data[STR_COLUMN_01]);
        Columns.Add(string.IsNullOrEmpty(data[STR_COLUMN_02]) ? Constants.EmptyString : data[STR_COLUMN_02]);
    }
}

[Serializable]
public class TalentTreeDataConfig
{
    private const string STR_ROW = "Row";
    private const string STR_COLUMN_00 = "Column00";
    private const string STR_COLUMN_01 = "Column01";
    private const string STR_COLUMN_02 = "Column02";
    private const string SPLITTER = ";";
    [field: SerializeField] public int Row { get; set; }
    [field: ShowInInspector] public List<List<int>> Columns { get; set; } = new();
    
    public void LoadData(Dictionary<string, string> data)
    {
        Row = int.Parse(data[STR_ROW]);
        Columns.Add(Helper.StringToIntList(data[STR_COLUMN_00], SPLITTER));
        Columns.Add(Helper.StringToIntList(data[STR_COLUMN_01], SPLITTER));
        Columns.Add(Helper.StringToIntList(data[STR_COLUMN_02], SPLITTER));
    }
}

