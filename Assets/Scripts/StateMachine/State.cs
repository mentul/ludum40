using UnityEngine;

namespace StateMachine
{
    public abstract class State
    {
        protected StateMachine stateMachine;
        public State(GameObject gameObject)
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
        abstract public void Enter(GameObject gameObject);
        abstract public void Execute(GameObject gameObject);
        abstract public void Exit(GameObject gameObject);
        abstract public bool OnMessage(GameObject gameObject, Message msg);
    }
}
