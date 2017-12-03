using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using Assets.Scripts;

public class PlayerController : MonoBehaviour {

    public GameObject SpearPrefab;
    public float speed = 0.5f;
    public Collider2D walkCollider;
    public Collider2D bodyTrigger;
    //public bool throwing;

    private bool hasSpear;
    // Use this for initialization
    void Start () {
        hasSpear = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (GameController.isRunning)
        {
            //Jeżeli coś z WSAD to nadaj velocity
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                GetComponent<Animator>().SetBool("Idling", false);
                Vector2 temp = Vector2.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    temp += Vector2.up;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    temp += Vector2.left;
                    GetComponent<SpriteRenderer>().flipX = true;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    temp += Vector2.down;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    temp += Vector2.right;
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                temp.Normalize();
                GetComponent<Rigidbody2D>().velocity = temp * speed;
            }
            else //Inaczej wyzeruj velocity
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Animator>().SetBool("Idling", true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hasSpear)
                {
                    hasSpear = false;
                    GetComponent<Animator>().SetBool("HasSpear", hasSpear);
                    GetComponent<Animator>().SetBool("Throw", true);
                }
                    //ThrowSpear();
            }

            if (Input.GetKey(KeyCode.Escape))
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
            Vector2 temp2 = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).x, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).y);

            Vector2 tempOffset = temp2 - temp1;

            //obroc kamere do kierunku rzutu
            float angle = Vector3.Angle(tempOffset.normalized, Vector3.left);
            if (tempOffset.y > 0) angle = 360f - angle;

            //stworz
            GameObject temp = Instantiate(SpearPrefab, transform.Find("HandPosition").position, Quaternion.Euler(0, 0, angle));
            Physics2D.IgnoreCollision(temp.GetComponent<Collider2D>(), this.walkCollider, true);
           
            //rzuc
            temp.GetComponent<Rigidbody2D>().velocity = tempOffset.normalized * speed * 2f;
        
    }

    public void PickUpSpear()
    {
        hasSpear = true;
        GetComponent<Animator>().SetBool("HasSpear", hasSpear);
    }

    void Die()
    {
        print("Umarłem, ała, boli.");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
