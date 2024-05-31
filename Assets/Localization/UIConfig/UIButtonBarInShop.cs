using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIButtonBarInShop : ButtonUI
{
    [field: SerializeField] public Sprite HighLightSprite { get; private set; }
    [field: SerializeField] public RectTransform m_RectTransformNoti { get; private set; }
    [field: SerializeField] public RectTransform m_RectTransformNotiAds { get; private set; }
    private void Awake()
    {
        
    }
    public void ShowNotiAds()
    {
        m_RectTransformNotiAds.gameObject.SetActive(true);
    }
    public void HideNotiAds()
    {
        m_RectTransformNotiAds.gameObject.SetActive(false);
    }
    public void ShowNoti()
    {
        m_RectTransformNoti.gameObject.SetActive(true);
    }
    public void HideNoti()
    {
        m_RectTransformNoti.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        m_RectTransformNoti.DOKill();
        m_RectTransformNotiAds.DOKill();
    }
}
