using System;
using System.Collections;
using System.Collections.Generic;
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
            //throw new NotImplementedException();
        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            //throw new NotImplementedException();
            return true;
        }
    }
}
