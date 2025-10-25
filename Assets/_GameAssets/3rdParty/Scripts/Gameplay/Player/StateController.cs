using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState m_currentState = PlayerState.Idle;
    
    private void Start() 
    {
        ChangeState(PlayerState.Idle);
    }


    public void ChangeState(PlayerState newState)
    {
        if (m_currentState == newState) return;

        m_currentState = newState;
    }

    public PlayerState GetCurrentState()
    {
        return m_currentState;
    }
}
