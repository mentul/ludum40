using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeatScript : MonoBehaviour {

    private int maxMeat;
    private int currentMeat;
    public GameObject MeatGameObject;
    public GameObject ToMuchMeat;

    private List<GameObject> meatGameObjcetList = new List<GameObject>();


	// Use this for initialization
	public void DoInit (int maxMeat)
    {
        foreach (var item in meatGameObjcetList)
        {
            Destroy(item.gameObject);
        }
        meatGameObjcetList.Clear();
        currentMeat = 0;
        this.maxMeat = maxMeat;
        ToMuchMeat.gameObject.SetActive(false);
        DrawMeat();
    }

    void DrawMeat()
    {
        for(int i = 0; i < maxMeat; i ++)
        {
            GameObject temp = Instantiate(MeatGameObject, Vector3.zero, gameObject.transform.rotation);
            temp.transform.SetParent(this.transform);
            meatGameObjcetList.Add(temp);
        }
    }

    public void SetMAxMeat(int maxMeat)
    {
        this.maxMeat = maxMeat;
    }

    public void SetCurrentMeat(int currentMeat)
    {
        this.currentMeat = currentMeat;
        UpdateScore();
    }
	
	public void  UpdateScore () {

        if(currentMeat > maxMeat)
        {
            ToMuchMeat.gameObject.SetActive(true);
			currentMeat = maxMeat;
		}

        for (int i = 0; i < currentMeat; i++)
        {
            meatGameObjcetList[i].transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
            
    }
}
