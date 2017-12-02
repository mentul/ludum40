using UnityEngine;

namespace StateMachine
{
    public class Message
    {
        private string subject;
        public string Subject
        {
            get
            {
                return subject;
            }
        }
        private GameObject from;
        public GameObject From
        {
            get
            {
                return from;
            }
        }
        public Message(string subject, GameObject from=null)
        {
            this.subject = subject;
            if (from != null)
            {
                this.from = from;
            }
        }
    }
}
