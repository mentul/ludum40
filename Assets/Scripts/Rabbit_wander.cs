using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_wander : State
    {
        public float wanderTime = 20f;
        public float time;
        CapsuleCollider2D collider;
        public float playerTriggerDistance = 3f;
        PlayerController player;

        public override void Enter()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            collider = GetComponent<CapsuleCollider2D>();
            Vector2 direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right;
            /*while (Physics2D.CapsuleCast(transform.position, collider.size, collider.direction, 0f, direction, 0.1f))
            {
                direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right;
            }*/
            GetComponent<Rigidbody2D>().velocity = direction*2f;
            //print(gameObject.name + " entering wander");
            time = wanderTime;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) < playerTriggerDistance)
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_triggered)));
            }
            else {
                if (time > 0) time -= Time.deltaTime;
                else
                {
                    stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_idle)));
                }
                //print(gameObject.name + " executing wander");
            }
        }

        public override void Exit()
        {
            //print(gameObject.name + " exiting wander");
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            print(gameObject.name + " received " + msg.Subject);
            return false;
        }
        
    }
}
