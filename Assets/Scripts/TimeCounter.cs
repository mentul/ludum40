using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour {

    int maxRoundTime;
    public GameObject lineCounter;
    public GameObject stoneGameObject;

    public void SetMaxRoundTime (int maxRoundTime)
    {
        this.maxRoundTime = maxRoundTime;
    }
    public void DoInit()
    {

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
        float x = Mathf.Lerp(stoneGameObject.transform.position.x, stoneGameObject.transform.position.x + time, 0.1f);
        //= Vector3.Lerp(stoneGameObject.transform.position, stoneGameObject.transform.position + new Vector3(time, 0, 0), 0.1f);
        stoneGameObject.transform.position = new Vector3(x, stoneGameObject.transform.position.y, stoneGameObject.transform.position.z);
        //stoneGameObject.transform.position = Vector3.Lerp(stoneGameObject.transform.position, stoneGameObject.transform.position + new Vector3(time, 0, 0), 0.1f);
        Debug.Log("aaa");
    }

   
}
