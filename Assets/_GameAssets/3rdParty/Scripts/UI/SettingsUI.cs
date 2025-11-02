using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _mainmenuButton;
    
    [Header("Objects")]
    [SerializeField] private GameObject _settingsPopupObject;
    [SerializeField] private GameObject _blackBackgroundObject;
    
    private Image _blackBackgroundImage;
    [SerializeField] private float _blackBackgroundDuration;
    [SerializeField] private float _fadeDuration;

    private void Awake()
    {
        _blackBackgroundImage = _blackBackgroundObject.GetComponent<Image>();
        _settingsPopupObject.transform.localScale = Vector3.zero;
        
        _settingsButton.onClick.AddListener(OnSettingsButtonClick);
        _resumeButton.onClick.AddListener(OnResumeButtonClick);

    }

    private void OnSettingsButtonClick()
    {
        GameManager.Instance.ChangeGameState(GameState.Pause);
        
        _settingsPopupObject.SetActive(true);
        _blackBackgroundObject.SetActive(true);

        _blackBackgroundImage.DOFade(0.8f, _blackBackgroundDuration).SetEase(Ease.Linear);
        _settingsPopupObject.transform.DOScale(1.5f, _blackBackgroundDuration).SetEase(Ease.OutBack);
    }

    private void OnResumeButtonClick()
    {
        _blackBackgroundImage.DOFade(0f, _fadeDuration).SetEase(Ease.Linear);
        _settingsPopupObject.transform.DOScale(0f, _fadeDuration).SetEase(Ease.OutExpo)
            .OnComplete(() =>
            {
                GameManager.Instance.ChangeGameState(GameState.Resume);
                _settingsPopupObject.SetActive(false);
                _blackBackgroundObject.SetActive(false);
            });
    }
}
