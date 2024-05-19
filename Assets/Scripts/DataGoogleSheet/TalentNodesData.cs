using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "TalentNodesData", menuName = "ScriptableObjects/Talent Nodes Data")]
public class TalentNodesData : ScriptableObject
{
    [field: SerializeField] public List<TalentNodeData> TalentNodeDataList { get; set; } = new();

#if UNITY_EDITOR

    public TalentNodeData GetTalentNodeDataByID(int id)
    {
        for (int i = 0; i < TalentNodeDataList.Count; i++)
        {
            if (TalentNodeDataList[i].TalentID != id) continue;
            return TalentNodeDataList[i];
        }
        return new TalentNodeData();
    }
    
#endif
    
}

[Serializable]
public class TalentNodeData
{
    [field: SerializeField, OnValueChanged("OnValueChange")] public int TalentID { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange"), HideInInspector] public string TalentNodeName { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange")] public ETalentType TalentType { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange")] public EBuffType BuffType { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange")] public ESpecialTalent SpecialTalent { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange")] public int NodeRow { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange")] public int NodeColumn { get; set; }
    [field: SerializeField, OnValueChanged("OnValueChange")] 
    public List<int> NodesIDRequired { get; set; } = new();
    
    public TalentNodeData()
    {
        
    }
    
    #region Get Set Functions

    public virtual int GetTalentID() => TalentID;
    
    public virtual System.Enum GetNodeType()
    {
        return null;
    }

    public virtual System.Enum GetTalentType()
    {
        return TalentType;
    }

    public virtual System.Enum GetBuffType()
    {
        return BuffType;
    }
    
    public virtual System.Enum GetSpecialTalent()
    {
        return SpecialTalent;
    }
    
    public virtual void SetNodeType(System.Enum value)
    {
        
    }

    public virtual void SetTalentType(System.Enum value)
    {
        TalentType = (ETalentType)value;
    }

    public virtual void SetBuffType(System.Enum value)
    {
        BuffType = (EBuffType)value;
    }
    
    public virtual void SetSpecialTalent(System.Enum value)
    {
        SpecialTalent = (ESpecialTalent)value;
    }

    public string GetTalentNodeName()
    {
        return TalentType switch
        {
            ETalentType.None => $"Node_Row: {NodeRow.ToString()}_Column: {NodeColumn.ToString()}_None_None",
            ETalentType.BasicAttribute =>
                $"Node_Row: {NodeRow.ToString()}_Column: {NodeColumn.ToString()}_BasicAttribute_{GetBuffType()}",
            ETalentType.Special =>
                $"Node_Row: {NodeRow.ToString()}_Column: {NodeColumn.ToString()}_SpecialTalent_{GetSpecialTalent()}",
            _ => null
        };
    }
    
    public void LoadData(TalentNodeLevelDataConfig talentNodeLevelDataConfig)
    {
        TalentType = talentNodeLevelDataConfig.talentType;
        BuffType = talentNodeLevelDataConfig.buffType;
        SpecialTalent = talentNodeLevelDataConfig.specialTalent;
        OnValueChange();
    }
    
    #endregion
    
    #region Editor Functions
    
#if UNITY_EDITOR
    [HideInInspector]
    public Vector2 position;
    public UnityAction onValueChange;
    
    public virtual Sprite GetImageIcon()
    {
        switch (TalentType)
        {
            case ETalentType.Special:
                return null;
            case ETalentType.BasicAttribute:
                return TalentTreeDataGlobalConfig.Instance != null ? TalentTreeDataGlobalConfig.Instance.GetTalentSpriteBasicAttribute((EBuffType)GetBuffType()) : null;
        }
        return null;
    }
    
#endif

    public void OnValueChange()
    {
#if UNITY_EDITOR
        onValueChange?.Invoke();
#endif
    }
    
    #endregion
    
}

