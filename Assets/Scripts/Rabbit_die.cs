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
            animal.animalAnimator.SetBool("Die", true);
            animal.animalRigidbody.velocity = Vector2.zero;
            foreach (var item in GetComponents<CapsuleCollider2D>())
            {
                item.enabled = false;
            }
            gameObject.RemoveComponentIncludingChildren<Collider>();
            gameObject.RemoveComponentIncludingChildren<Rigidbody2D>();

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
