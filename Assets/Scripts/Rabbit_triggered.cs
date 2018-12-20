using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_triggered : State
    {
        public float playerTriggerOffDistance = 3f;
        public float speedBoost = 10f;

        public override void Enter()
        {
            myAnimator.SetBool("isIdling", false);
            myRigidbody.velocity = -(player.transform.position-transform.position).normalized * myAnimal.speed * speedBoost;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > playerTriggerOffDistance)
            {
                myRigidbody.velocity = Vector2.zero;
                stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_idle)));
            }
            else
            {
                myRigidbody.velocity = -(player.transform.position - transform.position).normalized * myAnimal.speed * speedBoost;
                if (myRigidbody.velocity.x < 0f) mySpriteRenderer.flipX = true;
                else mySpriteRenderer.flipX = false;
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
