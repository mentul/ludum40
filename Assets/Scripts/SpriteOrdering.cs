using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrdering : MonoBehaviour {
    SpriteRenderer renderer;
    Vector3 pivot;
    PlayerController player;
    Animal animal;
	void Start () {
        player = GetComponent<PlayerController>();
        animal = GetComponent<Animal>();
        if (player != null)
        {
            pivot = player.walkCollider.offset;
            pivot.x += transform.position.x;
            pivot.y += transform.position.y;
        }
        else if (animal != null)
        {
            pivot = animal.walkCollider.offset;
            pivot.x += transform.position.x;
            pivot.y += transform.position.y;
        }
        else
        {
            pivot = transform.position;
        }
        renderer = GetComponent<SpriteRenderer>();
        renderer.sortingOrder = (int)((1f-Camera.main.WorldToViewportPoint(pivot).y)*100f);
	}
	
	void Update () {
		if (transform.hasChanged)
        {
            if (player != null)
            {
                pivot = player.walkCollider.offset;
                pivot.x += transform.position.x;
                pivot.y += transform.position.y;
            }
            else if (animal != null)
            {
                pivot = animal.walkCollider.offset;
                pivot.x += transform.position.x;
                pivot.y += transform.position.y;
            }
            else
            {
                pivot = transform.position;
            }
            renderer.sortingOrder = (int)((1f - Camera.main.WorldToViewportPoint(pivot).y) * 100f);
        }
	}
}
