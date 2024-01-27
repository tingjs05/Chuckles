using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyPatrolState : EnemyBaseState
    {
        EnemyStateMachine enemy;
        Vector3 point;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // cache enemy
            this.enemy = enemy;
            // set speed to walk speed
            enemy.Agent.speed = enemy.walkSpeed;
            // subscribe to event to listen if photo is taken
            CameraCapture.TakenPictureOfEnemy += PictureTaken;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            // check if player is within chase range
            Collider player = Physics.OverlapSphere(enemy.transform.position, enemy.chaseRange, enemy.playerMask);
            if (player != null)
            {
                enemy.SwitchState(enemy.Chase);
                return;
            }

            // set a random location to walk towards to patrol
            if (!(enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)) return;
            
            if (!RandomPoint(enemy.transform.position, enemy.patrolRange, out point)) return;

            Debug.DrawRay(point, Vector3.up, Color.blue, 1f);
            enemy.Agent.SetDestination(point);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            // unsubscribe from event
            CameraCapture.TakenPictureOfEnemy -= PictureTaken;
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

        void PictureTaken(float pictureQuality)
        {
            if (enemy == null) return;
            Collider player = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);
            if (player != null) enemy.SwitchState(enemy.Chase);
        }
    }
}
