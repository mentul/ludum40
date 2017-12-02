using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject SpearPrefab;
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
        }

        if (Input.GetMouseButtonDown(0))
        {
            ThrowSpear();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
 

    void ThrowSpear()
    {
        
        //oblicz kierunek rzutu
        Vector2 temp1 = new Vector2(transform.Find("HandPosition").position.x, transform.Find("HandPosition").position.y);
        Vector2 temp2 = new Vector2(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).x, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane)).y);

        Vector2 tempOffset = temp2 - temp1;

        //obroc kamere do kierunku rzutu
        float angle = Vector3.Angle(tempOffset.normalized, Vector3.left);
        if (tempOffset.y > 0) angle = 360f - angle;

        //stworz
        GameObject temp = Instantiate(SpearPrefab, transform.Find("HandPosition").position, Quaternion.Euler(0,0,angle) );

        //rzuc
        temp.GetComponent<Rigidbody2D>().velocity = tempOffset.normalized * speed;
    }

}
