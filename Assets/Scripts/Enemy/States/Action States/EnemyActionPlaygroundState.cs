using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyActionPlaygroundState : EnemyBaseState
    {
        EnemyStateMachine enemy;
        float actionDuration;
        float durationInAction;

        public static event Action CapturedWhileInPlayground;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // set action duration
            actionDuration = UnityEngine.Random.Range(enemy.minActionDuration, (enemy.maxActionDuration + 1f));
            // reset action duration counter
            durationInAction = 0f;

            // don't allow enemy to move
            enemy.Agent.enabled = false;
            enemy.rb.isKinematic = true;

            // subscribe to event to listen if photo is taken
            CameraCapture.TakenPictureOfEnemy += PictureTaken;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            // check if player is nearby
            if (enemy.PlayerNearbyAndPatrol()) return;

            // increment duration in state
            durationInAction += Time.deltaTime;

            // if duration in state more than action duration, return to patrol state
            if (durationInAction > actionDuration)
            {
                enemy.SwitchState(enemy.Patrol);
            }
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            // allow enemy to move
            enemy.Agent.enabled = true;
            enemy.rb.isKinematic = false;

            // unsubscribe from event
            CameraCapture.TakenPictureOfEnemy -= PictureTaken;

            // start cooldown before next action can be performed
            enemy.StartActionCooldown();
        }

        // event listeners
        void PictureTaken(float pictureQuality)
        {
            // invoke event when photo is taken while in action
            CapturedWhileInPlayground?.Invoke();

            // chase player if player takes a photo within the detection range of the enemy
            if (enemy == null) return;
            Collider[] players = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);
            if (players.Length > 0) enemy.SwitchState(enemy.Chase);
        }
    }
}
