using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum AnimalType { rabbit, elk, mammoth }

    public AnimalType animalType;
    protected float timeToSpawn, timeToVanish;

    public Collider2D walkCollider;
    public Collider2D bodyTrigger;

    public SpriteRenderer animalSpriteRenderer;
    public StateMachine.StateMachine animalStateMachine;
    public Rigidbody2D animalRigidbody;
    public Animator animalAnimator;

    public int HP;
    public float speed;
    bool animalActive = false;
    public float drawDistance = 50f;

    public void Hitted()
    {
        if (animalType == AnimalType.mammoth) animalAnimator.SetBool("Hit", false);
    }

    private void Start()
    {
        DoInit();
    }

    public virtual void DoInit()
    {
        animalAnimator = GetComponent<Animator>();
        animalStateMachine = GetComponent<StateMachine.StateMachine>();
        animalSpriteRenderer = GetComponent<SpriteRenderer>();
        animalRigidbody = GetComponent<Rigidbody2D>();
        SetAnimalActive(false);
    }

    void Update()
    {
        if (HP > 0)
        {
            if (Vector3.Distance(GameController.Current.player.transform.position, transform.position) < drawDistance)
            {
                if (!animalActive)
                {
                    SetAnimalActive(true);
                }
            }
            else
            {
                if (animalActive)
                {
                    SetAnimalActive(false);
                }
            }
        }
    }

    public void SetAnimalActive(bool active = true)
    {
        animalActive = active;
        animalStateMachine.enabled = active;
        if (animalRigidbody != null) animalRigidbody.isKinematic = !active;
        animalSpriteRenderer.enabled = active;
        walkCollider.enabled = active;
        bodyTrigger.enabled = active;
    }

    public void OnHit()
    {
        HP--;
        if (HP == 0)
        {
            //Tutaj bedzie zabijanie zwierzaka
            if (animalType == AnimalType.rabbit)
            {
                GameController.setScore(1, 0, 0);
            }
            else if (animalType == AnimalType.elk)
            {
                GameController.setScore(0, 1, 0);
            }
            else if (animalType == AnimalType.mammoth)
            {
                GameController.setScore(0, 0, 1);
            }
            StateMachine.MessageDispatcher.Send(this.gameObject, new StateMachine.Message("DIE"));
            GameController.GlobalCounterAnimal--;
        }
        if (animalType == AnimalType.mammoth) animalAnimator.SetBool("Hit", true);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        SSpear spear = other.gameObject.GetComponent<SSpear>();
        if (spear != null && spear.isActive)
        {
            spear.TurnOffTheSpear();
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<SpriteRenderer>().sprite = spear.secondSprite;
            OnHit();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        SSpear spear = other.gameObject.GetComponent<SSpear>();
        if (spear != null && spear.isActive)
        {
            spear.TurnOffTheSpear();
            OnHit();
        }
    }
}
