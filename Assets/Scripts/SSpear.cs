using UnityEngine;

public class SSpear : MonoBehaviour
{
    static public bool clearSpears=false;
    public Sprite secondSprite;
    public float timeToPickup = 1f;
    public float time = 1f;
    public bool isActive = true;
    public float flyDistance;
    public Vector2 lastPosition;

    PlayerController player;
    public BoxCollider2D myCollider;
    public Rigidbody2D myRigidbody;
    public SpriteRenderer mySpriteRenderer;

    // Use this for initialization
    void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        player = GameController.player;
        flyDistance = 0f;
        lastPosition = transform.position;
        isActive = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("SpearPicking"), false);
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
            if (isActive)
            {
                flyDistance += Vector2.Distance(transform.position, lastPosition);

                lastPosition = transform.position;
                myRigidbody.velocity = myRigidbody.velocity;
                if ((myRigidbody.velocity.magnitude < 1f && myRigidbody.velocity.magnitude != 0f) || flyDistance >= 15f)
                {
                    TurnOffTheSpear();
                }
            }
        }
    }

    public void TurnOffTheSpear()
    {
        Physics2D.IgnoreCollision(myCollider, player.walkCollider, false);
        Physics2D.IgnoreCollision(myCollider, player.bodyTrigger, false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("SpearPicking"), true);
        isActive = false;
        myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        myRigidbody.velocity = Vector2.zero;
        myCollider.isTrigger = true;

        mySpriteRenderer.sprite = secondSprite;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.PickUpSpear();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.PickUpSpear();
            Destroy(gameObject);
        }
    }

}
