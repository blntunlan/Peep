using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }
    
    
    public event Action<GameState> OnGameStateChanged; 
    [SerializeField] private EggCounterUI _eggCounterUI;
    [SerializeField] private WinLoseUI _winLoseUI;
    [SerializeField] private int _maxEggCount = 4;
    private GameState _currentGameState;
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

    private void OnEnable()
    {
        ChangeGameState(GameState.Play);
    }

    public void OnEggCollected()
    {
        _currentEggCount++;
        debugText = $"🥚 Egg Count: {_currentEggCount}/{_maxEggCount}";
        _eggCounterUI.SetEggCount(_currentEggCount, _maxEggCount);
        Debug.Log(debugText);
        if (_currentEggCount == _maxEggCount)
        {
            _eggCounterUI.SetEggCompleted();
            _winLoseUI.OnGameWin();
            ChangeGameState(GameState.GameOver);
            
        }
    }

    public void ChangeGameState(GameState state)
    {
        OnGameStateChanged?.Invoke(state);
        _currentGameState = state;
        Debug.Log($"Game State Changed: {_currentGameState}");
    }

    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }
}

