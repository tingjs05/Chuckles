using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyChaseState : EnemyBaseState
    {
        Collider[] players;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // set speed to sprint speed
            enemy.Agent.speed = enemy.sprintSpeed;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            // check if player is within range
            players = Physics.OverlapSphere(enemy.transform.position, enemy.detectionRange, enemy.playerMask);

            // if player is not within range, return to patrol state
            if (players.Length <= 0)
            {
                enemy.SwitchState(enemy.Patrol);
                return;
            }
            
            // order player list by distance
            players = players.OrderBy(x => Vector3.Distance(enemy.transform.position, x.transform.position)).ToArray();

            // get the location below the player and try to move there
            RaycastHit hit;
            if (Physics.Raycast(players[0].transform.position, -Vector3.up, out hit, Mathf.Infinity))
            {
                enemy.Agent.SetDestination(hit.point);
            }
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            
        }
    }
}
