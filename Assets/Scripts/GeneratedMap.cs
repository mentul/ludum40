using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedMap : MonoBehaviour
{

    public GameObject gameObjectTree;

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
        RandomFillMap();

        for (int i = 0; i < smooth; i++)
        {
            SmoothMap();
        }

        DrawTreeOnMap();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
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
                    Instantiate(gameObjectTree, position, gameObject.transform.rotation).transform.SetParent(this.transform);

                }
            }


        }

    }


    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                    Vector2 pos = new Vector2(-width / 2 + x + .5f, -height / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }
}
