using StateMachine;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public PlayerController player;
    public Material[] materialsWithPlayerPosition;
    private SScoreController scoreController;
    public static GameController Current;
    Camera mainCam;
    public Camera mainCamera
    {
        get
        {
            if(mainCam==null)
                mainCam = Camera.main;
            return mainCam;
        }
    }

    public GameObject caveButton;

    public GameObject LifeUIRoot;
    public static int livesLeft;
    GameObject kreska1, kreska2, kreska3;

    public TimeCounter timeCounter;

    public float initialRoundTime;
    private float roundTime;

    static bool isRunningVariable;
    public static bool isRunning
    {
        get
        {
            return isRunningVariable;
        }
        set
        {
            isRunningVariable = value;
            if (isRunningVariable)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }
    public int maxRoundTime;

    private float deltaToMove;

    private static int rabbitScore;
    private static int elkScore;
    private static int mammothScore;

    public static int totalScore;

    static int populationVariable;
    public static int population
    {
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
    public GameObject EndCanvas;

    GameController()
    {

        Current = this;
    }

    // Use this for initialization
    void Start()
    {
        livesLeft = 3;
        EndCanvas.gameObject.SetActive(false);
        population = 5;
        GlobalCounterAnimal = 0;
        isRunning = false;
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;
        TotalDays = -1;
        
        timeCounter.DoInit();
        timeCounter.SetMaxRoundTime(maxRoundTime);

        CalculateDeltaMoveStone();

        ResetMeatScript(population);
        kreska1 = LifeUIRoot.transform.Find("kreska1").gameObject;
        kreska2 = LifeUIRoot.transform.Find("kreska2").gameObject;
        kreska3 = LifeUIRoot.transform.Find("kreska3").gameObject;
        kreska1.SetActive(true);
        kreska2.SetActive(true);
        kreska3.SetActive(true);
    }

    public void SetIsRunning(bool isRunning)
    {
        GameController.isRunning = isRunning;
    }

    public void ResetGame()
    {
        livesLeft = 3;
        EndCanvas.gameObject.SetActive(false);
        population = 5;
        GlobalCounterAnimal = 0;
        isRunning = true;
        GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;
        TotalDays = -1;


        //timeCounter.DoInit();
        timeCounter.SetMaxRoundTime(maxRoundTime);

        CalculateDeltaMoveStone();

        ResetMeatScript(population);

        kreska1.SetActive(true);
        kreska2.SetActive(true);
        kreska3.SetActive(true);
        player.Reset();
        timeCounter.SetPositionStartStone();
    }

    public void AddDay()
    {
        TotalDays++;
    }

    public void UpdateLives()
    {
        if (livesLeft < 1)
        {
            kreska1.SetActive(false);
        }
        if (livesLeft < 2)
        {
            kreska2.SetActive(false);
        }
        if (livesLeft < 3)
        {
            kreska3.SetActive(false);
        }
    }

    public void ResetMeatScript(int maxMeat)
    {
        meatScript.DoInit(maxMeat);
    }

    public void RandAnimal(int count)
    {
        foreach (GameObject item in animalList)
        {
            Destroy(item.gameObject);
        }
        animalList.Clear();
        GeneratedMap.GetComponent<GeneratedMap>().GenerateAnimal(count, player.gameObject.transform.position);
    }

    void CalculateDeltaMoveStone()
    {
        float width = timeCounter.GetLengthToDoStone();
        deltaToMove = width / initialRoundTime;
    }

    public void ShowEndScreen()
    {
        //EndEnvas.gameObject.transform.Find()
        EndCanvas.gameObject.SetActive(true);

        //TotalDays

        //
        int ilePiatek = TotalDays / 5;
        int reszta = TotalDays - (5 * ilePiatek);

        for (int i = 0; i < 24; i++)
        {
            EndCanvas.transform.Find("Image").Find("Kreseczki").GetChild(i).gameObject.SetActive(false);
            for (int j = 0; j < 5; j++)
            {
                EndCanvas.transform.Find("Image").Find("Kreseczki").GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < ilePiatek; i++)
        {
            EndCanvas.transform.Find("Image").Find("Kreseczki").GetChild(i).gameObject.SetActive(true);
            for (int j = 0; j < 5; j++)
            {
                EndCanvas.transform.Find("Image").Find("Kreseczki").GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }

        if (ilePiatek < 6 && reszta != 0)
        {
            EndCanvas.transform.Find("Image").Find("Kreseczki").GetChild(ilePiatek).gameObject.SetActive(true);
            for (int j = 0; j < reszta; j++)
            {
                EndCanvas.transform.Find("Image").Find("Kreseczki").GetChild(ilePiatek).GetChild(j).gameObject.SetActive(true);
            }
        }
        //

        isRunning = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            UpdateLives();
            
            MessageDispatcher.Update();
            Vector4 playerPos = new Vector4(player.transform.position.x, player.transform.position.y, mainCamera.orthographicSize * 16, mainCamera.orthographicSize * 9);
            for (int i = 0; i < materialsWithPlayerPosition.Length; i++)
            {
                materialsWithPlayerPosition[i].SetVector("_PlayerPosition", playerPos);
                materialsWithPlayerPosition[i].SetFloat("_PlayerSpeed", player.speed);
            }

            //odliczanie czasu
            timeCounter.TranformStone(deltaToMove * Time.deltaTime);
            roundTime -= Time.deltaTime;

            if (totalScore >= population)
            {
                caveButton.SetActive(true);
            }
            else
            {
                caveButton.SetActive(false);
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
                bool touched = false;
                for (int i=0; i<Input.touchCount; i++)
                {
                    if (Input.GetTouch(i).phase == TouchPhase.Began) touched = true;
                }
                if (Input.GetMouseButtonDown(0) || touched)
                {
                    roundTime += initialRoundTime;
                    scoreController.ShowScore();
                    timeCounter.SetPositionStartStone();
                    player.Reset();
                }
            }

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

    public static void SetScoreToZero()
    {
        rabbitScore = 0;
        elkScore = 0;
        mammothScore = 0;
        ResetTotalScore();
    }

    private static void ResetTotalScore()
    {
        totalScore = rabbitScore * 2 + elkScore * 5 + mammothScore * 10;
        Current.meatScript.SetCurrentMeat(totalScore);
    }

    public void StartNewRound(bool switchRunning = true)
    {
        PlayerController.canThrowSpear = false;
        GlobalCounterAnimal = 0;
        if (switchRunning) isRunning = true;
        //GeneratedMap.GetComponent<GeneratedMap>().DoInit();
        //scoreController = GetComponent<SScoreController>();
        roundTime = initialRoundTime;

        //rabbitScore = 3;
        //elkScore = 17;
        //mammothScore = 33;

        //timeCounter.DoInit();
        //timeCounter.SetMaxRoundTime(maxRoundTime);
        //timeCounter.DoLine();
        //CalculateDeltaMoveStone();
        timeCounter.SetPositionStartStone();
        //GeneratedMap.GetComponent<GeneratedMap>().GenerateAnimal(40);
        SetScoreToZero();
        ResetMeatScript(population);
    }

}
