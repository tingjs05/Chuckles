using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void OnEnter(EnemyStateMachine enemy);

    public abstract void OnUpdate(EnemyStateMachine enemy);

    public abstract void OnExit(EnemyStateMachine enemy);
}
