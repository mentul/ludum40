using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rabbit_die : State
    {
        public override void Enter()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            
            foreach (var item in GetComponents<CapsuleCollider2D>())
            {
                item.enabled = false;
            }
            GetComponent<Animator>().SetBool("Die", true);

        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
            GetComponent<Animator>().SetBool("Die", false);
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
