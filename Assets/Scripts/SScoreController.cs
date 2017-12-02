using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SScoreController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        HideScore();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.isRunning)
        {

        }
    }

    public void ShowScore()
    {
        Camera.main.transform.Find("ScoreCanvas").gameObject.SetActive(true);
        Time.timeScale = 0;
        GameController.isRunning = false;
    }

    public void HideScore()
    {
        Camera.main.transform.Find("ScoreCanvas").gameObject.SetActive(false);
        Time.timeScale = 1;
        GameController.isRunning = true;

    }

}
