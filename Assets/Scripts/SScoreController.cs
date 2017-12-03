using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SScoreController : MonoBehaviour {

    private Transform scoreCanvas;
	// Use this for initialization
	void Start () {
        HideScore();
        scoreCanvas = Camera.main.transform.Find("ScoreCanvas");
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.isRunning)
        {

        }
    }

    public void ShowScore()
    {
        scoreCanvas.gameObject.SetActive(true);
        Time.timeScale = 0;
        GameController.isRunning = false;

        List<int> temp = GetComponent<GameController>().getScore();
        int RabbitScore = temp[0];
        int ElkScore = temp[1];
        int MammothScore = temp[2];
        
        RabbitScore = Mathf.Clamp(RabbitScore, 0, 30);
        ElkScore = Mathf.Clamp(ElkScore, 0, 30);
        MammothScore = Mathf.Clamp(MammothScore, 0, 30);

        int ilePiatek = 0, reszta = 0;

        //KROLIKI KrolikScore

        ilePiatek = RabbitScore / 5;
        reszta = RabbitScore - (5 * ilePiatek);

        for(int i=0;i<6;i++)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("KrolikScore").GetChild(i).gameObject.SetActive(false);
            for(int j=0;j<5;j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("KrolikScore").GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < ilePiatek; i++)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("KrolikScore").GetChild(i).gameObject.SetActive(true);
            for (int j = 0; j < 5; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("KrolikScore").GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }

        if(ilePiatek<6 && reszta!=0)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("KrolikScore").GetChild(ilePiatek).gameObject.SetActive(true);
            for (int j = 0; j < reszta; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("KrolikScore").GetChild(ilePiatek).GetChild(j).gameObject.SetActive(true);
            }
        }

        //EŁKI JelenScore

        ilePiatek = ElkScore / 5;
        reszta = ElkScore - (5 * ilePiatek);

        for (int i = 0; i < 6; i++)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("JelenScore").GetChild(i).gameObject.SetActive(false);
            for (int j = 0; j < 5; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("JelenScore").GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < ilePiatek; i++)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("JelenScore").GetChild(i).gameObject.SetActive(true);
            for (int j = 0; j < 5; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("JelenScore").GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }

        if (ilePiatek < 6 && reszta != 0)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("JelenScore").GetChild(ilePiatek).gameObject.SetActive(true);
            for (int j = 0; j < reszta; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("JelenScore").GetChild(ilePiatek).GetChild(j).gameObject.SetActive(true);
            }
        }

        //MAMUTY MamutScore

        ilePiatek = MammothScore / 5;
        reszta = MammothScore - (5 * ilePiatek);

        for (int i = 0; i < 6; i++)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("MamutScore").GetChild(i).gameObject.SetActive(false);
            for (int j = 0; j < 5; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("MamutScore").GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < ilePiatek; i++)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("MamutScore").GetChild(i).gameObject.SetActive(true);
            for (int j = 0; j < 5; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("MamutScore").GetChild(i).GetChild(j).gameObject.SetActive(true);
            }
        }

        if (ilePiatek < 6 && reszta != 0)
        {
            scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("MamutScore").GetChild(ilePiatek).gameObject.SetActive(true);
            for (int j = 0; j < reszta; j++)
            {
                scoreCanvas.Find("Wynik").Find("PunktyZwierzat").Find("MamutScore").GetChild(ilePiatek).GetChild(j).gameObject.SetActive(true);
            }
        }

    }

    public void HideScore()
    {
        Camera.main.transform.Find("ScoreCanvas").gameObject.SetActive(false);
        Time.timeScale = 1;
        GameController.isRunning = true;
        //Debug.Log("dzialam");
        GetComponent<GameController>().StartNewRound();

    }

}
