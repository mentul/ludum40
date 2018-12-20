using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeatScript : MonoBehaviour
{
    public int maxMeat { get; private set; }
    public int currentMeat { get; private set; }
    public GameObject MeatGameObject;
    public GameObject ToMuchMeat;

    private List<GameObject> meatGameObjcetList = new List<GameObject>();
    
    // Use this for initialization
    public void DoInit(int maxMeat)
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
        for (int i = 0; i < maxMeat; ++i)
        {
            GameObject temp = Instantiate(MeatGameObject, Vector3.zero, gameObject.transform.rotation);
            temp.transform.SetParent(transform);
            meatGameObjcetList.Add(temp);
        }
    }

    public void SetMAxMeat(int maxMeat)
    {
        this.maxMeat = maxMeat;
    }

    public void SetCuurenMeat(int currentMeat)
    {
        this.currentMeat = currentMeat;
        DoUpdate();
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        if (currentMeat > maxMeat)
        {
            ToMuchMeat.gameObject.SetActive(true);
        }
        if (currentMeat > maxMeat)
        {
            currentMeat = maxMeat;
        }
        for (int i = 0; i < currentMeat; ++i)
        {
            meatGameObjcetList[i].transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
    }
}
