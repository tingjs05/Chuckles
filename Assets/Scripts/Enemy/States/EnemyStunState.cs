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
            currentDuration = 0f;
            
            enemy.Agent.SetDestination(enemy.transform.position);
            // This will cause the agent position and transform position to be out of sync, hence agent will teleport when this state ends
            // Setting the destination to the current position is already enough to stop the agent from moving
            // enemy.Agent.updatePosition = false; 
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
            // enemy.Agent.updatePosition = true;
        }
    }
}
