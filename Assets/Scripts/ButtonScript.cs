using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ButtonScript : MonoBehaviour
{

    public Sprite idle, hover;
    public SpriteRenderer renderer;
    bool over = false;
    
    public void OnEnable()
    {
        renderer.sprite = idle;
    }

    public void OnMouseEnter()
    {
        renderer.sprite = hover;
    }
    public void OnMouseExit()
    {
        renderer.sprite = idle;
    }
    
}
