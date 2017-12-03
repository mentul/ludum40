using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSpear : MonoBehaviour {

    public Sprite secondSprite;
    public float timeToPickup = 1f;
    float time = 1f;

	// Use this for initialization
	void Start () {
        time = timeToPickup;
    }
	
	// Update is called once per frame
	void Update () {
        if(GameController.isRunning)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity - GetComponent<Rigidbody2D>().velocity * Time.deltaTime * 0.5f;
            if (GetComponent<Rigidbody2D>().velocity.magnitude < 1f && GetComponent<Rigidbody2D>().velocity.magnitude!=0f)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Sprite temp = GetComponent<SpriteRenderer>().sprite;
                GetComponent<SpriteRenderer>().sprite = secondSprite;
                secondSprite = temp;
                //transform.rotation = Quaternion.Euler(0f, 0f, 30f);
                GetComponent<Rigidbody2D>().freezeRotation = true;
                //GetComponent<Rigidbody2D>().
                time = 0f;
            }
            if (time <= 0)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().walkCollider, false);
                time = timeToPickup;
            }
            else time -= Time.deltaTime;
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>()!=null)
        {
            collision.gameObject.GetComponent<PlayerController>().PickUpSpear();
            Destroy(gameObject);
        }
    }


}
