using UnityEngine;

namespace StateMachine
{
    //State to use like null
    public class IdleGlobalState : State
    {
        public override void Enter()
        {
            
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
