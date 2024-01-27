using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyPatrolState : EnemyBaseState
    {
        Vector3 point;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            enemy.Agent.speed = enemy.walkSpeed;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            if (!(enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)) return;
            
            if (!RandomPoint(enemy.transform.position, enemy.patrolRange, out point)) return;

            Debug.DrawRay(point, Vector3.up, Color.blue, 1f);
            enemy.Agent.SetDestination(point);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            
        }

        bool RandomPoint(Vector3 center, float range, out Vector3 result)
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
    }
}
