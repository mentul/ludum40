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
    private int demandForFood=0;

    private float deltaToMove;

    private static int rabbitScore;
    private static int elkScore;
    private static int mammothScore;

    private static int totalScore;

    public GameObject GeneratedMap;

    public static List<GameObject> animalList = new List<GameObject>();

    public static int GlobalCounerAnimal;

	// Use this for initialization
	void Start () {
        GlobalCounerAnimal = 0;
        isRunning = true;
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;

        //rabbitScore = 3;
        //elkScore = 17;
        //mammothScore = 33;

        timeCounter.DoInit();
        timeCounter.SetMaxRoundTime(maxRoundTime);
        //timeCounter.DoLine();
        CalculateDeltaMoveStone();

        GeneratedMap.GetComponent<GeneratedMap>().GenerateAnimal(40, player.gameObject.transform.position);
    }

    void CalculateDeltaMoveStone()
    {
        float width = timeCounter.GetLengthToDoStone();
        deltaToMove = width /initialRoundTime;
    }
	
	// Update is called once per frame
	void Update () {
        if (isRunning)
        {
            Debug.Log(animalList.Count);
            MessageDispatcher.Update();
            Vector4 playerPos = new Vector4(player.transform.position.x, player.transform.position.y, Camera.main.orthographicSize * 16, Camera.main.orthographicSize * 9);
            BackgroundMaterial.SetVector("_PlayerPosition", playerPos);
            BackgroundMaterial.SetFloat("_PlayerSpeed", player.speed);
            DrawingMaterial.SetVector("_PlayerPosition", playerPos);
            DrawingMaterial.SetFloat("_PlayerSpeed", player.speed);

            //odliczanie czasu
            timeCounter.TranformStone(deltaToMove * Time.deltaTime);
            roundTime -= Time.deltaTime;

            if(totalScore >= demandForFood)
            {
                Camera.main.transform.Find("Canvas").Find("CaveButton").gameObject.SetActive(true);
            }
            else
            {
                Camera.main.transform.Find("Canvas").Find("CaveButton").gameObject.SetActive(false);
            }

            if (roundTime < 0)
            {
                roundTime += initialRoundTime;
               
                scoreController.ShowScore();
                timeCounter.SetPositionStartStone();

            }
#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 4f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2f;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 1f;
            }

#endif

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

    public static void setScore(int i, int j, int k)
    {
        rabbitScore += i;
        elkScore += j;
        mammothScore += k;
        ResetTotalScore();
    }

    private static void ResetTotalScore()
    {
        totalScore = rabbitScore * 2 + elkScore * 5 + mammothScore * 10; 
    }

    public void StartNewRound()
    {
        GlobalCounerAnimal = 0;
        isRunning = true;
        //GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        //scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;

        //rabbitScore = 3;
        //elkScore = 17;
        //mammothScore = 33;

        timeCounter.DoInit();
        //timeCounter.SetMaxRoundTime(maxRoundTime);
        //timeCounter.DoLine();
        //CalculateDeltaMoveStone();
        timeCounter.SetPositionStartStone();
        //GeneratedMap.GetComponent<GeneratedMap>().GenerateAnimal(40);
    }

}
