using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Mammoth_triggered : State
    {
        public float playerTriggerOffDistance = 3f;
        PlayerController player;
        public float speedBoost = 10f;
        public float chargeTime = 2f;
        float time = 0;

        public override void Enter()
        {
            time = chargeTime;
            GetComponent<Animator>().SetBool("isIdling", false);
            GetComponent<Animator>().SetBool("Attack", true);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GetComponent<Rigidbody2D>().velocity = (player.transform.position-transform.position).normalized * speedBoost;
            if (GetComponent<Rigidbody2D>().velocity.x < 0f) GetComponent<SpriteRenderer>().flipX = true;
            else GetComponent<SpriteRenderer>().flipX = false;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > playerTriggerOffDistance || time<=0f)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
            }
            time -= Time.deltaTime;
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
