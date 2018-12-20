﻿using UnityEngine;

public class TimeCounter : MonoBehaviour {
    
    public GameObject lineCounter;
    public GameObject stoneGameObject;
    private Vector3 positionStartStone;
    
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
        return gameObject.transform.GetChild(1).transform.GetComponent<RectTransform>().rect.width;
    }
    
    public void TranformStone(float time)
    {
        float x = Mathf.Lerp(stoneGameObject.transform.position.x, stoneGameObject.transform.position.x + time, 1f);
       
        stoneGameObject.transform.position = new Vector3(x, stoneGameObject.transform.position.y, stoneGameObject.transform.position.z);
    }

   
}
