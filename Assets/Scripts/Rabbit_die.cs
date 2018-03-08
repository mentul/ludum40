using System;
using StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rabbit_die : State
    {
        Animal animal;
        public override void Enter()
        {
            animal = GetComponent<Animal>();
            animal.animalRigidbody.velocity = Vector2.zero;
            gameObject.RemoveComponentIncludingChildren<Collider>();
            gameObject.RemoveComponentIncludingChildren<Rigidbody2D>();
            foreach (var item in GetComponents<CapsuleCollider2D>())
            {
                item.enabled = false;
            }
            animal.animalAnimator.SetBool("Die", true);

        }

        public override void Execute()
        {
        }

        public override void Exit()
        {
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            return true;
        }
    }
}
