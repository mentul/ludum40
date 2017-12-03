using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	public enum AnimalType { rabbit, elk, mammoth }
	public enum AnimalState { hiding, sightseeing, fighting, vanishing }

	public AnimalType animalType;
	public AnimalState animalState;
	public float minTimeToSpawn, maxTimeToSpawn, minTimeToVanish,maxTimeToVanish;
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
       
        DoInit();
    }

    public virtual void DoInit ()
	{
		timeToSpawn = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		timeToVanish = Random.Range (minTimeToVanish, maxTimeToVanish);
		mySprite = GetComponent<SpriteRenderer> ();
		//HideAnimal ();
	}

	public virtual void DoUpdate ()
	{
		switch (animalState)
		{
			case AnimalState.hiding:
				timeToSpawn -= Time.deltaTime;

				if (timeToSpawn <= 0f)
				{
					//LetAnimalOut ();
					animalState = AnimalState.sightseeing;
				}
				break;
			case AnimalState.sightseeing:
				timeToVanish -= Time.deltaTime;

				if (timeToVanish <= 0f)
				{
					animalState = AnimalState.vanishing;
				}
				break;
			case AnimalState.fighting:

				break;
			case AnimalState.vanishing:
				Destroy (this.gameObject);
				break;
		}
	}

	public void OnDestroy ()
	{

	}

	public void HideAnimal ()
	{
		mySprite.enabled = false;
		walkCollider.enabled = false;
		bodyTrigger.enabled = false;
	}

	public void LetAnimalOut ()
	{
		mySprite.enabled = true;
		walkCollider.enabled = true;
		bodyTrigger.enabled = true;
	}

    public void OnHit()
    {
        if(animalType==AnimalType.mammoth) GetComponent<Animator>().SetBool("Hit", true);
        Debug.Log("Au");
        HP--;
        if(HP==0)
        {
            //Tutaj bedzie zabijanie zwierzaka
            if(animalType==AnimalType.rabbit)
            {
                GameController.setScore(1, 0, 0);
                StateMachine.MessageDispatcher.Send(this.gameObject, new StateMachine.Message("DIE"));
            }
            else if (animalType == AnimalType.elk)
            {
                GameController.setScore(0, 1, 0);
                StateMachine.MessageDispatcher.Send(this.gameObject, new StateMachine.Message("DIE"));
            }
            else if (animalType == AnimalType.mammoth)
            {
                GameController.setScore(0, 0, 1);
                StateMachine.MessageDispatcher.Send(this.gameObject, new StateMachine.Message("DIE"));
            }
            //Destroy(gameObject);
            GameController.GlobalCounterAnimal--;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        SSpear spear = other.gameObject.GetComponent<SSpear>();
        if (spear != null && spear.isActive)
        {
            other.gameObject.GetComponent<SSpear>().TurnOffTheSpear();
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.GetComponent<SpriteRenderer>().sprite = other.GetComponent<SSpear>().secondSprite;
            OnHit();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        SSpear spear = other.gameObject.GetComponent<SSpear>();
        if (spear != null && spear.isActive)
        {
            other.gameObject.GetComponent<SSpear>().TurnOffTheSpear();
            OnHit();
        }
    }
}
