using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOrdering : MonoBehaviour {
    SpriteRenderer renderer;
    Collider collider;
	void Start () {
        collider = GetComponent<Collider>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.sortingOrder = (int)((1f-Camera.main.WorldToViewportPoint(transform.position).y)*100f);
	}
	
	void Update () {
		if (transform.hasChanged)
        {
            renderer.sortingOrder = (int)((1f - Camera.main.WorldToViewportPoint(transform.position).y) * 100f);
        }
	}
}
