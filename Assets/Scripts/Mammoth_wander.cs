using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Mammoth_wander : State
    {
        public float wanderTime = 20f;
        public float time;
        Rigidbody2D rigidbody;

        public override void Enter()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            GetComponent<Rigidbody2D>().velocity = Vector2.right;
            print(gameObject.name + " entering wander");
            time = wanderTime;
        }

        public override void Execute()
        {
            if (time > 0) time -= Time.deltaTime;
            else
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
            }
            print(gameObject.name + " executing wander");
        }

        public override void Exit()
        {
            print(gameObject.name + " exiting wander");
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            print(gameObject.name + " received " + msg.Subject);
            return false;
        }
        
    }
}
