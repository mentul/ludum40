using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerController player;
    public Material BackgroundMaterial, DrawingMaterial;
    private SScoreController scoreController;

    public TimeCounter timeCounter;

    public float initialRoundTime;
    private float roundTime;
    public static bool isRunning;
    public int maxRoundTime;

    private int rabbitScore;
    private int elkScore;
    private int mammothScore;

    public GameObject GeneratedMap;


	// Use this for initialization
	void Start () {
        isRunning = true;
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;

        rabbitScore = 3;
        elkScore = 17;
        mammothScore = 33;
       

        timeCounter.DoInit();
        timeCounter.SetMaxRoundTime(maxRoundTime);
        timeCounter.DoLine();
    }
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            MessageDispatcher.Update();
            Vector4 playerPos = new Vector4(player.transform.position.x, player.transform.position.y, Camera.main.orthographicSize * 16, Camera.main.orthographicSize * 9);
            BackgroundMaterial.SetVector("_PlayerPosition", playerPos);
            BackgroundMaterial.SetFloat("_PlayerSpeed", player.speed);
            DrawingMaterial.SetVector("_PlayerPosition", playerPos);
            DrawingMaterial.SetFloat("_PlayerSpeed", player.speed);

            //odliczanie czasu
            roundTime -= Time.deltaTime;
            timeCounter.TranformStone(Time.deltaTime);
            if (roundTime < 0)
            {
                roundTime += initialRoundTime;
                //Debug.Log("KOniec korku");
                scoreController.ShowScore();
            }

            timeCounter.DoUpdate();
        }
    }

    public List<int> getScore()
    {
        List<int> temp = new List<int>();
        temp.Add(rabbitScore);
        temp.Add(elkScore);
        temp.Add(mammothScore);
        return temp;
    }

}
