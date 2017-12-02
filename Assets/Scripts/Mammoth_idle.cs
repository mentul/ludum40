using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Mammoth_idle : State
    {
        public float eatTime = 10f;
        public float time;
        Rigidbody2D rigidbody;

        public override void Enter()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            print(gameObject.name + " entering idle");
            time = eatTime;
        }

        public override void Execute()
        {
            if (time > 0) time -= Time.deltaTime;
            else
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_wander)));
            }
            print(gameObject.name + " executing idle");
        }

        public override void Exit()
        {
            print(gameObject.name + " exiting idle");
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            print(gameObject.name + " received " + msg.Subject);
            return false;
        }
        
    }
}
