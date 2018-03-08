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
        Animal animal;

        public override void Enter()
        {
            animal = GetComponent<Animal>();
            time = chargeTime;
            player = GameController.Current.player;
            animal.animalRigidbody.velocity = (player.transform.position-transform.position).normalized * speedBoost;
            if (animal.animalRigidbody.velocity.x < 0f) animal.animalSpriteRenderer.flipX = true;
            else animal.animalSpriteRenderer.flipX = false;
            animal.animalAnimator.SetBool("isIdling", false);
            animal.animalAnimator.SetBool("Attack", true);
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) > playerTriggerOffDistance || time<=0f)
            {
                animal.animalRigidbody.velocity = Vector2.zero;
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
            }
            /*else
            {
                //GetComponent<Rigidbody2D>().velocity = (player.transform.position - transform.position).normalized * speedBoost;
            }*/
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
