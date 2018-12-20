using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rabbit_die : State
    {
        public override void Enter()
        {
            myRigidbody.velocity = Vector2.zero;
            
            foreach (var item in GetComponents<CapsuleCollider2D>())
            {
                item.enabled = false;
            }
            myAnimator.SetBool("Die", true);

        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            myAnimator.SetBool("Die", false);
            foreach (var item in GetComponents<CapsuleCollider2D>())
            {
                item.enabled = true;
            }
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            return true;
        }
    }
}
