using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Mammoth_wander : State
    {
        public float wanderTime = 20f;
        public float time;
        public float timeToChangeDirection = 1f;
        float directionTime = 2f;
        public float playerTriggerDistance = 3f;
        PlayerController player;
        Vector2 direction;

        public override void Enter()
        {
            GetComponent<Animator>().SetBool("isIdling", false);
            GetComponent<Animator>().SetBool("Attack", false);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector2.right;
            GetComponent<Rigidbody2D>().velocity = direction;
            //print(gameObject.name + " entering wander");
            directionTime = timeToChangeDirection;
            time = wanderTime;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) < playerTriggerDistance && !player.died)
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_triggered)));
            }
            else
            {
                if (directionTime <= 0)
                {
                    direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10)) * direction;
                    GetComponent<Rigidbody2D>().velocity = direction;
                }
                else directionTime -= Time.deltaTime;
                if (time > 0) time -= Time.deltaTime;
                else
                {
                    stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
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
