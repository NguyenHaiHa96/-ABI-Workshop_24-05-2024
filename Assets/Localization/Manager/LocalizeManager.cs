using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LanguageType 
{ 
    Eng = 0, 
    Vn = 1,
    Korean = 3,
    China = 4,
    Russia = 5,
}

public class LocalizeManager : Singleton<LocalizeManager>
{
    [SerializeField] LocalizeData data;
    private Dictionary<LocalizeType, LocalizeItem> dictionLocalize = new Dictionary<LocalizeType, LocalizeItem>();

    public Action OnChangeLocalizeAction;

    public static LanguageType LocalizeType;

    public void ChangeLanguage(LanguageType localizeType)
    {
        LocalizeType = localizeType;
        OnChangeLocalizeAction?.Invoke();
        //TODO: thay doi ca mot so UI
    }

    public string GetText(LocalizeType localizeType)
    {
        if (!dictionLocalize.ContainsKey(localizeType))
        {
            Debug.LogError(localizeType);
        }

        return dictionLocalize[localizeType].trans[(int)LocalizeType];
    }

    protected override void OnSingletonAwake()
    {
        base.OnSingletonAwake();
        for (int i = 0; i < data.Localizes.Length; i++)
        {
            dictionLocalize.Add(data.Localizes[i].type, data.Localizes[i]);
        }
    }
}

