using UnityEngine;

namespace StateMachine
{
    public class StateMachine : MonoBehaviour
    {
        public State[] states;
        State currentState;
        public State globalState;
        State previousState;
        public State initialState;
        public State CurrentState{
            get {
                return currentState;
            }
        }
        public State GlobalState
        {
            get
            {
                return globalState;
            }
        }
        public State PreviousState
        {
            get
            {
                return previousState;
            }
        }
        
        
        public void Start()
        {
            this.initialState.StateMachine = this;
            this.globalState.StateMachine = this;
            if (initialState == null) currentState = globalState;
            else currentState = initialState;
            currentState.StateMachine = this;
            currentState.Enter();
        }

        public void Update()
        {
            if (GameController.isRunning)
            {
                if (globalState != null && globalState.GetType() != currentState.GetType()) globalState.Execute();
                if (currentState != null) currentState.Execute();
            }
        }

        public void ChangeState(State newState)
        {
            if(currentState.GetType() == newState.GetType()) { }
            else
            {
                currentState.Exit();
                previousState = currentState;
                currentState = newState;
                currentState.Enter();
            }
        }

        public bool HandleMessage(Message msg)
        {
            if (currentState.OnMessage(gameObject, msg)) return true;
            else if (globalState.OnMessage(gameObject, msg)) return true;
            return false;
        }
    }
}
