using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Laugh")]
        public float minGiggleCooldown = 10f;
        public float maxGiggleCooldown = 25f;
        [field: SerializeField] public LaughGenerator Laugh { get; private set; }
        [SerializeField] private UIManager uiManager;

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

        [Header("Actions")]
        public float minActionDuration = 5f;
        public float maxActionDuration = 15f;
        public float minActionCooldown = 25f;
        public float maxActionCooldown = 45f;
        [Range(0f, 1f)] public float actionChance = 0.3f;
        // action states
        [field: SerializeField] public EnemyAction[] EnemyActions { get; private set; }

        [field: Header("Animations")]
        [field: SerializeField] public Animator Anim { get; private set; }

        // components
        public NavMeshAgent Agent { get; private set; }
        public Rigidbody rb { get; private set; }

        // action cooldown coroutine
        [HideInInspector] public Coroutine actionCooldown;

        // movement variables
        private enum MoveType
        {
            Up, Down, Left, Right
        }
        private MoveType prevMoveDir = MoveType.Down;
        private bool prevIsRunning = false;
        private MoveType currMoveDirection;
        private Vector3 moveDir;
        private bool isRunning;

        // giggle variables
        private float currentGiggleCooldown = -1f;
        private float timeSinceLastGiggle = 0f;
        private bool giggle = false;
        public bool Giggle 
        {
            get { return giggle; }
            set 
            {
                // set giggle
                giggle = value;
                // giggle if value is true
                if (value == true)
                {
                    Laugh.OnGiggle();
                    // track clown when giggling
                    if (uiManager != null) uiManager.ClownTracking();
                }
                // reset counter variables
                timeSinceLastGiggle = 0f;
                currentGiggleCooldown = -1f;
            }
        }

        void Start()
        {
            // get components
            Agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();

            // disable rotation and movement for navmesh agent
            Agent.updateRotation = false;

            // set default state
            State = Patrol;
            State.OnEnter(this);
        }

        void Update()
        {
            State.OnUpdate(this);
            // check for giggle
            if (giggle) CheckGiggle();
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
            if (!showActionLocations || EnemyActions == null || EnemyActions.Length == 0) return;

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

        public bool PlayerNearbyAndPatrol()
        {
            // check if player is within chase range
            Collider[] players = Physics.OverlapSphere(transform.position, alertRange, playerMask);

            if (players.Length <= 0) return false;

            // spawn laugh particles when found player
            Laugh.OnLaugh();

            Collider player = players.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray()[0];

            // if player is within chase range, start chasing player
            if (Vector3.Distance(transform.position, player.transform.position) > chaseRange)
            {
                SwitchState(Chase);
                return true;
            }

            // otherwise, check if player is within line of sight, if player is within line of sight, start chasing player
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out hit, Mathf.Infinity, obstacleMask))
            {
                SwitchState(Chase);
                return true;
            }

            // if cannot see and not within chase range, switch to alerted state
            SwitchState(Alert);
            return true;
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
            if (EnemyActions == null || EnemyActions.Length == 0) return -1;

            for (int i = 0; i < EnemyActions.Length; i++)
            {
                if (Vector3.Distance(transform.position, EnemyActions[i].locationCenter) <= EnemyActions[i].locationRange) return i;
            }

            return -1;
        }

        public void GoToActionLocation(int i)
        {
            Agent.SetDestination(EnemyActions[i].actionLocation);
            Agent.enabled = false;
            transform.position = EnemyActions[i].actionLocation;
        }

        public void StartActionCooldown()
        {
            actionCooldown = StartCoroutine(ActionCooldown(Random.Range(minActionCooldown, (maxActionCooldown + 1f))));
        }

        public void UpdateMovementAnim()
        {
            // get move direction
            // moveDir = (Agent.nextPosition - transform.position).normalized;
            moveDir = Agent.desiredVelocity.normalized;

            // set anim dir based on move dir
            if (Mathf.Abs(moveDir.z) >= 0.5f)
            {
                currMoveDirection = moveDir.z > 0f? MoveType.Up : MoveType.Down;
            }
            else
            {
                currMoveDirection = moveDir.x > 0f? MoveType.Right : MoveType.Left;
            }

            // set sprint speed
            isRunning = Agent.speed == sprintSpeed;

            // do not change animation if animation state hasn't changed
            if (isRunning == prevIsRunning && currMoveDirection == prevMoveDir) return;

            // cache move dir and is running
            prevMoveDir = currMoveDirection;
            prevIsRunning = isRunning;

            // play move animation (yes i am sorry for your eyes, pls bear with me here)
            if (isRunning)
            {
                if (currMoveDirection == MoveType.Left)
                {
                    Anim.Play("RunLeft");
                }
                else if (currMoveDirection == MoveType.Right)
                {
                    Anim.Play("RunRight");
                }
                else if (currMoveDirection == MoveType.Up)
                {
                    Anim.Play("RunUp");
                }
                else
                {
                    Anim.Play("RunDown");
                }

                return;
            }

            if (currMoveDirection == MoveType.Left)
            {
                Anim.Play("WalkLeft");
            }
            else if (currMoveDirection == MoveType.Right)
            {
                Anim.Play("WalkRight");
            }
            else if (currMoveDirection == MoveType.Up)
            {
                Anim.Play("WalkUp");
            }
            else
            {
                Anim.Play("WalkDown");
            }
        }


        IEnumerator ActionCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            actionCooldown = null;
        }

        void CheckGiggle()
        {
            // set random giggle cooldown if not set
            if (currentGiggleCooldown < 0f)
            {
                currentGiggleCooldown = Random.Range(minGiggleCooldown, (maxGiggleCooldown + 1f));
            }

            // giggle if time elapsed is more than cooldown
            if (timeSinceLastGiggle >= currentGiggleCooldown)
            {
                // track clown when giggling
                if (uiManager != null) uiManager.ClownTracking();
                // giggle
                Laugh.OnGiggle();
                // reset counter variables
                timeSinceLastGiggle = 0f;
                currentGiggleCooldown = -1f;
                return;
            }

            // increment time elapsed since last giggle
            timeSinceLastGiggle += Time.deltaTime;
        }
    }
}

