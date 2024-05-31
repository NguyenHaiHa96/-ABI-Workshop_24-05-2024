using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using UnityEngine;

public class UICustomButton : MonoBehaviour
{
    [field: SerializeField] public RectTransform myRectTransform { get; private set; }
    [field: SerializeField] public Image ImageThis { get; private set; }
    [field: SerializeField, InlineEditor] public ButtonConfig ButtonConfig { get; private set; }

    [field: ValueDropdown(nameof(CustomChangeTextPreset), DrawDropdownForListElements = false)]
    [field: SerializeField] public string TextPreset { get; private set; }
    [field: SerializeField] public SpriteButtonPresetType SpritePreset { get; set; }
    [field: SerializeField] public SpriteButtonPresetType SpritePresetOFF { get; private set; }

    [SerializeField] private bool sound = true;

    private void Awake()
    {
        if(myRectTransform == null) { myRectTransform = GetComponent<RectTransform>(); } 
        if(ImageThis == null) { ImageThis = GetComponent<Image>(); }
        if (sound)
        {
            GetComponent<Button>()?.onClick.AddListener(PlayFx);
        } 
    }
    private IEnumerable CustomChangeTextPreset()
    {
        //show lai list name de select
        if (ButtonConfig == null) return null;
        return ButtonConfig.Presets.Select(x => x.Name);
    }
    [Button]
    public void ChangeUIOn()
    {
        if (SpritePreset != SpriteButtonPresetType.None)
        {
            ButtonConfig.SpriteButtonPreset spriteButtonPreset = ButtonConfig.SpriteButtonPresets[(int)SpritePreset];
            ImageThis.sprite = spriteButtonPreset.Sprite;
        }
    }
    [Button]
    public void ChangeUIOff()
    {
        if (SpritePresetOFF != SpriteButtonPresetType.None)
        {
            ButtonConfig.SpriteButtonPreset spriteButtonPreset = ButtonConfig.SpriteButtonPresets[(int)SpritePresetOFF];
            ImageThis.sprite = spriteButtonPreset.Sprite;
        }
    }
    public void ChangeUI(bool IsOn)
    {
        if (IsOn) { ChangeUIOn(); }
        else { ChangeUIOff(); }
    }

    public void PlayFx()
    {
        
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
        if (TextPreset != "None")
        {
            ButtonConfig.Preset preset = ButtonConfig.Presets.FirstOrDefault(x => x.Name == TextPreset);
            if (myRectTransform == null) { myRectTransform = GetComponent<RectTransform>(); }
            myRectTransform.sizeDelta = preset.Size;
            //add sound
        }
        if (SpritePreset != SpriteButtonPresetType.None)
        {
            ButtonConfig.SpriteButtonPreset spriteButtonPreset = ButtonConfig.SpriteButtonPresets[(int)SpritePreset];
            if (ImageThis == null) { ImageThis = GetComponent<Image>(); }
            if (ImageThis != null) { ImageThis.sprite = spriteButtonPreset.Sprite; }
        }
    }

    [Button]
    private void AddLink()
    {
        ButtonConfig.AddLink(this);
    }
#endif
}
