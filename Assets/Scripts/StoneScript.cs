using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "paski")
        {
            collision.gameObject.GetComponent<Image>().color = Color.blue;
            collision.gameObject.GetComponent<Animator>().SetBool("Play", true);
        }
    }


}
