using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyBaseState State { get; private set; }

    // states

    void Start()
    {
        State.OnEnter(this);
    }

    void Update()
    {
        State.OnUpdate(this);
    }

    public void SwitchState(EnemyBaseState state)
    {
        State.OnExit(this);
        State = state;
        State.OnEnter(this);
    }
}
