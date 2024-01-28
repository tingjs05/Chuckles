using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBaseState : MonoBehaviour
    {
        public abstract void OnEnter(EnemyStateMachine enemy);

        public abstract void OnUpdate(EnemyStateMachine enemy);

        public abstract void OnExit(EnemyStateMachine enemy);
    }
}
