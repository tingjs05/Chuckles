using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyChaseState : EnemyBaseState
    {
        EnemyStateMachine enemy;
        Collider[] players;
        Collider player;
        float timeSinceLastLaugh;

        public static event Action<float> CapturedWhileChasing;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // spawn laugh particles
            enemy.Laugh.OnCharge();
            // reset laugh time elapsed counter
            timeSinceLastLaugh = 0f;
            // cache enemy so that event listener can use
            this.enemy = enemy;
            // set speed to sprint speed
            enemy.Agent.speed = enemy.sprintSpeed;
            // subscribe to event to listen if photo is taken
            CameraCapture.TakenPictureOfEnemy += PictureTaken;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            // update animation
            enemy.UpdateMovementAnim();
            
            // laugh after certain period
            if (timeSinceLastLaugh >= 3.0f)
            {
                enemy.Laugh.OnCharge();
                timeSinceLastLaugh = 0f;
            }
            else
            {
                timeSinceLastLaugh += Time.deltaTime;
            }

            // check if player is within range
            players = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);

            // if player is not within range, return to patrol state
            if (players.Length <= 0)
            {
                enemy.SwitchState(enemy.Patrol);
                return;
            }
            
            // order player list by distance
            player = players.OrderBy(x => Vector3.Distance(enemy.transform.position, x.transform.position)).ToArray()[0];

            // get the location below the player and try to move there
            enemy.Agent.SetDestination(player.transform.position);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            // unsubscribe from event
            CameraCapture.TakenPictureOfEnemy -= PictureTaken;
        }

        void PictureTaken(float pictureQuality)
        {
            // invoke event when picture is taken while chasing player
            CapturedWhileChasing?.Invoke(pictureQuality);

            // ensure enemy and player are not null
            if (enemy == null || player == null) return;

            // check if player is within stun range
            float dist = Vector3.Distance(enemy.transform.position, player.transform.position);

            // stun the enemy if picture quality is more than threshold, and is within range
            if (dist <= enemy.stunDistance && pictureQuality >= enemy.pictureQualityStunThreshold)
            {
                enemy.SwitchState(enemy.Stun);
            }
        }
    }
}
