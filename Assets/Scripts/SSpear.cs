using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSpear : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(GameController.isRunning)
        {

        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<IAnimal>()!=null)
        {
            collision.gameObject.GetComponent<IAnimal>().OnHit();
        }
    }

}
