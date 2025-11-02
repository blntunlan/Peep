using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _blackBackground;
    [SerializeField] private GameObject _winPopup;
    [SerializeField] private GameObject _losePopup;
    
    [Header("Settings")]
    [SerializeField] private float _animationDuration = 0.3f;
    
    private Image _blackBackgroundImage;
    private RectTransform _winPopupRectTransform;
    private RectTransform _losePopupRectTransform;

    private void Awake()
    {
        _blackBackgroundImage = _blackBackground.GetComponent<Image>();
        _winPopupRectTransform = _winPopup.GetComponent<RectTransform>();
        _losePopupRectTransform = _losePopup.GetComponent<RectTransform>();
    }
    
    

    public void OnGameWin()
    {
        _blackBackground.SetActive(true);
        _winPopup.SetActive(true);

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear);
        _winPopupRectTransform.DOScale(1.8f, _animationDuration).SetEase(Ease.OutBack);
    }

    public void OnGameLose()
    {
        _blackBackground.SetActive(true);
        _losePopup.SetActive(true);

        _blackBackgroundImage.DOFade(0.8f, _animationDuration).SetEase(Ease.Linear).SetLink(_blackBackground);
        _losePopupRectTransform.DOScale(1.8f, _animationDuration).SetEase(Ease.OutBack).SetLink(_losePopup);
    }
    
}
