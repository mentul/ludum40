using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_triggered : State
    {
        public float playerTriggerOffDistance = 3f;
        PlayerController player;
        public float speedBoost = 10f;

        public override void Enter()
        {
            GetComponent<Animator>().SetBool("isIdling", false);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GetComponent<Rigidbody2D>().velocity = -(player.transform.position-transform.position).normalized * GetComponent<Animal>().speed * speedBoost;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > playerTriggerOffDistance)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_idle)));
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = -(player.transform.position - transform.position).normalized * GetComponent<Animal>().speed * speedBoost;
                if (GetComponent<Rigidbody2D>().velocity.x < 0f) GetComponent<SpriteRenderer>().flipX = true;
                else GetComponent<SpriteRenderer>().flipX = false;
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
