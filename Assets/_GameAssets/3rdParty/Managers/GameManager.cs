using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private int _maxEggCount = 4;
    private string debugText;
    private int _currentEggCount = 0;

    private void Awake()
    {
        // Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnEggCollected()
    {
        _currentEggCount++;
        debugText = $"🥚 Egg Count: {_currentEggCount}/{_maxEggCount}";
        _eggCounterUI.SetEggCount(_currentEggCount, _maxEggCount);
        Debug.Log(debugText);
        if (_currentEggCount == _maxEggCount)
        {
            Debug.Log("You Win!");
            _eggCounterUI.SetEggComplted();
        }
    }

}
