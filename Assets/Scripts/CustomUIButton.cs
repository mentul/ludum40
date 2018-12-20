using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomUIButton : MonoBehaviour
{
    public bool interactable = true;
    public Image targetGraphic;
    public Color normalColor, highlightedColor, pressedColor, disabledColor;
    [Range(1, 5)]
    public float colorMultiplier = 1f;
    public float fadeDuration = 0.1f;

    public UnityEvent onClick;
    Rect buttonRect;
    Vector3 mousePos;

    Camera mainCamera;

    private void Start()
    {
        targetGraphic.color = interactable ? normalColor : disabledColor;
        buttonRect = (gameObject.transform as RectTransform).rect;
        float newWidth = buttonRect.width * transform.localScale.x;
        float newHeight = buttonRect.height * transform.localScale.y;
        buttonRect = new Rect(buttonRect.x + transform.position.x + (buttonRect.width * 0.5f) - (newWidth * 0.5f), buttonRect.y + transform.position.y + (buttonRect.height * 0.5f) - (newHeight * 0.35f), newWidth, newHeight);
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (interactable)
        {
            mousePos = mainCamera.ScreenToWorldPoint(CustomInput.mousePosition);
            if (buttonRect.Contains(mousePos))
            {
                if (CustomInput.GetMouseButtonDown(0))
                {
                    targetGraphic.color = pressedColor;
                    onClick.Invoke();
                }
                else
                {
                    targetGraphic.color = Color.Lerp(targetGraphic.color, highlightedColor, fadeDuration);
                }
            }
            else
            {
                targetGraphic.color = Color.Lerp(targetGraphic.color, normalColor, fadeDuration);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(buttonRect.position.x, buttonRect.position.y, 0f), new Vector3(buttonRect.position.x + buttonRect.width, buttonRect.position.y, 0f));
        Gizmos.DrawLine(new Vector3(buttonRect.position.x, buttonRect.position.y + buttonRect.height, 0f), new Vector3(buttonRect.position.x + buttonRect.width, buttonRect.position.y + buttonRect.height, 0f));
        Gizmos.DrawLine(new Vector3(buttonRect.position.x, buttonRect.position.y, 0f), new Vector3(buttonRect.position.x, buttonRect.position.y + buttonRect.height, 0f));
        Gizmos.DrawLine(new Vector3(buttonRect.position.x + buttonRect.width, buttonRect.position.y, 0f), new Vector3(buttonRect.position.x + buttonRect.width, buttonRect.position.y + buttonRect.height, 0f));

        Gizmos.DrawLine(mousePos + Vector3.up, mousePos + Vector3.down);
        Gizmos.DrawLine(mousePos + Vector3.left, mousePos + Vector3.right);
    }

}
