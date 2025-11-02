using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePopup : MonoBehaviour
{
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private TMP_Text _loseTimerText;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _mainMenuButton;

    private void Awake()
    {
        _tryAgainButton.onClick.AddListener(OnTryAgainButtonClick);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
    }

    private void OnTryAgainButtonClick()
    {
        SceneManager.LoadScene(Consts.GameScene.GAME_SCENE);
    }

    private void OnMainMenuButtonClick()
    {
        
    }
}
