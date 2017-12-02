using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerController player;
    public Material BackgroundMaterial, DrawingMaterial;

    public GameObject GeneratedMap;

	// Use this for initialization
	void Start () {
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
	}
	
	// Update is called once per frame
	void Update () {
        Vector4 playerPos = new Vector4(player.transform.position.x, player.transform.position.y, Camera.main.orthographicSize * 16, Camera.main.orthographicSize * 9);
        BackgroundMaterial.SetVector("_PlayerPosition", playerPos);
        BackgroundMaterial.SetFloat("_PlayerSpeed", player.speed);
        DrawingMaterial.SetVector("_PlayerPosition", playerPos);
        DrawingMaterial.SetFloat("_PlayerSpeed", player.speed);
    }
}
