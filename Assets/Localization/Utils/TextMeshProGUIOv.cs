using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshProGUIOv : TMPro.TextMeshProUGUI
{
    private string m_TextBase = "-?";
    public string TextBase { get { if (m_TextBase == "-?") { m_TextBase = text; }; return m_TextBase; } }
}
