using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Mammoth_triggered : State
    {
        public float playerTriggerOffDistance = 3f;
        PlayerController player;

        public override void Enter()
        {
            GetComponent<Animator>().SetBool("isIdling", false);
            GetComponent<Animator>().SetBool("Attack", true);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GetComponent<Rigidbody2D>().velocity = (player.transform.position-transform.position).normalized*2f;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > playerTriggerOffDistance)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * 2f;
            }
        }

        public override void Exit()
        {
            //print(gameObject.name + " exiting idle");
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            print(gameObject.name + " received " + msg.Subject);
            return false;
        }
        
    }
}
