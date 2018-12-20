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
        Vector2 direction;

        public override void Enter()
        {
            myAnimator.SetBool("isIdling", false);
            myAnimator.SetBool("Attack", false);
            direction = Quaternion.Euler(0, 0, GeneratedMap.pseudoRandom.Next(0, 360)) * Vector2.right;
            myRigidbody.velocity = direction;
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
                    direction = Quaternion.Euler(0, 0, GeneratedMap.pseudoRandom.Next(-10, 10)) * direction;
                    myRigidbody.velocity = direction;
                    if (direction.x < 0f) mySpriteRenderer.flipX = true;
                    else mySpriteRenderer.flipX = false;
                }
                else directionTime -= Time.deltaTime;
                if (time > 0) time -= Time.deltaTime;
                else
                {
                    stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
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
