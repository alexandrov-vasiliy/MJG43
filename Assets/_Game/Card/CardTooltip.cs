using System;
using UnityEngine;
using UnityEngine.UI;
using _Game;
using _Game.Card;
using TMPro;

/// <summary>
/// Компонент для отображения тултипа с информацией о карте над ней (фиксированно сверху), 
/// а не рядом с курсором.
/// Требует наличия Collider и Card на том же объекте.
/// </summary>
[RequireComponent(typeof(Collider), typeof(Card))]
public class CardTooltip : MonoBehaviour
{
    [Tooltip("Вертикальный отступ (в мировых единицах) над картой, где появится тултип.")]
    public float worldHeightOffset = 0.2f;

    [Tooltip("Дополнительный сдвиг в пикселях внутри канваса (например, по Y, чтобы чуть выше).")]
    public Vector2 screenOffset = new Vector2(0f, 10f);

    // Компонент Card, чтобы брать из неё данные
    private Card _card;
    // Collider, чтобы получить верхнюю точку bounds
    private Collider _collider;
    // Инстанс тултипа
    private GameObject _tooltipInstance;
    // RectTransform тултипа
    private RectTransform _tooltipRect;
    // Text внутри тултипа
    private TMP_Text _tooltipText;

    private void Awake()
    {
        _card = GetComponent<Card>();
        _collider = GetComponent<Collider>();

    }

    private void OnMouseEnter()
    {
        // Показываем тултип только если карта в руке
        if (!_card.inHand) return;

        if (_tooltipInstance != null) return;

        // Создаём тултип внутри overlayCanvas
        _tooltipInstance = Instantiate(G.ui.tooltip, G.ui.overlayCanvas.transform);
        _tooltipRect = _tooltipInstance.GetComponent<RectTransform>();

        _tooltipText = _tooltipInstance.GetComponentInChildren<TMP_Text>();
        if (_tooltipText == null)
            Debug.LogWarning($"[{nameof(CardTooltip)}] Внутри Tooltip Prefab нет UI Text для вывода информации.");

        // Заполняем содержимое (замените на свои поля Card)
        if (_tooltipText != null)
        {
            float cardValue = _card.GetCardValue();         // Предполагаем, что есть публичное свойство CardName
            _tooltipText.text = $"{cardValue}";
        }

        // Сразу позиционируем тултип над картой
        UpdateTooltipPosition();
    }

    private void OnMouseOver()
    {
        // Обновляем позицию каждую кадр, если тултип активен (например, карта может подниматься через HoverLift)
        if (_tooltipInstance != null)
            UpdateTooltipPosition();
    }

    private void OnMouseExit()
    {

        if (_tooltipInstance != null)
        {
            Destroy(_tooltipInstance);
            _tooltipInstance = null;
            _tooltipRect = null;
            _tooltipText = null;
        }
    }

    private void OnDisable()
    {
        // Если карта деактивируется, удаляем тултип
        if (_tooltipInstance != null)
        {
            Destroy(_tooltipInstance);
            _tooltipInstance = null;
            _tooltipRect = null;
            _tooltipText = null;
        }
    }

    /// <summary>
    /// Пересчитывает и задаёт позицию тултипа в локальных координатах канваса,
    /// опираясь на верхнюю точку коллайдера карты (Bounds.max.y + offset).
    /// </summary>
    private void UpdateTooltipPosition()
    {
        // Берём верхнюю точку коллайдера
        Vector3 worldTop = new Vector3(
            _collider.bounds.center.x,
            _collider.bounds.max.y + worldHeightOffset,
            _collider.bounds.center.z
        );

        // Переводим её в экранные координаты
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldTop);

        // Если объект за камерой — не показываем (хотя знак Z > 0 обычно)
        if (screenPoint.z < 0)
        {
            _tooltipInstance.SetActive(false);
            return;
        }
        else
        {
            _tooltipInstance.SetActive(true);
        }

        // Добавляем дополнительный экранный сдвиг (чтобы чуть выше)
        Vector2 screenPosWithOffset = new Vector2(screenPoint.x + screenOffset.x, screenPoint.y + screenOffset.y);

        // Переводим из экранных координат в локальные координаты canvas
        RectTransform canvasRect = G.ui.overlayCanvas.transform as RectTransform;
        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosWithOffset,
            G.ui.overlayCanvas.worldCamera,
            out anchoredPos
        );

        // Устанавливаем позицию
        if (_tooltipRect != null)
            _tooltipRect.anchoredPosition = anchoredPos;
    }
}
