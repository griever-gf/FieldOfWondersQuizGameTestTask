using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineBase
{
    public IState CurrentState { get; protected set; }

    public void ChangeStay(IState nextState, IStateData data)
    {
        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter(data);
    }

    public void Update(float deltaTime)
    {
        CurrentState?.Update(deltaTime);
    }
}
