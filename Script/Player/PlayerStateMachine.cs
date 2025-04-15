using UnityEngine;

public class PlayerStateMach 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerState currentState {  get; private set; }

    public void Initialize(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();

    }
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
