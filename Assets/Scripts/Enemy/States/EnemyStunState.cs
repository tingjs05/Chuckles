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
            enemy.Agent.updatePosition = false;
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
            enemy.Agent.updatePosition = true;
        }
    }
}
