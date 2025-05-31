using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Outline))]
public class OutlineHIghtlihter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;

    private bool isHover = false;
    void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isHover) return;

        isHover = true;

        outline.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isHover) return;

        isHover = false;

        outline.enabled = false;    
    }
}
