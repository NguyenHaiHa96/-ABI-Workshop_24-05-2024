using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [field: SerializeField] public RectTransform myRectTransform { get; set; }
    public static ButtonUI ButtonUISelect;
    public virtual void Setup()
    {
        if(myRectTransform == null) { myRectTransform = GetComponent<RectTransform>(); }
    }
    public virtual void AddListener()
    {

    }
    public virtual void RemoveEvent()
    {

    }
    public virtual void DOKillNoti()
    {

    }
    public virtual void SelectThis()
    {
        ButtonUISelect = this;
    }
}
