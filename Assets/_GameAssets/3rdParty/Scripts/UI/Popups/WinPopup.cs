using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinPopup : MonoBehaviour
{
    [SerializeField] private TimerUI _timerUI;
    [SerializeField] private TMP_Text _winTimerText;
    [SerializeField] private Button _oneMoreButton;
    [SerializeField] private Button _mainMenuButton;
    
    private void OnEnable()
    {

        _winTimerText.text = _timerUI.GetTimeString();
        _oneMoreButton.onClick.AddListener(OneMoreButtonClick);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);
    }

    private void OneMoreButtonClick()
    {
        SceneManager.LoadScene(Consts.GameScene.GAME_SCENE);
    }

    private void MainMenuButtonClick()
    {
        SceneManager.LoadScene(Consts.GameScene.MAIN_MENU_SCENE);
    }
}
