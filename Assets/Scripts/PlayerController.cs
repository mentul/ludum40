﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Assets.Scripts;

public class PlayerController : MonoBehaviour {

    static public bool canThrowSpear = false;
    public float regainControlTime = 0.1f;
    public float regianTime = 0f;

    public GameObject SpearPrefab;
    public float speed = 0.5f;
    public Collider2D walkCollider;
    public Collider2D bodyTrigger;
    //public bool throwing;

    public Vector2 leftAnalog, rightAnalog;

    public bool mouse = true;

    public bool died = false;
    public bool hasSpear;

    public Transform joystickSpriteTransform;
    Vector2 currentJoystickPosition
    {
        get
        {
            return new Vector2(joystickSpriteTransform.localPosition.x, joystickSpriteTransform.localPosition.y);
        }
    }
    Vector2 initialJoystickPosition;
    public float joystickRadius = 0.6f;

    // Use this for initialization
    void Start ()
    {
        CustomInput.multiTouchEnabled = true;
        initialJoystickPosition = currentJoystickPosition;
        Reset();
    }
	
    public void Reset()
    {
        SSpear.clearSpears = true;
        hasSpear = true;
        died = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        walkCollider.enabled = true;
        bodyTrigger.enabled = true;
        GetComponent<Animator>().SetBool("HasSpear", true);
        GetComponent<Animator>().SetBool("Idling", true);
        GetComponent<Animator>().SetBool("Reset", true);
        regianTime = regainControlTime;
    }

    Touch joystickTouch, throwTouch;
    GameObject test = null;
    Vector2 joystickArea = new Vector2(0.3f, 0.4f);
    void CheckTouch()
    {
        if (joystickTouch.phase == TouchPhase.Ended || joystickTouch.phase == TouchPhase.Canceled || joystickTouch.Equals(default(Touch)) || (joystickTouch.position.x >= joystickArea.x && joystickTouch.position.y >= joystickArea.y))
        {
            joystickSpriteTransform.localPosition = initialJoystickPosition;
            joystickTouch = default(Touch);
        }
        throwTouch = default(Touch);

        for (int i = 0; i < CustomInput.touchCount; i++)
        {
            Debug.Log(CustomInput.touchCount);
            Touch t = CustomInput.GetTouch(i);
            if (t.Equals(joystickTouch)) continue;
            Vector3 temp = Camera.main.ScreenToViewportPoint(t.position);
            if (temp.x < joystickArea.x && temp.y < joystickArea.y)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(CustomInput.mousePosition.x, CustomInput.mousePosition.y, Camera.main.nearClipPlane));
                joystickTouch = t;
                joystickSpriteTransform.position = touchPos;
            }
            else if (t.phase == TouchPhase.Began)
            {
                throwTouch = t;
            }
        }

        //Joystick
        if (Vector2.Distance(initialJoystickPosition, currentJoystickPosition) > joystickRadius)
        {
            joystickSpriteTransform.localPosition = Vector2.MoveTowards(initialJoystickPosition, currentJoystickPosition, joystickRadius);
        }

        leftAnalog = (currentJoystickPosition - initialJoystickPosition).normalized;

    }


    // Update is called once per frame
    void Update () {

        if (GameController.isRunning)
        {
            if (died) return;
            if (!PlayerController.canThrowSpear)
            {
                regianTime -= Time.deltaTime;
                if (regianTime <= 0)
                {
                    SSpear.clearSpears = false;
                    PlayerController.canThrowSpear = true;
                    regianTime = regainControlTime;
                }
            }

            CheckTouch();

            //leftAnalog = new Vector2(Input.GetAxis("HorizontalLeft"), Input.GetAxis("VerticalLeft"));
            //rightAnalog = new Vector2(Input.GetAxis("HorizontalRight"), Input.GetAxis("VerticalRight") * 4);
            //Jeżeli coś z WSAD to nadaj velocity
            if (CustomInput.GetKey(KeyCode.W) || CustomInput.GetKey(KeyCode.S) || CustomInput.GetKey(KeyCode.A) || CustomInput.GetKey(KeyCode.D))
            {
                GetComponent<Animator>().SetBool("Idling", false);
                Vector2 temp = Vector2.zero;
                if (CustomInput.GetKey(KeyCode.W))
                {
                    temp += Vector2.up;
                }
                if (CustomInput.GetKey(KeyCode.A))
                {
                    temp += Vector2.left;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                if (CustomInput.GetKey(KeyCode.S))
                {
                    temp += Vector2.down;
                }
                if (CustomInput.GetKey(KeyCode.D))
                {
                    temp += Vector2.right;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                temp.Normalize();
                GetComponent<Rigidbody2D>().velocity = temp * speed;
            }
            else if (leftAnalog != Vector2.zero)
            {
                GetComponent<Animator>().SetBool("Idling", false);
                Vector2 temp = leftAnalog;
                if (leftAnalog.x < 0)
                    GetComponent<SpriteRenderer>().flipX = true;
                else
                    GetComponent<SpriteRenderer>().flipX = false;
                temp.Normalize();
                GetComponent<Rigidbody2D>().velocity = temp * speed;
            }
            else //Inaczej wyzeruj velocity
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Animator>().SetBool("Idling", true);
            }

            bool touched = (throwTouch.phase != TouchPhase.Ended && throwTouch.phase != TouchPhase.Canceled && !throwTouch.Equals(default(Touch)));
#if UNITY_EDITOR
            touched = true;
#endif
            if (CustomInput.GetMouseButtonDown(0) && touched)
            {
                if (hasSpear && canThrowSpear)
                {
                    hasSpear = false;
                    mouse = true;
                    GetComponent<Animator>().SetBool("HasSpear", hasSpear);
                    GetComponent<Animator>().SetBool("Throw", true);
                }
                //ThrowSpear();
            }

            if (CustomInput.GetKeyDown("joystick 1 button 7"))
            {
                if (hasSpear && canThrowSpear)
                {
                    hasSpear = false;
                    mouse = false;
                    GetComponent<Animator>().SetBool("HasSpear", hasSpear);
                    GetComponent<Animator>().SetBool("Throw", true);
                }
                //ThrowSpear();
            }
            if (CustomInput.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
 
    public void Throwing()
    {
        GetComponent<Animator>().SetBool("Throw", false);
        //ThrowSpear();
    }

    public void ThrowSpear()
    {
        
        //oblicz kierunek rzutu
        Vector2 temp1 = new Vector2(transform.Find("HandPosition").position.x, transform.Find("HandPosition").position.y);
        Vector2 temp2 = Vector2.zero;
        Vector2 tempOffset = Vector2.zero;
        float angle = 0f;
        if (mouse)
        {
            Vector2 throwPosition;
            if (!throwTouch.Equals(default(Touch)))
            {
                throwPosition = throwTouch.position;
            }
            else
            {
                throwPosition = CustomInput.mousePosition;
            }
            temp2 = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(throwPosition.x, throwPosition.y, Camera.main.nearClipPlane)).x, Camera.main.ScreenToWorldPoint(new Vector3(throwPosition.x, throwPosition.y, Camera.main.nearClipPlane)).y);

            tempOffset = temp2 - temp1;

        }
        else
        {
            //temp2 = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(temp1.x+rightAnalog.x, temp1.y + rightAnalog.y, Camera.main.nearClipPlane)).x, Camera.main.ScreenToWorldPoint(new Vector3(temp1.x + rightAnalog.x, temp1.y + rightAnalog.y, Camera.main.nearClipPlane)).y);
            tempOffset = rightAnalog;

        }
        //obroc kamere do kierunku rzutu
        angle = Vector3.Angle(tempOffset.normalized, Vector3.left);
        if (tempOffset.y > 0) angle = 360f - angle;
        //stworz
        GameController.spearObject = Instantiate(SpearPrefab, transform.Find("HandPosition").position, Quaternion.Euler(0, 0, angle));
        Physics2D.IgnoreCollision(GameController.spearObject.GetComponent<Collider2D>(), this.walkCollider, true);
        Physics2D.IgnoreCollision(GameController.spearObject.GetComponent<Collider2D>(), this.bodyTrigger, true);

        //rzuc
        GameController.spearObject.GetComponent<Rigidbody2D>().velocity = tempOffset.normalized * speed * 2f;
        
    }

    public void PickUpSpear()
    {
        hasSpear = true;
        GetComponent<Animator>().SetBool("HasSpear", hasSpear);
    }

    void Die()
    {
        if (died) return;
        died = true;
        walkCollider.enabled = false;
        bodyTrigger.enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Animator>().SetBool("Dead", true);
        canThrowSpear = false;
    }

    public void DeathAnimationOff()
    {
        GetComponent<Animator>().SetBool("Dead", false);
        GetComponent<Animator>().SetBool("Reset", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (died) return;
        StateMachine.StateMachine stateMachine = collision.gameObject.GetComponent<StateMachine.StateMachine>();
        if (stateMachine != null)
        {
            if(stateMachine.CurrentState.GetType() == typeof(Mammoth_wander)|| stateMachine.CurrentState.GetType() == typeof(Mammoth_triggered))
            {
                Die();
            }
        }
    }
}
