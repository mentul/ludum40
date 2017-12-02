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

	public virtual void DoInit ()
	{
		timeToSpawn = Random.Range (minTimeToSpawn, maxTimeToSpawn);
		timeToVanish = Random.Range (minTimeToVanish, maxTimeToVanish);
	}

	public virtual void DoUpdate ()
	{
		switch (animalState)
		{
			case AnimalState.hiding:
				timeToSpawn -= Time.deltaTime;
				if (timeToSpawn <= 0f)
					animalState = AnimalState.sightseeing;
				break;
			case AnimalState.sightseeing:
				timeToVanish -= Time.deltaTime;
				if (timeToVanish <= 0f)
					animalState = AnimalState.vanishing;
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

}
