﻿using StateMachine;
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

        public override void Enter()
        {
            Vector2 direction = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360)) * Vector2.right;
            GetComponent<Rigidbody2D>().velocity = direction;
            //print(gameObject.name + " entering wander");
            time = wanderTime;
        }

        public override void Execute()
        {
            if (time > 0) time -= Time.deltaTime;
            else
            {
                stateMachine.ChangeState(GetStateOfType(typeof(Mammoth_idle)));
            }
            //print(gameObject.name + " executing wander");
        }

        public override void Exit()
        {
            //print(gameObject.name + " exiting wander");
        }

        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            print(gameObject.name + " received " + msg.Subject);
            return false;
        }
        
    }
}
