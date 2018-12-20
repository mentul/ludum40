using UnityEngine;

public class SpriteOrdering : MonoBehaviour {
    SpriteRenderer myRenderer;
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
        myRenderer = GetComponent<SpriteRenderer>();
        myRenderer.sortingOrder = (int)((1f-Camera.main.WorldToViewportPoint(pivot).y)*100f);
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
            myRenderer.sortingOrder = (int)((1f - Camera.main.WorldToViewportPoint(pivot).y) * 100f);
        }
	}
}
