using _Game;
using UnityEngine;
using UnityEngine.Events;

public class Hover : MonoBehaviour
{
    public UnityEvent OnHover = new UnityEvent();
    public UnityEvent OnHoverEnd = new UnityEvent();
    
    private bool isHover = false;

    public void OnMouseEnter()
    {
        if(isHover) return;
        CustomCursor.Instance.SetCursor(CustomCursor.CursorType.Interactable);
        isHover = true;

        OnHover.Invoke();
    }

    public void OnMouseExit()
    {
        if(!isHover) return;
        CustomCursor.Instance.SetCursor(CustomCursor.CursorType.Default);

        isHover = false;

        OnHoverEnd.Invoke();
    }
}
