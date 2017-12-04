using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public PlayerController player;
    public Material [] materialsWithPlayerPosition;
    private SScoreController scoreController;

    public GameObject LifeUIRoot;
    public static int livesLeft = 3;

    public TimeCounter timeCounter;

    public float initialRoundTime;
    private float roundTime;
    public static bool isRunning;
    public int maxRoundTime;

    private float deltaToMove;

    private static int rabbitScore;
    private static int elkScore;
    private static int mammothScore;

    public static int totalScore;

    static int populationVariable;
	public static int population {
        get
        {
            return populationVariable;
        }
        set
        {
            if (value > 0) populationVariable = value;
            else populationVariable = 1;
        }
    }

    public GameObject GeneratedMap;

    public static List<GameObject> animalList = new List<GameObject>();

    public static int GlobalCounterAnimal;

    public MeatScript meatScript;

    private int TotalDays;
    public GameObject EndEnvas;

	// Use this for initialization
	void Start () {
        EndEnvas.gameObject.SetActive(false);
        population = 5;
        GlobalCounterAnimal = 0;
        isRunning = true;
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;
        TotalDays = -1;

        //rabbitScore = 3;
        //elkScore = 17;
        //mammothScore = 33;

        timeCounter.DoInit();
        timeCounter.SetMaxRoundTime(maxRoundTime);
        //timeCounter.DoLine();
        CalculateDeltaMoveStone();

        ResetMeatScript(population);

        LifeUIRoot.transform.Find("kreska1").gameObject.SetActive(true);
        LifeUIRoot.transform.Find("kreska2").gameObject.SetActive(true);
        LifeUIRoot.transform.Find("kreska3").gameObject.SetActive(true);
    }

    public void AddDay()
    {
        TotalDays++;
    }

    public void UpdateLives()
    {
        if (livesLeft < 1) {
            LifeUIRoot.transform.Find("kreska1").gameObject.SetActive(false);
        }
        if (livesLeft < 2)
        {
            LifeUIRoot.transform.Find("kreska2").gameObject.SetActive(false);
        }
        if (livesLeft < 3)
        {
            LifeUIRoot.transform.Find("kreska3").gameObject.SetActive(false);
        }
    }

    public void ResetMeatScript(int maxMeat)
    {
        meatScript.DoInit(maxMeat);
    }

    public void RandAnimal(int count)
    {
        foreach (var item in animalList)
        {
            Destroy(item.gameObject);
        }
        animalList.Clear();
        GeneratedMap.GetComponent<GeneratedMap>().GenerateAnimal(count, player.gameObject.transform.position);
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
            UpdateLives();

            if (livesLeft <= 0)
            {
                //EndEnvas.gameObject.transform.Find()
                EndEnvas.gameObject.SetActive(true);

                //TotalDays

                //
                int ilePiatek = TotalDays / 5;
                int reszta = TotalDays - (5 * ilePiatek);

                for (int i = 0; i < 24; i++)
                {
                    EndEnvas.transform.Find("Image").Find("Kreseczki").GetChild(i).gameObject.SetActive(false);
                    for (int j = 0; j < 5; j++)
                    {
                        EndEnvas.transform.Find("Image").Find("Kreseczki").GetChild(i).GetChild(j).gameObject.SetActive(false);
                    }
                }

                for (int i = 0; i < ilePiatek; i++)
                {
                    EndEnvas.transform.Find("Image").Find("Kreseczki").GetChild(i).gameObject.SetActive(true);
                    for (int j = 0; j < 5; j++)
                    {
                        EndEnvas.transform.Find("Image").Find("Kreseczki").GetChild(i).GetChild(j).gameObject.SetActive(true);
                    }
                }

                if (ilePiatek < 6 && reszta != 0)
                {
                    EndEnvas.transform.Find("Image").Find("Kreseczki").GetChild(ilePiatek).gameObject.SetActive(true);
                    for (int j = 0; j < reszta; j++)
                    {
                        EndEnvas.transform.Find("Image").Find("Kreseczki").GetChild(ilePiatek).GetChild(j).gameObject.SetActive(true);
                    }
                }
                //

                isRunning = false;

            }

            meatScript.SetCuurenMeat(totalScore);
            MessageDispatcher.Update();
            Vector4 playerPos = new Vector4(player.transform.position.x, player.transform.position.y, Camera.main.orthographicSize * 16, Camera.main.orthographicSize * 9);
            for(int i=0; i< materialsWithPlayerPosition.Length; i++)
            {
                materialsWithPlayerPosition[i].SetVector("_PlayerPosition", playerPos);
                materialsWithPlayerPosition[i].SetFloat("_PlayerSpeed", player.speed);
            }

            //odliczanie czasu
            timeCounter.TranformStone(deltaToMove * Time.deltaTime);
            roundTime -= Time.deltaTime;

            if(totalScore >= population)
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
                player.Reset();

            }

            if (player.died)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    roundTime += initialRoundTime;
                    scoreController.ShowScore();
                    timeCounter.SetPositionStartStone();
                    player.Reset();
                }
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

    public static void SetScoreTo0()
    {
        rabbitScore = 0;
        elkScore = 0;
        mammothScore = 0;
        ResetTotalScore();
    }

    private static void ResetTotalScore()
    {
        totalScore = rabbitScore * 2 + elkScore * 5 + mammothScore * 10; 
    }

    public void StartNewRound()
    {
        GlobalCounterAnimal = 0;
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
        SetScoreTo0();
        ResetMeatScript(population);
    }

}
