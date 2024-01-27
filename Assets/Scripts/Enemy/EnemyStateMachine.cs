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
        public float patrolRange = 25f;
        public float alertRange = 15f;
        public float chaseRange = 10f;

        [Header("Stun Variables")]
        public float stunDuration = 1.5f;
        public float stunDistance = 12f;
        [Range(0f, 100f)]
        public float pictureQualityStunThreshold = 40f;

        [Header("Layer Masks")]
        public LayerMask playerMask;
        public LayerMask obstacleMask;

        [Header("Toggle Gizmos")]
        public bool showDetectionRanges = false;
        public bool showActionLocations = true;

        // current state of enemy
        public EnemyBaseState State { get; private set; }

        // states
        public EnemyPatrolState Patrol { get; private set; } = new EnemyPatrolState();
        public EnemyAlertState Alert { get; private set; } = new EnemyAlertState();
        public EnemyChaseState Chase { get; private set; } = new EnemyChaseState();
        public EnemyStunState Stun { get; private set; } = new EnemyStunState();

        // action states
        [field: Header("Actions")]
        [field: SerializeField] public EnemyAction[] EnemyActions { get; private set; }

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
            if (!showDetectionRanges) return;

            Gizmos.DrawWireSphere(transform.position, patrolRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, alertRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseRange);
        }

        void OnDrawGizmos()
        {
            if (!showActionLocations) return;

            // show enemy action locations
            foreach (EnemyAction action in EnemyActions)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(action.locationCenter, action.locationRange);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(action.actionLocation, 0.5f);
            }
        }

        public void SwitchState(EnemyBaseState state)
        {
            State.OnExit(this);
            State = state;
            State.OnEnter(this);
        }

        public bool RandomPoint(Vector3 center, float range, out Vector3 result)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            UnityEngine.AI.NavMeshHit hit;
            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 1f, UnityEngine.AI.NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }

            result = Vector3.zero;
            return false;
        }

        public int CheckActionLocation()
        {
            for (int i = 0; i < EnemyActions.Length; i++)
            {
                if (Vector3.Distance(transform.position, EnemyActions[i].locationCenter) <= EnemyActions[i].locationRange) return i;
            }
            return -1;
        }

        public void GoToActionLocation(int i)
        {
            Agent.SetDestination(EnemyActions[i].actionLocation);
            Agent.Warp(EnemyActions[i].actionLocation);
            Agent.updatePosition = false;
        }
    }
}

