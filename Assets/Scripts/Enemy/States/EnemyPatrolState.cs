using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyPatrolState : EnemyBaseState
    {
        EnemyStateMachine enemy;
        Vector3 point;
        Collider[] players;
        Collider player;
        int actionIndex;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // cache enemy so that event listener can use
            this.enemy = enemy;
            // set speed to walk speed
            enemy.Agent.speed = enemy.walkSpeed;
            // subscribe to event to listen if photo is taken
            CameraCapture.TakenPictureOfEnemy += PictureTaken;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            // check if player is nearby
            // check if player is within chase range
            players = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);

            if (players.Length > 0)
            {
                player = players.OrderBy(x => Vector3.Distance(enemy.transform.position, x.transform.position)).ToArray()[0];

                // if player is within chase range, start chasing player
                if (Vector3.Distance(enemy.transform.position, player.transform.position) > enemy.chaseRange)
                {
                    enemy.SwitchState(enemy.Chase);
                    return;
                }

                // otherwise, check if player is within line of sight.
                // if player is within line of sight, start chasing player
                RaycastHit hit;
                if (!Physics.Raycast(enemy.transform.position, (player.transform.position - enemy.transform.position).normalized, out hit, Mathf.Infinity, enemy.obstacleMask))
                {
                    enemy.SwitchState(enemy.Chase);
                    return;
                }

                // if cannot see and not within chase range, switch to alerted state
                enemy.SwitchState(enemy.Alert);
            }

            // if no player, try to perform action
            // check for action location
            actionIndex = enemy.CheckActionLocation();

            if (actionIndex != -1)
            {
                enemy.Agent.SetDestination(enemy.EnemyActions[actionIndex].actionLocation);

                // check if player is at action location
                if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
                {
                    enemy.GoToActionLocation(actionIndex);
                    enemy.SwitchState(enemy.EnemyActions[actionIndex].state);
                }

                return;
            }

            // otherwise, just patrol the area
            // set a random location to walk towards to patrol
            if (!(enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)) return;
            
            if (!enemy.RandomPoint(enemy.transform.position, enemy.patrolRange, out point)) return;

            enemy.Agent.SetDestination(point);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            // unsubscribe from event
            CameraCapture.TakenPictureOfEnemy -= PictureTaken;
        }

        void PictureTaken(float pictureQuality)
        {
            // chase player if player takes a photo within the detection range of the enemy
            if (enemy == null) return;
            players = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);
            if (players.Length > 0) enemy.SwitchState(enemy.Chase);
        }
    }
}
