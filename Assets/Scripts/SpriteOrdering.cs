using UnityEngine;

public class SpriteOrdering : MonoBehaviour
{
    static int maxUpdateFrames=20;
    static int cuf=1;
    static int currentupdateframe
    {
        get
        {
            int tmp = cuf;
            cuf++;
            if (cuf > maxUpdateFrames) cuf = 1;
            return tmp;
        }
    }
    SpriteRenderer renderer;
    Vector3 pivot;
    PlayerController player;
    Animal animal;
    int updateframe = 3;
    void Start()
    {
        updateframe = currentupdateframe;
        renderer = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerController>();
        animal = GetComponent<Animal>();
        if (player != null)
        {
            pivot = player.walkCollider.offset;
            pivot.x += transform.position.x;
            pivot.y += transform.position.y;
        }
        else if (animal != null)
        {
            pivot = animal.walkCollider.offset;
            pivot.x += transform.position.x;
            pivot.y += transform.position.y;
        }
        else
        {
            pivot = transform.position;
        }
        renderer.sortingOrder = (int)((1f - GameController.Current.mainCamera.WorldToViewportPoint(pivot).y) * 100f);
    }

    void Update()
    {
        if (Time.frameCount % maxUpdateFrames == updateframe)
        {
            Vector3 tempPos = transform.position;
            if (transform.hasChanged)
            {
                if (player != null)
                {
                    pivot = player.walkCollider.offset;
                    pivot.x += tempPos.x;
                    pivot.y += tempPos.y;
                }
                else if (animal != null)
                {
                    pivot = animal.walkCollider.offset;
                    pivot.x += tempPos.x;
                    pivot.y += tempPos.y;
                }
                else
                {
                    pivot = tempPos;
                }
                renderer.sortingOrder = (int)((1f - GameController.Current.mainCamera.WorldToViewportPoint(pivot).y) * 100f);
            }
        }
    }
}
