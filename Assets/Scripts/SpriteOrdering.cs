using UnityEngine;

public class SpriteOrdering : MonoBehaviour
{
    static int maxUpdateFrames=20;
    static int cuf=0;
    static int currentupdateframe
    {
        get
        {
            int tmp = cuf;
            cuf++;
            if (cuf >= maxUpdateFrames) cuf = 0;
            return tmp;
        }
    }
    SpriteRenderer renderer;
    Vector3 pivot;
    PlayerController player;
    Animal animal;
    int updateframe = 3;
    Vector2 playerOffset, animalOffset;

    void Start()
    {
        updateframe = currentupdateframe;
        renderer = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerController>();
        animal = GetComponent<Animal>();
        if (player != null)
        {
            playerOffset = player.walkCollider.offset;
            pivot = playerOffset;
            pivot.x += transform.position.x;
            pivot.y += transform.position.y;
        }
        else if (animal != null)
        {
            animalOffset = animal.walkCollider.offset;
            pivot = animalOffset;
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
                    playerOffset = player.walkCollider.offset;
                    pivot = playerOffset;
                    pivot.x += tempPos.x;
                    pivot.y += tempPos.y;
                }
                else if (animal != null)
                {
                    animalOffset = animal.walkCollider.offset;
                    pivot = animalOffset;
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
