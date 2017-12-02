using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedMap : MonoBehaviour
{

    public List<GameObject> gameObjectTree;
    public List<GameObject> gameObjectSmallTree;

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


    //private Vector2 minVectorTree;
    //private Vector2 maxVectorTree;




    // Use this for initialization
    public void DoInit()
    {
        sizeMap = this.gameObject.GetComponent<BoxCollider2D>();
        minVectorMap = sizeMap.bounds.min;
        maxVectorMap = sizeMap.bounds.max;

        widthColider = (int)sizeMap.size.x;
        heightColider = (int)sizeMap.size.y;
        width = widthColider / scale;
        height = heightColider / scale;

        if (smooth == 0)
            smooth = 1;
        //minVectorTree = spriteTree.bounds.min;
        //maxVectorTree = spriteTree.bounds.max;

        GenerateMap();
    }

    // Update is called once per frame
    public void DoUpdate()
    {

    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap(1);

        for (int i = 0; i < smooth; i++)
        {
            SmoothMap();
        }
        RandomFillMap(2,2);

        DrawTreeOnMap();

    }

    void RandomFillMap(int number, int change = 0)
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        if (change == 2)
        {
            randomFillPercent = 20;
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

    public int randomTrees()
    {
        return Random.Range(0, gameObjectTree.Count);
    }

    public int randomSmallTrees()
    {
        return Random.Range(0, gameObjectSmallTree.Count);
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
}
