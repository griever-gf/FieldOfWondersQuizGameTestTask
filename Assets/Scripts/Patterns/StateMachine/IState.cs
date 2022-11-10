using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter(IStateData data);

    void Update(float deltaTime);

    void Exit();
}

public interface IStateData
{

}