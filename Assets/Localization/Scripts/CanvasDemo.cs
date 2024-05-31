using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDemo : MonoBehaviour
{
    public Button btnEnglishLanguage;
    public Button btnVietnameseLanguage;

    private void Start()
    {
        btnEnglishLanguage.onClick.AddListener(() => ChangeLanguage(LanguageType.Eng));
        btnVietnameseLanguage.onClick.AddListener(() => ChangeLanguage(LanguageType.Vn));
    }

    private void ChangeLanguage(LanguageType eng)
    {
        LocalizeManager.Ins.ChangeLanguage(eng);
    }
}
