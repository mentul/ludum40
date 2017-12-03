using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSpear : MonoBehaviour
{

    public Sprite secondSprite;
    public float timeToPickup = 1f;
    float time = 1f;
    public bool isActive = true;

    // Use this for initialization
    void Start()
    {
        time = timeToPickup;
        isActive = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("SpearPicking"), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRunning)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity - GetComponent<Rigidbody2D>().velocity * Time.deltaTime * 0.5f;
            if (GetComponent<Rigidbody2D>().velocity.magnitude < 1f && GetComponent<Rigidbody2D>().velocity.magnitude != 0f)
            {
                TurnOffTheSpear();
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

    public void TurnOffTheSpear()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("SpearPicking"), true);
        isActive = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //Sprite temp = GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = secondSprite;
        //secondSprite = temp;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            collision.gameObject.GetComponent<PlayerController>().PickUpSpear();
            Destroy(gameObject);
        }
    }


}
