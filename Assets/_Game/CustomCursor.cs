using UnityEngine;

namespace _Game
{
 using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    // === Singleton ===
    private static CustomCursor _instance;
    public static CustomCursor Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("CustomCursor: экземпляр не найден! Добавь объект с этим скриптом в сцену.");
            return _instance;
        }
    }

    // ===== Определяем варианты курсора =====
    // Добавляй новые значения сюда, если захочешь расширить набор
    public enum CursorType
    {
        Default = 0,
        Interactable = 1,
        Brush = 2,
        // Можно добавить, например:
        // Busy = 2,
        // Attack = 3,
        // и т.д.
    }

    [Header("Cursor Textures & Hotspots")]
    [Tooltip("Массив текстур курсоров. Индексы должны соответствовать CursorType.")]
    public Texture2D[] cursorTextures;
    [Tooltip("Массив 'горячих точек' (hotspot) для каждой текстуры. " +
             "Размер массива должен совпадать с cursorTextures.")]
    public Vector2[] hotspots;

    [Space(10)]
    [Header("Cursor Mode")]
    [Tooltip("Режим установки курсора (Auto или ForceSoftware)")]
    public CursorMode cursorMode = CursorMode.Auto;

    // ===== Текущий выбранный вариант =====
    private CursorType _currentVariant = CursorType.Default;

    // ===== MonoBehaviour =====
    private void Awake()
    {
        // Реализация Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Debug.LogWarning("CustomCursor: Обнаружен второй экземпляр. Уничтожаю лишний.");
            Destroy(gameObject);
            return;
        }

        // Сразу показываем системный курсор (он будет заменён SetCursor)
        Cursor.visible = true;
    }

    private void Start()
    {
        // При старте выставляем курсор по умолчанию
        SetCursor(CursorType.Default);
    }

    // ===== Основной метод для переключения =====

    /// <summary>
    /// Меняет курсор на вариант, заданный через CursorType.
    /// </summary>
    public void SetCursor(CursorType variant)
    {
        int index = (int)variant;

        // Проверяем, что массив настроен корректно
        if (cursorTextures == null || hotspots == null)
        {
            Debug.LogError("CustomCursor: Массив cursorTextures или hotspots не задан!");
            return;
        }

        if (index < 0 || index >= cursorTextures.Length || index >= hotspots.Length)
        {
            Debug.LogError($"CustomCursor: Неверный индекс {index}. " +
                           $"Длина массивов: cursorTextures = {cursorTextures.Length}, hotspots = {hotspots.Length}.");
            return;
        }

        Texture2D tex = cursorTextures[index];
        Vector2 hs = hotspots[index];

        if (tex == null)
        {
            Debug.LogWarning($"CustomCursor: Текстура для варианта {variant} (индекс {index}) не задана!");
            return;
        }

        // Устанавливаем курсор через Unity API
        Cursor.SetCursor(tex, hs, cursorMode);
        _currentVariant = variant;
    }

    /// <summary>
    /// Возвращает текущий тип курсора в виде CursorType.
    /// </summary>
    public CursorType GetCurrentVariant()
    {
        return _currentVariant;
    }

    /// <summary>
    /// Пример вспомогательного метода для быстрого переключения «по кругу».
    /// Полезно для теста: если вариантов много, будет циклическая смена.
    /// </summary>
    public void ToggleCursor()
    {
        int next = ((int)_currentVariant + 1) % cursorTextures.Length;
        SetCursor((CursorType)next);
    }

    // ===== Статические методы для простого доступа =====

    /// <summary>
    /// Устанавливает курсор «Default» через Singleton.
    /// </summary>
    public static void UseDefault()
    {
        if (Instance != null)
            Instance.SetCursor(CursorType.Default);
    }

    /// <summary>
    /// Устанавливает курсор «Interactable» через Singleton.
    /// </summary>
    public static void UseInteractable()
    {
        if (Instance != null)
            Instance.SetCursor(CursorType.Interactable);
    }

    /// <summary>
    /// Общий метод: можно передать любой CursorType, чтобы поменять.
    /// </summary>
    public static void UseCursorType(CursorType type)
    {
        if (Instance != null)
            Instance.SetCursor(type);
    }
}

}