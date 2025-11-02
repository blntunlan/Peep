using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class EggCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _eggCounterText;
    [SerializeField] private Color _eggCounterColor;
    [SerializeField] private float ColorChangeDuration = 1f;
    private RectTransform _eggCounterRectTransform;
    [SerializeField] private float _scale = 1f;

    private void Awake()
    {
        _eggCounterRectTransform = _eggCounterText.rectTransform;
        _eggCounterText.text = String.Empty;
    }

    public void SetEggCount(int count, int max)
    {
        _eggCounterText.text = count.ToString() + "/" + max.ToString();
    }

    public void SetEggCompleted()
    {
        _eggCounterText.DOColor(_eggCounterColor, ColorChangeDuration).SetEase(Ease.OutBack);
        _eggCounterRectTransform.DOScale(1.2f, _scale).SetEase(Ease.Flash);
    }
}
