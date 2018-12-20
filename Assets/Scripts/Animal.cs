using UnityEngine;

public class Animal : MonoBehaviour
{
    public enum AnimalType { rabbit, elk, mammoth }

    public AnimalType animalType;
    public float minTimeToSpawn, maxTimeToSpawn, minTimeToVanish, maxTimeToVanish;
    protected float timeToSpawn, timeToVanish;

    public Collider2D walkCollider;
    public Collider2D bodyTrigger;

    protected SpriteRenderer mySprite;

    public int HP;
    public float speed;

    public void Hitted()
    {
        if (animalType == AnimalType.mammoth) GetComponent<Animator>().SetBool("Hit", false);
    }

    private void Start()
    {
        timeToSpawn = (float)GeneratedMap.pseudoRandom.NextDouble() * (maxTimeToSpawn - minTimeToSpawn) - minTimeToSpawn;
        timeToVanish = (float)GeneratedMap.pseudoRandom.NextDouble() * (maxTimeToVanish - minTimeToVanish) - minTimeToVanish;
        mySprite = GetComponent<SpriteRenderer>();
    }
    
    public void HideAnimal()
    {
        mySprite.enabled = false;
        walkCollider.enabled = false;
        bodyTrigger.enabled = false;
    }

    public void LetAnimalOut()
    {
        mySprite.enabled = true;
        walkCollider.enabled = true;
        bodyTrigger.enabled = true;
    }

    public void OnHit()
    {
        if (animalType == AnimalType.mammoth) GetComponent<Animator>().SetBool("Hit", true);
        if (--HP == 0)
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
            StateMachine.MessageDispatcher.Send(gameObject, new StateMachine.Message("DIE"));
            --GameController.GlobalCounterAnimal;
        }
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
