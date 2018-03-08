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
        Animal animal;

        public override void Enter()
        {
            animal = GetComponent<Animal>();
            animal.animalAnimator.SetBool("isIdling", false);
            animal.animalAnimator.SetBool("Attack", false);
            player = GameController.Current.player;
            direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector2.right;
            animal.animalRigidbody.velocity = direction;
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
                    animal.animalRigidbody.velocity = direction;
                    if (direction.x < 0f) animal.animalSpriteRenderer.flipX = true;
                    else animal.animalSpriteRenderer.flipX = false;
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
