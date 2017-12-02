using System;

namespace StateMachine
{
    public class StateMachine
    {
        static public bool isStopped=false;
        State currentState;
        State globalState;
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

        public StateMachine(GameObject gameObject, State initialState)
            : base(gameObject)
        {
            this.initialState = initialState;
            this.initialState.StateMachine = this;
            this.globalState = new IdleGlobalState(gameObject);
            this.globalState.StateMachine = this;
            if (initialState == null) currentState = globalState;
            else currentState = initialState;
            currentState.StateMachine = this;
        }
        public StateMachine(GameObject gameObject, State initialState, State globalState)
            : base(gameObject)
        {
            this.initialState = initialState;
            this.initialState.StateMachine = this;
            this.globalState = globalState;
            this.globalState.StateMachine = this;
            if (initialState == null) currentState = globalState;
            else currentState = initialState;
            currentState.StateMachine = this;
        }

        public StateMachine(GameObject gameObject)
            : base(gameObject)
        {
            currentState = globalState;
            currentState.StateMachine = this;
        }
        public StateMachine(GameObject gameObject, StateMachine stateMachine)
            : base(gameObject, stateMachine)
        {
            object[] param = { gameObject };
            this.initialState = Activator.CreateInstance(stateMachine.initialState.GetType(), param) as State;
            this.initialState.StateMachine = this;
            this.globalState = Activator.CreateInstance(stateMachine.globalState.GetType(), param) as State;
            this.globalState.StateMachine = this;
            if (initialState == null) this.currentState = this.globalState;
            else this.currentState = this.initialState;
            this.currentState.StateMachine = this;
        }

        public override void Update(GameTime gameTime)
        {
            if (!isStopped)
            {
                if (globalState != null && globalState.GetType() != currentState.GetType()) globalState.Execute(gameObject);
                if (currentState != null) currentState.Execute(gameObject);
            }
        }

        public void ChangeState(State newState)
        {
            //if (currentState is WitchHit && newState is WitchHit) { }
            //if(currentState!=null && newState != null)
            if(currentState.GetType() == newState.GetType()) { }
            else
            {

                //System.Console.WriteLine(newState.GetType().ToString());
                currentState.Exit(gameObject);
                previousState = currentState;
                currentState = newState;
                currentState.Enter(gameObject);
            }
            //System.Console.WriteLine(gameObject.name+" changed " + previousState.GetType().Name + " to " + currentState.GetType().Name);
        }

        public bool HandleMessage(Message msg)
        {
            if (currentState.OnMessage(gameObject, msg)) return true;
            else if (globalState.OnMessage(gameObject, msg)) return true;
            return false;
        }
    }
}
