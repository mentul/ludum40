using UnityEngine;

public class SSpear : MonoBehaviour
{
    static public bool clearSpears=false;
    public Sprite secondSprite;
    public float timeToPickup = 1f;
    float time = 1f;
    public bool isActive = true;
    private float flyDistance;
    private Vector2 lastPosition;
    Rigidbody2D myRigidbody;
    Collider2D myCollider;

    PlayerController player;

    // Use this for initialization
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameController.Current.player;
        flyDistance = 0f;
        lastPosition = transform.position;
        time = timeToPickup;
        isActive = true;
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("SpearPicking"), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.isRunning)
        {
            if (player.died || clearSpears)
            {
                Destroy(gameObject);
                return;
            }
            flyDistance += Vector2.Distance(transform.position, lastPosition);
            //Debug.Log(Vector2.Distance(transform.position, lastPosition));
            lastPosition = transform.position;
            myRigidbody.velocity = myRigidbody.velocity;
            if (((myRigidbody.velocity.magnitude < 1f && myRigidbody.velocity.magnitude != 0f) || flyDistance >= 15f) && !spearoff)
            {
                TurnOffTheSpear();
                time = 0f;
                spearoff = true;
            }
            if (time <= 0)
            {
                if(!ignore) IgnoreCollisions();
                time = timeToPickup;
            }
            else time -= Time.deltaTime;
        }
    }
    bool spearoff = false;
    bool ignore = false;
    void IgnoreCollisions()
    {
        Physics2D.IgnoreCollision(myCollider, player.walkCollider, false);
        Physics2D.IgnoreCollision(myCollider, player.bodyTrigger, false);
        ignore = true;
    }

    public void TurnOffTheSpear()
    {
        isActive = false;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        myRigidbody.velocity = Vector2.zero;
        myCollider.isTrigger = true;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().PickUpSpear();
            Destroy(gameObject);
        }
    }

}
