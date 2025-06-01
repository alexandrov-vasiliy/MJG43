using System;
using _Game;
using _Game.Card;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Скрипт поднимает карту при наведении мыши и возвращает на место при уходе.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Card))]
public class HoverLift : MonoBehaviour
{
    [Header("Настройки анимации")]
    [Tooltip("Насколько единиц карта поднимается по оси Y при наведении.")]
    private float liftHeight = 0.4f;
    
    [Tooltip("Длительность анимации поднятия / опускания.")]
    public float duration = 0.25f;
    
    [Tooltip("Easing-профиль DOTween (по умолчанию Ease.OutQuad).")]
    public Ease easing = Ease.OutQuad;

    // Исходная позиция карты (сохраняется при старте).
    private Vector3 _originalLocalPosition;
    
    // Флаг, чтобы не запускать анимацию повторно, если курсор уже "внутри"
    private bool _isHovered = false;

    private Card card;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    public void SavePosition()
    {
    }

    private void OnMouseEnter()
    {
        if(!card.inHand) return;
        
        if (_isHovered) return;
        _isHovered = true;
        
        G.feel.PlayCardHover();
        card.isHover = true;
        if(!G.brush.brushActivated)
            CustomCursor.Instance.SetCursor(CustomCursor.CursorType.Interactable);

        
       
    }

    private void OnMouseExit()
    {
        if(!card.inHand) return;

        if (!_isHovered) return;
        _isHovered = false;
        card.isHover = false;

        if(!G.brush.brushActivated)
            CustomCursor.Instance.SetCursor(CustomCursor.CursorType.Default);

    }

    // На случай, если объект деактивируется в процессе анимации
 
}
