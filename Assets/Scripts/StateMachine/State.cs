using System;
using UnityEngine;

namespace StateMachine
{
    public abstract class State : MonoBehaviour
    {
        protected StateMachine stateMachine;
        
        protected State GetStateOfType(Type type)
        {
            for(int i=0; i < stateMachine.states.Length; i++)
            {
                if (stateMachine.states[i].GetType() == type) return stateMachine.states[i];
            }
            return null;
        }
        public void Start()
        {
            stateMachine = gameObject.GetComponent<StateMachine>();
        }
        public StateMachine StateMachine
        {
            get
            {
                return stateMachine;
            }
            set
            {
                stateMachine = value;
            }
        }
        abstract public void Enter();
        abstract public void Execute();
        abstract public void Exit();
        abstract public bool OnMessage(GameObject gameObject, Message msg);
    }
}
