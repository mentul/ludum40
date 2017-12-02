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
}
