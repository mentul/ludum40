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

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        flyDistance = 0f;
        lastPosition = transform.position;
        time = timeToPickup;
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
            flyDistance += Vector2.Distance(transform.position, lastPosition);

            lastPosition = transform.position;
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
            if ((GetComponent<Rigidbody2D>().velocity.magnitude < 1f && GetComponent<Rigidbody2D>().velocity.magnitude != 0f) || flyDistance >= 15f)
            {
                TurnOffTheSpear();
                time = 0f;
            }
            if (time <= 0)
            {
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.walkCollider, false);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.bodyTrigger, false);
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

        GetComponent<SpriteRenderer>().sprite = secondSprite;
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
