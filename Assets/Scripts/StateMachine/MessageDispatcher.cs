using System;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{

    static public class MessageDispatcher
    {
        private struct delayedMessage
        {
            public Message Message;
            public GameObject Target;
            public DateTime TransmitTime;
            public TimeSpan Delayed;
        }
        static private List<delayedMessage> toRemove = new List<delayedMessage>();
        static private List<delayedMessage> toAdd = new List<delayedMessage>();

        static private List<delayedMessage> delayedMessages = new List<delayedMessage>();

        static public void Send(GameObject gameObject, Message msg, int seconds = 0)
        {
            if (gameObject.GetComponent<StateMachine>() == null) return;
            if (seconds <= 0) gameObject.GetComponent<StateMachine>().HandleMessage(msg);
            else
            {
                delayedMessage message = new delayedMessage();
                message.Message = msg;
                message.Target = gameObject;
                message.TransmitTime = DateTime.Now;
                message.Delayed = new TimeSpan(0, 0, seconds);
                toAdd.Add(message);
            }
        }

        static public void Update()
        {
            foreach(delayedMessage msg in toAdd)
            {
                delayedMessages.Add(msg);
            }
            foreach(delayedMessage msg in delayedMessages)
            {
                if ((DateTime.Now - msg.TransmitTime) >= msg.Delayed)
                {
                    Send(msg.Target, msg.Message);
                    toRemove.Add(msg);
                }
            }
            if (toRemove.Count > 0)
            {
                foreach(delayedMessage msg in toRemove)
                {
                    delayedMessages.Remove(msg);
                }
                toRemove.Clear();
            }
            if (toAdd.Count > 0) toAdd.Clear();
        }
    }
}
