using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject GeneratedMap;

	// Use this for initialization
	void Start () {
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
	}
	
	// Update is called once per frame
	void Update () {
        GeneratedMap.GetComponent<GeneratedMap>().DoUpdate();
    }
}
