using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private RectTransform _timerRectTransform;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private Ease easeType = Ease.Linear;
    
    private float _elapsedTime;
    private Tweener _rotationTween;
    private string _finalTime;
    

    private void Start()
    {
        
        PlayRotationAnimation();
        StartTimer();
        GameManager.Instance.OnGameStateChanged += InstanceOnOnGameStateChanged;
    }

    private void InstanceOnOnGameStateChanged(GameState obj)
    {
        switch (obj)
        {
            case GameState.Pause:
                StopTimer();
                break;
            case GameState.Resume:
                ResumeTimer();
                break;
            case GameState.GameOver:
                FinishTimer();
                break;
        }
    }

    private void FinishTimer()
    {
        StopTimer();
        _finalTime = GetTimeString();   
    }

    public string GetTimeString()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnDestroy()
    {
        // DOTween tweenlerini temizle
        _rotationTween?.Kill();
        CancelInvoke(nameof(UpdateTimerUI));
    }

    private void PlayRotationAnimation()
    {
        _rotationTween = _timerRectTransform
            .DORotate(new Vector3(0, 0, -360), rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(easeType)
            .SetUpdate(true); // Time.timeScale'den bağımsız çalışır
    }

    private void StartTimer()
    {
        _elapsedTime = 0f;
        InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
    }

    private void UpdateTimerUI()
    {
        _elapsedTime += 1f;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }

    // Timer'ı durdurmak için yardımcı metod
    public void StopTimer()
    {
        CancelInvoke(nameof(UpdateTimerUI));
        _rotationTween?.Pause();
    }

    // Timer'ı devam ettirmek için
    public void ResumeTimer()
    {
        InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
        _rotationTween?.Play();
    }

    // Timer'ı sıfırlamak için
    public void ResetTimer()
    {
        _elapsedTime = 0f;
        UpdateTimerUI();
    }
}