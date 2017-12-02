using UnityEngine;

namespace StateMachine
{
    //State to use like null
    public class IdleGlobalState : State
    {
        public IdleGlobalState(GameObject gameObject) : base(gameObject) { }
        public override void Enter(GameObject gameObject)
        {
            
        }
        public override void Execute(GameObject gameObject)
        {

        }
        public override void Exit(GameObject gameObject)
        {

        }
        public override bool OnMessage(GameObject gameObject, Message msg)
        {
            return true;
        }
    }
}
