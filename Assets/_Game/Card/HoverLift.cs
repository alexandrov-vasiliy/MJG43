using System;
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
    public float liftHeight = 0.4f;
    
    [Tooltip("Длительность анимации поднятия / опускания.")]
    public float duration = 0.25f;
    
    [Tooltip("Easing-профиль DOTween (по умолчанию Ease.OutQuad).")]
    public Ease easing = Ease.OutQuad;

    // Исходная позиция карты (сохраняется при старте).
    private Vector3 _originalPosition;
    
    // Флаг, чтобы не запускать анимацию повторно, если курсор уже "внутри"
    private bool _isHovered = false;

    private Card card;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    public void SavePosition()
    {
        // Сохраняем исходную позицию локально (или глобально, зависит от потребностей)
        _originalPosition = transform.position;
    }

    private void OnMouseEnter()
    {
        if(!card.inHand) return;
        
        if (_isHovered) return;
        _isHovered = true;

        // Останавливаем все текущие твины на этом объекте, чтобы не было конфликтов
        transform.DOKill();
        
        // Вычисляем целевую позицию по оси Y
        float targetY = _originalPosition.y + liftHeight;
        Vector3 targetPos = new Vector3(_originalPosition.x, targetY, _originalPosition.z);
        
        // Запускаем анимацию поднятия
        transform.DOMove(targetPos, duration)
                 .SetEase(easing);
    }

    private void OnMouseExit()
    {
        if(!card.inHand) return;

        if (!_isHovered) return;
        _isHovered = false;

        // Останавливаем текущие твины
        transform.DOKill();
        
        // Возвращаемся к исходной позиции
        transform.DOMove(_originalPosition, duration)
                 .SetEase(easing);
    }

    // На случай, если объект деактивируется в процессе анимации
    private void OnDisable()
    {
        transform.DOKill();
        transform.position = _originalPosition;
    }
}
