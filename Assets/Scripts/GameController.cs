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
    private float deltaToMove;

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
        //timeCounter.DoLine();
        CalculateDeltaMoveStone();
    }

    void CalculateDeltaMoveStone()
    {
        float width = timeCounter.GetLengthToDoStone();
        deltaToMove = width /initialRoundTime;
        Debug.Log(width + "  " + deltaToMove);

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


            if(roundTime <= initialRoundTime/2 && roundTime >= initialRoundTime / 2 - 0.01f)
            {
                UnityEditor.EditorApplication.isPaused = false;
            }
            //odliczanie czasu
            timeCounter.TranformStone(deltaToMove * Time.deltaTime);
            roundTime -= Time.deltaTime;
            Debug.Log(roundTime);
            
            if (roundTime < 0)
            {
                roundTime += initialRoundTime;
               
                scoreController.ShowScore();
                timeCounter.SetPositionStartStone();

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
