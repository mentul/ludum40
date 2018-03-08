using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_triggered : State
    {
        public float playerTriggerOffDistance = 3f;
        PlayerController player;
        public float speedBoost = 10f;
        Animal animal;

        public override void Enter()
        {
            animal = GetComponent<Animal>();
            animal.animalAnimator.SetBool("isIdling", false);
            player = GameController.Current.player;
            animal.animalRigidbody.velocity = -(player.transform.position-transform.position).normalized * animal.speed * speedBoost;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > playerTriggerOffDistance)
            {
                animal.animalRigidbody.velocity = Vector2.zero;
                stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_idle)));
            }
            else
            {
                animal.animalRigidbody.velocity = -(player.transform.position - transform.position).normalized * animal.speed * speedBoost;
                if (animal.animalRigidbody.velocity.x < 0f) animal.animalSpriteRenderer.flipX = true;
                else animal.animalSpriteRenderer.flipX = false;
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
