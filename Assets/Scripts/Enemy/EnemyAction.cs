using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [Serializable]
    public struct EnemyAction
    {
        public string actionName;
        public Vector3 actionLocation;
        public Vector3 locationCenter;
        public float locationRange;
        public EnemyBaseState state;
    }
}
