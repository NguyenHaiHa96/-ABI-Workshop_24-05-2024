using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UICustomText : MonoBehaviour
{
    [field: SerializeField, InlineEditor] public TextMeshConfig TextMeshConfig { get; private set; }
    [field: SerializeField] public TextMeshProUGUI TextField { get; private set; }

    [field: ValueDropdown(nameof(CustomChangeTextPreset), DrawDropdownForListElements = false)]
    [field: SerializeField] public string TextPreset { get; private set; }

    [SerializeField, ReadOnly] float size;

    [field: SerializeField] public LocalizeType localizeType { get; private set; }
    [field: SerializeField] public PresetType presetType { get; set; }

    private IEnumerable CustomChangeTextPreset()
    {
        //show lai list name de select
        if (TextMeshConfig == null) return null;
        return TextMeshConfig.Presets.Select(x => x.Name);
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
        //thay doi 1 thong so nao day se reload lai
        ReloadUI();
    }

    [Title("Editor Button")]
    [Button]
    public void ReloadUI() 
    {
        if (TextField == null) TextField = GetComponent<TextMeshProUGUI>();

        //TextMeshConfig.Preset preset = TextMeshConfig.Presets.FirstOrDefault(x => x.Name == TextPreset);
        Preset preset = TextMeshConfig.GetPreset(presetType);

        if (preset == null)
        {
            Debug.Log($"{TextPreset} - {transform.root.name}");
        }

        //TextField.enableWordWrapping = false;
        //TextField.overflowMode = TextOverflowModes.Overflow;

        TextField.fontSize = preset.FontSize;
        TextField.font = preset.FontAsset;
        TextField.fontStyle = (FontStyles)preset.PresetStyle;

        size = preset.FontSize;

        ////convert
        //presetType = TextPreset switch
        //{
        //    "Title 1" => PresetType.Title_1,
        //    "Title 2" => PresetType.Title_2,
        //    "Title 3" => PresetType.Title_3,
        //    "Content 1" => PresetType.Content_1,
        //    "Content 2" => PresetType.Content_2,
        //    "Button 0" => PresetType.Button_0,
        //    "Button 1" => PresetType.Button_1,
        //    "Button 2" => PresetType.Button_2,
        //    "Quote 1" => PresetType.Quote_1,
        //    "Quote 2" => PresetType.Quote_2,
        //    "Quote 3" => PresetType.Quote_3,
        //    "Quote 4" => PresetType.Quote_4,
        //    "Normal" => PresetType.Normal,
        //    _ => PresetType.Normal,
        //};
    }

    internal void Reload()
    {
       
    }

    [Button]
    private void AddLink()
    {
        TextMeshConfig.AddLink(this);
    }

    private void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.red;
        style.fontSize = 10; 

        UnityEditor.Handles.Label(transform.position, TextPreset, style);
    }
#endif

    #region Localize

    public void OnReloadText()
    {
        if (localizeType != LocalizeType.None)
        {
            TextField.SetText(LocalizeManager.Ins.GetText(localizeType));
        }

        TextField.fontSize = TextMeshConfig.GetPreset(presetType).FontSize;
        TextField.font = TextMeshConfig.GetPreset(presetType).FontAsset;
        TextField.fontStyle = (FontStyles)TextMeshConfig.GetPreset(presetType).PresetStyle;

        TextField.characterSpacing = TextMeshConfig.GetPreset(presetType).OffsetText.x;
        TextField.wordSpacing = TextMeshConfig.GetPreset(presetType).OffsetText.y;
    }

    private void OnEnable()
    {
        OnReloadText();
        LocalizeManager.Ins.OnChangeLocalizeAction += OnReloadText;
    }

    private void OnDisable()
    {
        LocalizeManager.Ins.OnChangeLocalizeAction -= OnReloadText;
    }

    private void OnDestroy()
    {
        LocalizeManager.Ins.OnChangeLocalizeAction -= OnReloadText;
    }

    #endregion
}
