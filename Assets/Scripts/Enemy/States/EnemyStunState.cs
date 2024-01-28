using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyStunState : EnemyBaseState
    {
        float currentDuration;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // spawn laugh particles
            enemy.Laugh.OnStunned();

            // play animation
            enemy.Anim.Play("Stunned");

            // resetting some variables
            currentDuration = 0f;
            enemy.Agent.SetDestination(enemy.transform.position); 
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            if (currentDuration < enemy.stunDuration)
            {
                currentDuration += Time.deltaTime;
                return;
            }

            enemy.SwitchState(enemy.Patrol);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            
        }
    }
}
