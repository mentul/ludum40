﻿using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    class Mammoth_idle : State
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
            animal.animalAnimator.SetBool("Attack", false);
            player = GameController.Current.player;
            animal.animalRigidbody.velocity = Vector2.zero;
            time = eatTime;
        }

        public override void Execute()
        {
            if (Vector2.Distance(transform.position, player.transform.position) < playerTriggerDistance && !player.died)
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_triggered)));
            }
            else {
                if (time > 0) time -= Time.deltaTime;
                else
                {
                    stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_wander)));
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
