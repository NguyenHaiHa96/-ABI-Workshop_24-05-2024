using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "TalentNodeSpriteDataConfig", menuName = "ScriptableObjects/Talent Node Sprite Data Config")]
public class TalentNodeSpriteDataConfig : ScriptableObject
{
    [Header("Icon")]
    public List<TalentNodeSpriteData> basicAttributeTalentNodesData;
    public List<TalentNodeSpriteData> specialTalentsData;
}

[Serializable]
public class TalentNodeSpriteData
{
    public ETalentType talentType;
    public EBuffType buffType;
    public ESpecialTalent specialTalent;
    [PreviewField(80, ObjectFieldAlignment.Center)] public Sprite sprIconLock;
    [PreviewField(80, ObjectFieldAlignment.Center)] public Sprite sprIconUnlock;

    public TalentNodeSpriteData()
    {
        
    }
}