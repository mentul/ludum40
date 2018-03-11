using UnityEngine;
using Assets.Scripts;

public class PlayerController : MonoBehaviour
{

    static public bool canThrowSpear = false;
    public float regainControlTime = 0.1f;
    public float regianTime = 0f;

    public GameObject SpearPrefab;
    public float speed = 0.5f;
    public Collider2D walkCollider;
    public Collider2D bodyTrigger;
    //public bool throwing;

    Animator playerAnimator;
    SpriteRenderer playerRenderer;
    Rigidbody2D playerRigidbody;

    public Vector2 leftAnalog, rightAnalog;

    public bool mouse = true;

    public bool died = false;
    private bool hasSpear;

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

    Touch joystickTouch, throwTouch;
    Vector2 throwTouchPosition;
    Vector2 joystickArea = new Vector2(0.3f, 0.4f);

    
    void Start()
    {
        Input.multiTouchEnabled = true;
        Input.simulateMouseWithTouches = false;
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        initialJoystickPosition = currentJoystickPosition;
        Reset();
    }

    public void Reset()
    {
        Input.multiTouchEnabled = true;
        Input.simulateMouseWithTouches = false;
        SSpear.clearSpears = true;
        hasSpear = true;
        died = false;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        walkCollider.enabled = true;
        bodyTrigger.enabled = true;
        playerAnimator.SetBool("HasSpear", true);
        playerAnimator.SetBool("Idling", true);
        playerAnimator.SetBool("Reset", true);
        regianTime = regainControlTime;
    }
    

    void CheckTouch()
    {
        if (joystickTouch.phase == TouchPhase.Ended || joystickTouch.phase == TouchPhase.Canceled || joystickTouch.Equals(default(Touch)) || (joystickTouch.position.x >= joystickArea.x && joystickTouch.position.y >= joystickArea.y))
        {
            joystickSpriteTransform.localPosition = initialJoystickPosition;
            joystickTouch = default(Touch);
        }

        for (int i=0; i<Input.touchCount;i++)
        {
            Debug.Log(Input.touchCount);
            Touch t = Input.GetTouch(i);
            Vector3 temp = GameController.Current.mainCamera.ScreenToViewportPoint(t.position);

            //Check if this touch was in joystick area
            if (temp.x < joystickArea.x && temp.y < joystickArea.y)
            {
                Vector3 touchPos = GameController.Current.mainCamera.ScreenToWorldPoint(new Vector3(t.position.x, t.position.y, GameController.Current.mainCamera.nearClipPlane));
                joystickTouch = t;
                joystickSpriteTransform.position = touchPos;
            }
            else if(t.phase == TouchPhase.Began && (!t.Equals(joystickTouch)) && throwTouch.Equals(default(Touch)))
            {
                throwTouch = t;
                throwTouchPosition = t.position;
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
    void Update()
    {

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
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                playerAnimator.SetBool("Idling", false);
                Vector2 temp = Vector2.zero;
                if (Input.GetKey(KeyCode.W))
                {
                    temp += Vector2.up;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    temp += Vector2.left;
                    playerRenderer.flipX = true;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    temp += Vector2.down;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    temp += Vector2.right;
                    playerRenderer.flipX = false;
                }
                temp.Normalize();
                playerRigidbody.velocity = temp * speed;
            }
            else if (leftAnalog != Vector2.zero)
            {
                playerAnimator.SetBool("Idling", false);
                Vector2 temp = leftAnalog;
                if (leftAnalog.x < 0)
                    playerRenderer.flipX = true;
                else
                    playerRenderer.flipX = false;
                temp.Normalize();
                playerRigidbody.velocity = temp * speed;
            }
            else //Inaczej wyzeruj velocity
            {
                playerRigidbody.velocity = Vector2.zero;
                playerAnimator.SetBool("Idling", true);
            }
            bool touched = (!(throwTouch.phase == TouchPhase.Ended || throwTouch.phase == TouchPhase.Canceled) && !throwTouch.Equals(default(Touch)));

            if (Input.GetMouseButtonDown(0) || touched)
            {
                if (hasSpear && canThrowSpear)
                {
                    hasSpear = false;
                    mouse = true;
                    playerAnimator.SetBool("HasSpear", hasSpear);
                    playerAnimator.SetBool("Throw", true);
                }
            }
            
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public void Throwing()
    {
        playerAnimator.SetBool("Throw", false);
        //ThrowSpear();
    }

    public void ThrowSpear()
    {

        //oblicz kierunek rzutu
        Vector2 tempHand = transform.Find("HandPosition").position;
        Vector2 temp1 = new Vector2(tempHand.x, tempHand.y);
        Vector2 temp2 = Vector2.zero;
        Vector2 tempOffset = Vector2.zero;
        float angle = 0f;


        if (mouse)
        {
            Vector2 throwPosition;
            if (!throwTouch.Equals(default(Touch)))
            {
                throwPosition = throwTouchPosition;
                throwTouch = default(Touch);
            }
            else
            {
                throwPosition = Input.mousePosition;
            }
            temp2 = new Vector2(GameController.Current.mainCamera.ScreenToWorldPoint(new Vector3(throwPosition.x, throwPosition.y, GameController.Current.mainCamera.nearClipPlane)).x, GameController.Current.mainCamera.ScreenToWorldPoint(new Vector3(throwPosition.x, throwPosition.y, GameController.Current.mainCamera.nearClipPlane)).y);

            tempOffset = temp2 - temp1;

        }
        else
        {
            tempOffset = rightAnalog;
        }
        //obroc kamere do kierunku rzutu
        angle = Vector3.Angle(tempOffset.normalized, Vector3.left);
        if (tempOffset.y > 0) angle = 360f - angle;
        //stworz
        GameObject temp = Instantiate(SpearPrefab, transform.Find("HandPosition").position, Quaternion.Euler(0, 0, angle));
        Physics2D.IgnoreCollision(temp.GetComponent<Collider2D>(), this.walkCollider, true);
        Physics2D.IgnoreCollision(temp.GetComponent<Collider2D>(), this.bodyTrigger, true);

        //rzuc
        temp.GetComponent<Rigidbody2D>().velocity = tempOffset.normalized * speed * 2f;

    }

    public void PickUpSpear()
    {
        hasSpear = true;
        playerAnimator.SetBool("HasSpear", hasSpear);
    }

    void Die()
    {
        if (died) return;
        died = true;
        walkCollider.enabled = false;
        bodyTrigger.enabled = false;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        playerAnimator.SetBool("Dead", true);
        canThrowSpear = false;
    }

    public void DeathAnimationOff()
    {
        playerAnimator.SetBool("Dead", false);
        playerAnimator.SetBool("Reset", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (died) return;
        StateMachine.StateMachine stateMachine = collision.gameObject.GetComponent<StateMachine.StateMachine>();
        if (stateMachine != null)
        {
            if (stateMachine.CurrentState.GetType() == typeof(Mammoth_wander) || stateMachine.CurrentState.GetType() == typeof(Mammoth_triggered))
            {
                Die();
            }
        }
    }
}
