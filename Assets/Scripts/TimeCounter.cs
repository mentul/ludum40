using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour {

    int maxRoundTime;
    public GameObject lineCounter;
    public GameObject stoneGameObject;
    private Vector3 positionStartStone;

    public void SetMaxRoundTime (int maxRoundTime)
    {
        this.maxRoundTime = maxRoundTime;
    }
    public void DoInit()
    {
        positionStartStone = stoneGameObject.transform.localPosition;
    }

    public void SetPositionStartStone()
    {
        stoneGameObject.transform.localPosition = positionStartStone;
    }

    public float GetLengthToDoStone()
    {
        return this.gameObject.transform.GetChild(1).transform.GetComponent<RectTransform>().rect.width;
    }

    // Use this for initialization
    public void DoLine () {

        for (int i = 0; i < maxRoundTime; i++)
        {
            Instantiate(lineCounter, Vector3.zero, lineCounter.transform.rotation).transform.SetParent(this.transform.GetChild(1));
        }

	}
	
	// Update is called once per frame
	public void DoUpdate () {
        
		
	}

    public void TranformStone(float time)
    {
        float x = Mathf.Lerp(stoneGameObject.transform.position.x, stoneGameObject.transform.position.x + time, 1f);
       
        stoneGameObject.transform.position = new Vector3(x, stoneGameObject.transform.position.y, stoneGameObject.transform.position.z);

    }

   
}
