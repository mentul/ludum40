using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ButtonScript : MonoBehaviour
{

    public Sprite idle, hover, dead;
    public SpriteRenderer renderer;
    bool over = false;
    
    public void OnEnable()
    {
        if (GameController.livesLeft <= 0)
        {
            renderer.sprite = dead;
        }
        else
        {
            renderer.sprite = idle;
        }
    }

    public void OnMouseEnter()
    {
        if (GameController.livesLeft > 0)
        {
            renderer.sprite = hover;
        }
    }
    public void OnMouseExit()
    {
        if (GameController.livesLeft > 0)
        {
            renderer.sprite = idle;
        }
    }
    
}
