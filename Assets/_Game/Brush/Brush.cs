using System;
using _Game;
using UnityEngine;

public class Brush : MonoBehaviour
{

    public bool brushActivated = false;
    private int brushCount = 1;
    public GameObject brushGO;
    public Outline outline;
    private void Awake()
    {
        G.brush = this;
    }

    private void Update()
    {
        outline.enabled = CanUse();
    }

    private void OnMouseDown()
    {
        if(CanUse() == false) return;
        
        
        brushActivated = true;
        CustomCursor.Instance.SetCursor(CustomCursor.CursorType.Brush);
    }

    public void Painted()
    {
        brushActivated = false;
        brushCount--;
        CustomCursor.Instance.SetCursor(CustomCursor.CursorType.Default);

        Destroy(brushGO);
    }

    public bool CanUse()
    {
        if(G.coreLoop.status != CoreStatus.ChooseCard ) return false;
        if(brushCount <= 0) return false;

        return true;
    }
}