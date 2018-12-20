using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_wander : State
    {
        public float wanderTime = 20f;
        public float time;
        public float playerTriggerDistance = 3f;
        PlayerController player;

        public override void Enter()
        {
            GetComponent<Animator>().SetBool("isIdling", false);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Vector2 direction = Quaternion.Euler(0, 0, GeneratedMap.pseudoRandom.Next(0, 360)) * Vector2.right;

            if (direction.x < 0f) GetComponent<SpriteRenderer>().flipX = true;
            else GetComponent<SpriteRenderer>().flipX = false;
            GetComponent<Rigidbody2D>().velocity = direction*GetComponent<Animal>().speed;
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
            }
        }

        public override void Exit()
        {
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            switch (msg.Subject)
            {
                case "DIE":
                    stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_die)));
                    break;
            }
            return false;
        }
        
    }
}
