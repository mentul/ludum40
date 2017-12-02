using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed = 0.5f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Jeżeli coś z WSAD to nadaj velocity
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            Vector2 temp = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                temp += Vector2.up;
            }
            if (Input.GetKey(KeyCode.A))
            {
                temp += Vector2.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                temp += Vector2.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                temp += Vector2.right;
            }
            temp.Normalize();
            GetComponent<Rigidbody2D>().velocity = temp * speed;
        }
        else //Inaczej wyzeruj velocity
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
