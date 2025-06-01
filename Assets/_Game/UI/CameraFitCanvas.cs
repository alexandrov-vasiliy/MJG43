using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(RectTransform))]
public class CameraFitCanvas : MonoBehaviour
{
    [Header("Настройки камеры")]
    [Tooltip("Ссылка на камеру, под которую нужно подстроить Canvas")]
    public Camera targetCamera;

    [Tooltip("Расстояние от камеры до плоскости Canvas (в мировых единицах). Только для перспективной камеры.")]
    public float distanceFromCamera = 10f;

    // Внутренние ссылки
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogWarning("Этот скрипт рассчитан на Canvas в режиме WorldSpace. Измените Render Mode на WorldSpace.", gameObject);
        }

        if (targetCamera == null)
        {
            // Если камера явно не назначена, пытаемся взять основную
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogError("Не найдена ни одна камера для подстройки Canvas. Назначьте targetCamera вручную.", gameObject);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        // Первичная подстройка размеров
        FitToCamera();
    }

    private void OnValidate()
    {
        // При желании можно обновлять каждый кадр (например, если камера меняет положение или параметры)
        FitToCamera();
    }

    private void FitToCamera()
    {
        // 1) Вычисляем ширину и высоту видимой области камеры на заданном расстоянии
        float width, height;

        if (targetCamera.orthographic)
        {
            // Для ортографической камеры: размер плоскости не зависит от distanceFromCamera
            height = targetCamera.orthographicSize * 2f;
            width = height * targetCamera.aspect;
        }
        else
        {
            // Для перспективной камеры:
            // Высота фрустума на расстоянии D: h = 2 * D * tan(FOV/2)
            float fovRad = targetCamera.fieldOfView * Mathf.Deg2Rad;
            height = 2f * distanceFromCamera * Mathf.Tan(fovRad / 2f);
            width = height * targetCamera.aspect;
        }

        if(rectTransform == null) return;
        
        // 2) Устанавливаем размер RectTransform (sizeDelta) в мировых единицах
        // Примечание: при World Space Canvas sizeDelta напрямую задаёт ширину/высоту в мировых единицах
        rectTransform.sizeDelta = new Vector2(width, height);

        // 3) Позиционируем Canvas на нужном расстоянии перед камерой и ориентируем его «лицом» к камере
        if (!targetCamera.orthographic)
        {
            // Для перспективной камеры: сдвигаем Canvas вперёд вдоль направления взгляда
            transform.position = targetCamera.transform.position + targetCamera.transform.forward * distanceFromCamera;
            transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.transform.position);
        }
        else
        {
            // Для ортографической камеры: можно расположить Canvas там, где вам удобно.
            // Ниже пример: помещаем в центре видимой области на расстоянии = 0 (просто в позиции камеры)
            Vector3 orthoCenter = targetCamera.transform.position + targetCamera.transform.forward * distanceFromCamera;
            transform.position = orthoCenter;
            transform.rotation = targetCamera.transform.rotation;
        }
    }
}
