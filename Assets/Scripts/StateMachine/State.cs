using System;
using UnityEngine;

namespace StateMachine
{
    public abstract class State : MonoBehaviour
    {
        protected StateMachine stateMachine;
        Animator _myAnimator;
        protected Animator myAnimator {
            get
            {
                if (_myAnimator == null) _myAnimator = gameObject.GetComponent<Animator>();
                return _myAnimator;
            }
        }
        Rigidbody2D _myRigidbody;
        protected Rigidbody2D myRigidbody {
            get
            {
                if(_myRigidbody==null) _myRigidbody = gameObject.GetComponent<Rigidbody2D>();
                return _myRigidbody;
            }
        }
        SpriteRenderer _mySpriteRenderer;
        protected SpriteRenderer mySpriteRenderer
        {
            get
            {
                if(_mySpriteRenderer==null) _mySpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                return _mySpriteRenderer;
            }
        }
        Animal _myAnimal;
        protected Animal myAnimal
        {
            get
            {
                if (_myAnimal == null) _myAnimal = gameObject.GetComponent<Animal>();
                return _myAnimal;
            }
        }
        PlayerController _player;
        protected PlayerController player
        {
            get
            {
                if(_player==null) _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                return _player;
            }
        }

        protected State GetStateOfType(Type type)
        {
            for(int i=0; i < stateMachine.states.Length; ++i)
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
