using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyPatrolState : EnemyBaseState
    {
        EnemyStateMachine enemy;
        Vector3 point;
        int actionIndex;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // can giggle
            enemy.Giggle = true;
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
            if (enemy.PlayerNearbyAndPatrol()) return;

            // if no player and not on action cooldown, try to perform action
            if (enemy.actionCooldown == null && InActionArea(enemy)) return;

            // otherwise, just patrol the area, set a random location to walk towards to patrol
            if (!(enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)) return;
            
            if (!enemy.RandomPoint(enemy.transform.position, enemy.patrolRange, out point)) return;

            enemy.Agent.SetDestination(point);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            // stop giggle
            enemy.Giggle = false;
            // unsubscribe from event
            CameraCapture.TakenPictureOfEnemy -= PictureTaken;
        }

        // behaviour checks
        bool InActionArea(EnemyStateMachine enemy)
        {
            // random chance to choose not to perform action
            if (Random.Range(0f, 1f) > enemy.actionChance) return false;

            // check for action location
            actionIndex = enemy.CheckActionLocation();

            if (actionIndex == -1) return false;

            enemy.Agent.SetDestination(enemy.EnemyActions[actionIndex].actionLocation);

            // check if player is at action location
            if (enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)
            {
                enemy.GoToActionLocation(actionIndex);
                enemy.SwitchState(enemy.EnemyActions[actionIndex].state);
            }

            return true;
        }

        // event listeners
        void PictureTaken(float pictureQuality)
        {
            // chase player if player takes a photo within the detection range of the enemy
            if (enemy == null) return;
            Collider[] players = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);
            if (players.Length > 0) enemy.SwitchState(enemy.Chase);
        }
    }
}
