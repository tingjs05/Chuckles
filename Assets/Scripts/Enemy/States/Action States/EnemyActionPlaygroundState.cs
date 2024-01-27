using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemyActionPlaygroundState : EnemyBaseState
    {
        public override void OnEnter(EnemyStateMachine enemy)
        {

        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {

        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            enemy.Agent.updatePosition = true;
        }
    }
}
