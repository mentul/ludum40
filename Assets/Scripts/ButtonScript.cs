using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ButtonScript : MonoBehaviour
{
    public Sprite idle, hover, dead;
    public SpriteRenderer myRenderer;
    
    public void OnEnable()
    {
        if (GameController.livesLeft <= 0)
        {
            myRenderer.sprite = dead;
        }
        else
        {
            myRenderer.sprite = idle;
        }
    }

    public void OnMouseEnter()
    {
        if (GameController.livesLeft > 0)
        {
            myRenderer.sprite = hover;
        }
    }
    public void OnMouseExit()
    {
        if (GameController.livesLeft > 0)
        {
            myRenderer.sprite = idle;
        }
    }
    
}
