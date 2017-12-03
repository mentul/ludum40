using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeneratedMap : MonoBehaviour
{

    public List<GameObject> gameObjectTree;
    public List<GameObject> gameObjectSmallTree;
    public List<GameObject> gameObjectAnimals;
    public float PercentMammoth;
    public float PercentElk;
    public float PercentRabbit;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    private BoxCollider2D sizeMap;
    private Vector2 minVectorMap;
    private Vector2 maxVectorMap;
    private int[,] map;
    private int width, height, widthColider, heightColider;
    public int scale;
    public int smooth;

    public int maxAnimalsCount;
    private int animalsCount;

    private Vector2 positionPlayerInMap;
    public float currentPercentMammoth;
    public float currentPercentElk;
    public float currentPercentRabbit;

    // Use this for initialization
    public void DoInit()
    {
        positionPlayerInMap = Vector2.zero;
        sizeMap = this.gameObject.GetComponent<BoxCollider2D>();
        minVectorMap = sizeMap.bounds.min;
        maxVectorMap = sizeMap.bounds.max;
        animalsCount = 0;
        maxAnimalsCount = 1;

        widthColider = (int)sizeMap.size.x;
        heightColider = (int)sizeMap.size.y;
        width = widthColider / scale;
        height = heightColider / scale;

        if (smooth == 0)
            smooth = 1;
        //minVectorTree = spriteTree.bounds.min;
        //maxVectorTree = spriteTree.bounds.max;

        GenerateMap();
        AddMapSides();
    }

    // Update is called once per frame
    public void DoUpdate()
    {
        if (GameController.isRunning)
        {

        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap(1);

        for (int i = 0; i < smooth; i++)
        {
            SmoothMap();
        }
        RandomFillMap(2, 2);

        DrawTreeOnMap();

    }

    void RandomFillMap(int number, int change = 0)
    {
        int moveRange = 0;
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        if (change == 2)
        {
            randomFillPercent = 20;
        }
        else if (change == 3)
        {
            moveRange = 10;
            randomFillPercent = 5;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 0 && (x == 0 || x == width - 1 || y == 0 || y == height - 1))
                {
                    map[x, y] = 1;
                }
                else
                {
                    if (map[x, y] == 0)
                    {
                        map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? number : 0;
                    }
                }
            }
        }
    }


    void RandomFillAnimal(int number, int change = 0)
    {
        int moveRange = 0;
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        if (change == 3)
        {
            moveRange = 18;
            randomFillPercent = 5;
        }

        do
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            if (map[x, y] == 0)
            {
                if (change == 3)
                {
                    if (x > moveRange && y > moveRange && x < width - moveRange && y < height - moveRange)
                    {
                        if ((x < positionPlayerInMap.x - 5 || x > positionPlayerInMap.x + 5) && (y < positionPlayerInMap.y - 5 || y > positionPlayerInMap.y + 5))
                        {
                            map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? number : 0;
                        }
                    }
                }
                if (map[x, y] == 3)
                {
                    animalsCount++;
                    GameController.GlobalCounerAnimal++;
                }
            }
        }
        while (animalsCount < maxAnimalsCount);
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    map[x, y] = 0;

            }
        }
    }

    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    void DrawTreeOnMap()
    {
        Vector3 position = Vector3.zero;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 1)
                {
                    // position = (new Vector3(x, y, 0) * scale - new Vector3(widthColider / 2, heightColider / 2, 0));

                    position = (new Vector3(x, y, 0) * scale - new Vector3(widthColider / 2, heightColider / 2, 0) + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0f) * scale);
                    Instantiate(gameObjectTree[randomTrees()], position, gameObject.transform.rotation).transform.SetParent(this.transform);

                }
                else if (map[x, y] == 2)
                {
                    position = (new Vector3(x, y, 0) * scale - new Vector3(widthColider / 2, heightColider / 2, 0) + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0f) * scale);
                    Instantiate(gameObjectSmallTree[randomSmallTrees()], position, gameObject.transform.rotation).transform.SetParent(this.transform);

                }


            }


        }

    }
    void DrawAnimalOnMap()
    {
        Vector3 position = Vector3.zero;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 3)
                {
                    // position = (new Vector3(x, y, 0) * scale - new Vector3(widthColider / 2, heightColider / 2, 0));

                    position = (new Vector3(x, y, 0) * scale - new Vector3(widthColider / 2, heightColider / 2, 0) + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.2f, 0.2f), 0f) * scale);
                    Instantiate(gameObjectAnimals[randomAnimals()], position, gameObject.transform.rotation).transform.SetParent(this.transform);
                    GameController.animalList.Add(this.gameObject);
                }
            }
        }
    }

    public void GenerateAnimal(int count, Vector3 positionPalyer)
    {
        positionPlayerInMap.x = (int)positionPalyer.x / scale + width / 2;
        positionPlayerInMap.y = (int)positionPalyer.y / scale + height / 2;

        ResetAnimal();
        maxAnimalsCount = count;
        animalsCount = 0;
        RandomFillAnimal(3, 3);
        DrawAnimalOnMap();
    }

    private void ResetAnimal()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == 3)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    public int randomTrees()
    {
        return Random.Range(0, gameObjectTree.Count);
    }

    public int randomSmallTrees()
    {
        return Random.Range(0, gameObjectSmallTree.Count);
    }

    public int randomAnimals()
    {
        List<int> listNumber = new List<int>();
        //0 - elk
        //1 - mammoth
        //2 - rabbit
        float maxAnimal = GameController.animalList.Count;
        if (maxAnimal == 0)
        {
            listNumber.Add(0);
            listNumber.Add(1);
            listNumber.Add(2);
        }
        else
        {
            currentPercentElk = GameController.animalList.Where(x => x.gameObject.transform.GetComponent<AElk>()).ToList().Count / maxAnimal;
            currentPercentRabbit = GameController.animalList.Where(x => x.gameObject.transform.GetComponent<ARabbit>()).ToList().Count / maxAnimal;
            currentPercentMammoth = GameController.animalList.Where(x => x.gameObject.transform.GetComponent<AMammoth>()).ToList().Count / maxAnimal;

            if (currentPercentElk < PercentElk)
                listNumber.Add(0);

            if (currentPercentRabbit < PercentRabbit)
                listNumber.Add(2);

            if (currentPercentMammoth < PercentMammoth)
                listNumber.Add(1);
        }

        int random = Random.Range(0, listNumber.Count);
        Debug.Log(currentPercentElk + " " + currentPercentRabbit + " " + currentPercentMammoth + " " + random + " " + listNumber.Count + " " + maxAnimal);

        return listNumber[random];
    }

    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (map[x, y] == 1)
                    {
                        Gizmos.color = Color.black;
                    }
                    else if (map[x, y] == 2)
                    {
                        Gizmos.color = Color.red;
                    }
                    else if (map[x, y] == 3)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }

                    Vector2 pos = new Vector2(-width / 2 + x + .5f, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }

    void AddMapSides()
    {
        Vector2[] xy = new Vector2[4];
        xy[0] = new Vector2(-widthColider / 2, heightColider / 2);
        xy[1] = new Vector2(widthColider / 2, heightColider / 2);
        xy[2] = new Vector2(widthColider / 2, -heightColider / 2);
        xy[3] = new Vector2(-widthColider / 2, -heightColider / 2);
        EdgeCollider2D edges = gameObject.AddComponent<EdgeCollider2D>();
        edges.points = xy;
    }
}
