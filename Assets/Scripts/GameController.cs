using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour, IGameDataRestorer
{
    public static GameObject spearObject;
    public static List<Vector3> animalsPosition = new List<Vector3>();
    public PlayerController player;
    public Material[] materialsWithPlayerPosition;
    private SScoreController scoreController;

    public GameObject LifeUIRoot;
    public static int livesLeft;

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

        LifeUIRoot.transform.Find("kreska1").gameObject.SetActive(true);
        LifeUIRoot.transform.Find("kreska2").gameObject.SetActive(true);
        LifeUIRoot.transform.Find("kreska3").gameObject.SetActive(true);
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
        animalsPosition.Clear();
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


            meatScript.SetCuurenMeat(totalScore);
            MessageDispatcher.Update();
            Vector4 playerPos = new Vector4(player.transform.position.x, player.transform.position.y, Camera.main.orthographicSize * 16, Camera.main.orthographicSize * 9);
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
                bool touched = false;
                for (int i = 0; i < CustomInput.touchCount; i++)
                {
                    if (CustomInput.GetTouch(i).phase == TouchPhase.Began) touched = true;
                }
                if (CustomInput.GetMouseButtonDown(0) || touched)
                {
                    roundTime += initialRoundTime;
                    scoreController.ShowScore();
                    timeCounter.SetPositionStartStone();
                    player.Reset();
                }
            }
#if UNITY_EDITOR

            if (CustomInput.GetKeyDown(KeyCode.Alpha4))
            {
                Time.timeScale = 4f;
            }
            else if (CustomInput.GetKeyDown(KeyCode.Alpha3))
            {
                Time.timeScale = 3f;
            }
            else if (CustomInput.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 2f;
            }
            else if (CustomInput.GetKeyDown(KeyCode.Alpha1))
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
        SetScoreTo0();
        ResetMeatScript(population);
    }






    int storedLivesLeft, storedPopulation, storedGlobalCounterAnimal, storedTotalDays, storedMaxRoundTime, storedRabbitScore, storedElkScore, storedMammothScore, storedTotalScore;
    int storedMaxMeat, storedCurrentMeat;
    float storedRoundTime, storedDeltaToMove;
    float storedPlayerRegainTime, storedPlayerRegainControlTime;
    bool storedIsRunning, storedEndCanvasActive, storedKreska1Active, storedKreska2Active, storedKreska3Active;
    bool storedPlayerHasSpear, storedPlayerDied, storedPlayerWalkColliderEnabled, storedPlayerBodyTriggerEnabled;
    Vector3 storedPlayerPosition, storedStonePosition, storedSpearVelocity;
    List<Vector3> storedAnimalsPositions;
    List<State> storedAnimalsStates;
    GameObject storedSpearObject;

    public void StoreGameData()
    {
        storedLivesLeft = livesLeft;
        storedPopulation = population;
        storedGlobalCounterAnimal = GlobalCounterAnimal;
        storedTotalDays = TotalDays;
        storedMaxRoundTime = maxRoundTime;
        storedRoundTime = roundTime;
        storedDeltaToMove = deltaToMove;
        storedIsRunning = isRunning;
        storedEndCanvasActive = EndCanvas.gameObject.activeInHierarchy;
        storedKreska1Active = LifeUIRoot.transform.Find("kreska1").gameObject.activeInHierarchy;
        storedKreska2Active = LifeUIRoot.transform.Find("kreska2").gameObject.activeInHierarchy;
        storedKreska3Active = LifeUIRoot.transform.Find("kreska3").gameObject.activeInHierarchy;
        storedRabbitScore = rabbitScore;
        storedElkScore = elkScore;
        storedMammothScore = mammothScore;
        storedTotalScore = totalScore;


        storedPlayerHasSpear = player.hasSpear;
        if (storedSpearObject != null) Destroy(storedSpearObject);
        if (spearObject != null)
        {
            storedSpearObject = Instantiate(player.SpearPrefab);
            storedSpearObject.SetActive(false);
            storedSpearObject.transform.position = spearObject.transform.position;
            storedSpearObject.transform.rotation = spearObject.transform.rotation;
            storedSpearVelocity = spearObject.GetComponent<Rigidbody2D>().velocity;
            storedSpearObject.GetComponent<Rigidbody2D>().velocity = storedSpearVelocity;
            storedSpearObject.GetComponent<SpriteRenderer>().sprite = spearObject.GetComponent<SpriteRenderer>().sprite;
            storedSpearObject.GetComponent<SSpear>().isActive = spearObject.GetComponent<SSpear>().isActive;
            storedSpearObject.GetComponent<SSpear>().flyDistance = spearObject.GetComponent<SSpear>().flyDistance;
            storedSpearObject.GetComponent<SSpear>().lastPosition = spearObject.GetComponent<SSpear>().lastPosition;
            storedSpearObject.GetComponent<SSpear>().time = spearObject.GetComponent<SSpear>().time;
        }
        else
        {
            storedSpearObject = null;
        }
        storedPlayerDied = player.died;
        storedPlayerWalkColliderEnabled = player.walkCollider.enabled;
        storedPlayerBodyTriggerEnabled = player.bodyTrigger.enabled;
        storedPlayerRegainTime = player.regianTime;
        storedPlayerRegainControlTime = player.regainControlTime;
        storedPlayerPosition = player.transform.position;

        storedStonePosition = timeCounter.stoneGameObject.transform.localPosition;

        storedMaxMeat = meatScript.maxMeat;
        storedCurrentMeat = meatScript.currentMeat;

        storedAnimalsPositions = new List<Vector3>(animalsPosition);
        storedAnimalsStates = new List<State>();
        foreach (GameObject go in animalList)
        {
            StateMachine.StateMachine sm = go.GetComponent<StateMachine.StateMachine>();
            if (sm != null) storedAnimalsStates.Add(sm.CurrentState);
        }
        SaveGameData();
    }

    public void RestoreGameData()
    {
        LoadGameData();
        livesLeft = storedLivesLeft;
        population = storedPopulation;
        GlobalCounterAnimal = storedGlobalCounterAnimal;
        TotalDays = storedTotalDays;
        maxRoundTime = storedMaxRoundTime;
        roundTime = storedRoundTime;
        deltaToMove = storedDeltaToMove;
        isRunning = storedIsRunning;
        EndCanvas.gameObject.SetActive(storedEndCanvasActive);
        LifeUIRoot.transform.Find("kreska1").gameObject.SetActive(storedKreska1Active);
        LifeUIRoot.transform.Find("kreska2").gameObject.SetActive(storedKreska2Active);
        LifeUIRoot.transform.Find("kreska3").gameObject.SetActive(storedKreska3Active);
        rabbitScore = storedRabbitScore;
        elkScore = storedElkScore;
        mammothScore = storedMammothScore;
        totalScore = storedTotalScore;

        player.hasSpear = storedPlayerHasSpear;
        if (player.hasSpear)
        {
            player.GetComponent<Animator>().SetBool("HasSpear", true);
            SSpear.clearSpears = true;
            PlayerController.canThrowSpear = false;
        }
        else
        {
            player.GetComponent<Animator>().SetBool("HasSpear", false);
            if (spearObject != null) Destroy(spearObject);
            if (storedSpearObject != null)
            {
                spearObject = Instantiate(storedSpearObject);
                spearObject.SetActive(true);
                spearObject.GetComponent<Rigidbody2D>().velocity = storedSpearVelocity;
            }
        }
        player.died = storedPlayerDied;
        player.walkCollider.enabled = storedPlayerWalkColliderEnabled;
        player.bodyTrigger.enabled = storedPlayerBodyTriggerEnabled;
        player.regianTime = storedPlayerRegainTime;
        player.regainControlTime = storedPlayerRegainControlTime;
        player.transform.position = storedPlayerPosition;

        timeCounter.stoneGameObject.transform.localPosition = storedStonePosition;

        meatScript.DoInit(storedMaxMeat);
        meatScript.SetCuurenMeat(storedCurrentMeat);

        for (int i = 0; i < animalList.Count && i < storedAnimalsPositions.Count; i++)
        {
            animalList[i].transform.position = storedAnimalsPositions[i];
        }
        animalsPosition = new List<Vector3>(storedAnimalsPositions);

        for (int i = 0; i < animalList.Count && i < storedAnimalsStates.Count; i++)
        {
            StateMachine.StateMachine sm = animalList[i].GetComponent<StateMachine.StateMachine>();
            if (sm != null)
            {
                storedAnimalsStates[i] = (State)animalList[i].AddComponent(storedAnimalsStates[i].GetType());
                sm.ChangeState(storedAnimalsStates[i]);
            }
        }
    }

    string savedGameDataFile = "GameData.sav";
    public void SaveGameData()
    {
        string filepath = Application.persistentDataPath + '/' + savedGameDataFile;
        List<string> str = new List<string>();
        str.Add(storedLivesLeft + ";" + storedPopulation + ";" + storedGlobalCounterAnimal + ";" + storedTotalDays + ";" + storedMaxRoundTime + ";" + storedRabbitScore + ";" + storedElkScore + ";" + storedMammothScore + ";" + storedTotalScore + ";" + storedMaxMeat + ";" + storedCurrentMeat);
        str.Add(storedRoundTime + ";" + storedDeltaToMove + ";" + storedPlayerRegainTime + ";" + storedPlayerRegainControlTime);
        str.Add(storedIsRunning + ";" + storedEndCanvasActive + ";" + storedKreska1Active + ";" + storedKreska2Active + ";" + storedKreska3Active + ";" + storedPlayerHasSpear + ";" + storedPlayerDied + ";" + storedPlayerWalkColliderEnabled + ";" + storedPlayerBodyTriggerEnabled);
        str.Add(storedPlayerPosition + ";" + storedStonePosition + ";" + storedSpearVelocity);
        str.Add(storedAnimalsPositions.Count.ToString());
        for (int i = 0; i < storedAnimalsPositions.Count; ++i)
        {
            str.Add(storedAnimalsPositions[i].ToString());
        }
        str.Add(storedAnimalsStates.Count.ToString());
        for (int i = 0; i < storedAnimalsStates.Count; ++i)
        {
            str.Add(storedAnimalsStates[i].GetType().ToString());
        }
        if (storedSpearObject != null) str.Add(storedSpearObject.ToString());

        try
        {
            System.IO.File.WriteAllLines(filepath, str.ToArray());
        }
        catch (Exception e)
        {
            print(e.Message + "; " + e.StackTrace + "\n");
        }

    }

    public void LoadGameData()
    {
        string filepath = Application.persistentDataPath + '/' + savedGameDataFile;
        if (!System.IO.File.Exists(filepath)) return;

        string[] fileLines = System.IO.File.ReadAllLines(filepath);
        string line = fileLines[0];
        line.Trim(' ');
        string[] splittedLine = line.Split(';');
        storedLivesLeft = int.Parse(splittedLine[0]);
        storedPopulation = int.Parse(splittedLine[1]);
        storedGlobalCounterAnimal = int.Parse(splittedLine[2]);
        storedTotalDays = int.Parse(splittedLine[3]);
        storedMaxRoundTime = int.Parse(splittedLine[4]);
        storedRabbitScore = int.Parse(splittedLine[5]);
        storedElkScore = int.Parse(splittedLine[6]);
        storedMammothScore = int.Parse(splittedLine[7]);
        storedTotalScore = int.Parse(splittedLine[8]);
        storedMaxMeat = int.Parse(splittedLine[9]);
        storedCurrentMeat = int.Parse(splittedLine[10]);

        line = fileLines[1];
        line.Trim(' ');
        splittedLine = line.Split(';');
        storedRoundTime = float.Parse(splittedLine[0]);
        storedDeltaToMove = float.Parse(splittedLine[1]);
        storedPlayerRegainTime = float.Parse(splittedLine[2]);
        storedPlayerRegainControlTime = float.Parse(splittedLine[3]);

        line = fileLines[2];
        line.Trim(' ');
        splittedLine = line.Split(';');
        storedIsRunning = bool.Parse(splittedLine[0]);
        storedEndCanvasActive = bool.Parse(splittedLine[1]);
        storedKreska1Active = bool.Parse(splittedLine[2]);
        storedKreska2Active = bool.Parse(splittedLine[3]);
        storedKreska3Active = bool.Parse(splittedLine[4]);
        storedPlayerHasSpear = bool.Parse(splittedLine[5]);
        storedPlayerDied = bool.Parse(splittedLine[6]);
        storedPlayerWalkColliderEnabled = bool.Parse(splittedLine[7]);
        storedPlayerBodyTriggerEnabled = bool.Parse(splittedLine[8]);


        line = fileLines[3];
        line.Trim(' ');
        splittedLine = line.Split(';');
        storedPlayerPosition = Extensions.ParseVector3(splittedLine[0]);
        storedStonePosition = Extensions.ParseVector3(splittedLine[1]);
        storedSpearVelocity = Extensions.ParseVector3(splittedLine[2]);


        line = fileLines[4];
        line.Trim(' ');
        int storedAnimalsPositionsCount = int.Parse(line);
        storedAnimalsPositions = new List<Vector3>();
        for (int i = 0; i < storedAnimalsPositionsCount; ++i)
        {
            storedAnimalsPositions.Add(Extensions.ParseVector3(fileLines[5 + i]));
        }

        line = fileLines[5 + storedAnimalsPositionsCount];
        line.Trim(' ');
        int storedAnimalsStatesCount = int.Parse(line);
        storedAnimalsStates = new List<State>();
        for (int i = 0; i < storedAnimalsStatesCount; ++i)
        {
            Type type = Type.GetType(fileLines[6 + storedAnimalsPositionsCount + i].Trim(' '));
            if (type == null) continue;
            storedAnimalsStates.Add((State)Activator.CreateInstance(type));
        }
    }

}

