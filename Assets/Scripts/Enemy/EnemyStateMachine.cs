using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStateMachine : MonoBehaviour
    {
        public EnemyBaseState State { get; private set; }

        // states
        public EnemyPatrolState Patrol { get; private set; } = new EnemyPatrolState();
        public EnemyAlertState Alert { get; private set; } = new EnemyAlertState();
        public EnemyChaseState Chase { get; private set; } = new EnemyChaseState();

        // action states
        [field: SerializeField] public EnemyBaseState[] EnemyActionStates { get; private set; }

        // components
        public NavMeshAgent Agent { get; private set; }

        void Start()
        {
            // get components
            Agent = GetComponent<NavMeshAgent>();

            // set default state
            State = Patrol;
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
}

