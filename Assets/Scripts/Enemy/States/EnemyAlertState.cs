using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player;

namespace Enemy
{
    public class EnemyAlertState : EnemyBaseState
    {
        EnemyStateMachine enemy;
        Vector3 point;
        Collider[] players;
        Collider player;

        public override void OnEnter(EnemyStateMachine enemy)
        {
            // cache enemy so that event listener can use
            this.enemy = enemy;
            // set speed to walk speed
            enemy.Agent.speed = enemy.walkSpeed;
            // subscribe to event to listen if photo is taken
            CameraCapture.TakenPictureOfEnemy += PictureTaken;
            // subscribe to event to listen when within light
            enemy.Listener.OnLaugh += enemy.CheckChase;
        }

        public override void OnUpdate(EnemyStateMachine enemy)
        {
            // update animation
            enemy.UpdateMovementAnim();

            players = Physics.OverlapSphere(enemy.transform.position, enemy.alertRange, enemy.playerMask);

            if (players.Length <= 0)
            {
                enemy.SwitchState(enemy.Patrol);
            }

            player = players.OrderBy(x => Vector3.Distance(enemy.transform.position, x.transform.position)).ToArray()[0];

            // if player is within chase range or player lit up enemy, start chasing player
            if (Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.chaseRange || player.GetComponent<PlayerVisibility>()?.ClownInLight)
            {
                enemy.Laugh.OnLaugh();
                enemy.SwitchState(enemy.Chase);
                return;
            }

            // set a random location to walk to around player's location
            if (!(enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance)) return;
            
            if (!enemy.RandomPoint(player.transform.position, enemy.chaseRange, out point)) return;

            enemy.Agent.SetDestination(point);
        }

        public override void OnExit(EnemyStateMachine enemy)
        {
            // unsubscribe from events
            CameraCapture.TakenPictureOfEnemy -= PictureTaken;
            enemy.Listener.OnLaugh -= enemy.CheckChase;
        }

        void PictureTaken(float pictureQuality)
        {
            // chase player if player takes a photo within the detection range of the enemy
            if (enemy == null) return;
            enemy.CheckChase();
        }
    }
}
