using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_idle : State
    {
        public float eatTime = 10f;
        public float time;
        public float playerTriggerDistance = 3f;
        PlayerController player;

        public override void Enter()
        {
            GetComponent<Animator>().SetBool("isIdling", true);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //print(gameObject.name + " entering idle");
            time = eatTime;
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
                    stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_wander)));
                }
                //print(gameObject.name + " executing idle");
            }
        }

        public override void Exit()
        {
            //print(gameObject.name + " exiting idle");
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            print(gameObject.name + " received " + msg.Subject);
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
