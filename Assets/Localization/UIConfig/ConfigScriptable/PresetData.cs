using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[CreateAssetMenu(fileName = "PresetData", menuName = "UIConfig/PresetData")]
public class PresetData : ScriptableObject
{
    [field: SerializeField] public Preset[] Presets { get; private set; }

    public Preset GetPreset(PresetType presetType)
    {
        return Presets[(int)presetType];
    }
}

public enum MyFontStyles
{
    N = FontStyles.Normal,
    B = FontStyles.Bold,
    I = FontStyles.Italic,
    U = FontStyles.Underline,
    S = FontStyles.Strikethrough,
    AA = FontStyles.UpperCase,
    ab = FontStyles.LowerCase,
    SC = FontStyles.SmallCaps,
}

[Serializable]
public class Preset
{
    public string Name = "Normal";
    public PresetType PresetType = PresetType.Normal;
    public Vector2 OffsetText = new Vector2(0, 0);
    public float FontSize = 20;
    public TMP_FontAsset FontAsset;
    [EnumToggleButtons] public MyFontStyles PresetStyle = MyFontStyles.N;
}