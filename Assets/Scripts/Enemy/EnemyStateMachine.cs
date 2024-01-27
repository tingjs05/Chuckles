using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyStateMachine : MonoBehaviour
    {
        // inspector elements
        [Header("Movement")]
        public float walkSpeed = 10f;
        public float sprintSpeed = 25f;

        [Header("Range Checks")]
        public float detectionRange = 25f;
        public float alertRange = 15f;
        public float chaseRange = 10f;

        [Header("Layer Masks")]
        public LayerMask playerMask;
        public LayerMask obstacleMask;

        // current state of enemy
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

            // disable rotation and movement for navmesh agent
            Agent.updateRotation = false;

            // set default state
            State = Patrol;
            State.OnEnter(this);
        }

        void Update()
        {
            State.OnUpdate(this);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, alertRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }

        public void SwitchState(EnemyBaseState state)
        {
            State.OnExit(this);
            State = state;
            State.OnEnter(this);
        }
    }
}

