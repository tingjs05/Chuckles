using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyBaseState State { get; private set; }

    // states
    public EnemyIdleState Idle { get; private set; } = new EnemyIdleState();
    public EnemyPatrolState Patrol { get; private set; } = new EnemyPatrolState();
    public EnemyAlertState Alert { get; private set; } = new EnemyAlertState();
    public EnemyChaseState Chase { get; private set; } = new EnemyChaseState();

    [field: SerializeField] public EnemyBaseState[] EnemyActionStates;

    void Start()
    {
        State = Idle;
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
