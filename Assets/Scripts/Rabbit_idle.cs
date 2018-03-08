using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Rabbit_idle : State
    {
        public float eatTime = 10f;
        public float time;
        public float playerTriggerDistance = 3f;
        PlayerController player;
        Animal animal;

        public override void Enter()
        {
            animal = GetComponent<Animal>();
            animal.animalAnimator.SetBool("isIdling", true);
            player = GameController.Current.player;
            animal.animalRigidbody.velocity = Vector2.zero;
            time = eatTime;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) < playerTriggerDistance)
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_triggered)));
            }
            else {
                if (time > 0) time -= Time.deltaTime;
                else
                {
                    stateMachine.ChangeState(GetStateOfType(typeof(Rabbit_wander)));
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
