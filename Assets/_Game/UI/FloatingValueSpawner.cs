using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Game
{
      public class FloatingValueSpawner : MonoBehaviour
    {
        [Header("Ссылка на Table Canvas, в котором будет рендериться Text")]
        public Canvas worldCanvas;

        [Header("Префаб с UI/Text (или TextMeshProUGUI), который отображает значение")]
        public GameObject floatingTextPrefab;

        [Header("Сколько в мировых единицах поднять над верхом коллайдера карты")]
        public float extraYOffset = 0.2f;        
        
        [Header("Мировое вертикальное смещение от центра карты, где появится текст")]
        [Tooltip("В каких единицах Unity: сколько поднять над коллайдером карты по Y")]
        public float worldYOffset = 0.3f;

        [Header("Скорость «улета» и затухания")]
        public float floatUpSpeed = 0.2f;
        public float fadeDuration = 2.8f;

        private Camera mainCamera;

        private void Awake()
        {
            // Кэшируем основную камеру (ту, которая рендерит worldCanvas)
            mainCamera = Camera.main;

            if (worldCanvas == null)
            {
                Debug.LogError($"[{nameof(FloatingValueSpawner)}] Не назначен World Canvas! Укажите в инспекторе.");
            }

            if (floatingTextPrefab == null)
            {
                Debug.LogError($"[{nameof(FloatingValueSpawner)}] Не назначен prefab для плавающего текста! Укажите в инспекторе.");
            }
        }

        /// <summary>
        /// Вызывает «инстанцирование» плавающего текста над данной картой.
        /// </summary>
        /// <param name="cardTransform">Transform самой карты</param>
        /// <param name="value">Числовое значение карты (положительное или отрицательное)</param>
        public void SpawnFloatingValue(Transform cardTransform, int value)
        {
            Collider col = cardTransform.GetComponent<Collider>();
            if (col == null)
            {
                Debug.LogWarning($"[{nameof(FloatingValueSpawner)}] У карты {cardTransform.name} нет Collider-а; используем просто position + extraYOffset.");
                // если вдруг нет коллайдера, как «запасной» вариант возьмём pivot:
                Vector3 fallbackTop = cardTransform.position + Vector3.up * extraYOffset;
                SpawnAtWorldPosition(fallbackTop, value);
                return;
            }
            Vector3 worldTop = new Vector3(
                col.bounds.center.x,
                col.bounds.max.y + extraYOffset, 
                col.bounds.center.z
            );

            
            // --- 2. Проецируем в экранное пространство ---
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldTop);
            if (screenPoint.z < 0f) 
                return; // карта «за» камерой, не рисуем

            // --- 3. Конвертируем screenPoint → локальные координаты Canvas ---
            RectTransform canvasRect = worldCanvas.transform as RectTransform;
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                new Vector2(screenPoint.x, screenPoint.y),
                worldCanvas.renderMode == RenderMode.ScreenSpaceOverlay 
                    ? null 
                    : worldCanvas.worldCamera,
                out anchoredPos
            );

            // --- 4. Инстанцируем префаб, ставим RectTransform.anchoredPosition = anchoredPos ---
            GameObject go = Instantiate(floatingTextPrefab, worldCanvas.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = anchoredPos;


            // 5) Устанавливаем текст: если value >= 0, добавляем плюс «+», иначе просто число (с минусом)
            TMP_Text uiText = go.GetComponent<TMP_Text>();
            if (uiText != null)
            {
                uiText.text = (value >= 0 ? "+" : "") + value.ToString();
                uiText.color = (value >= 0) ? Color.green : Color.red;
            }

            uiText.DOFade(0f, fadeDuration).OnComplete((() => Destroy(uiText.gameObject)));
        }

        private IEnumerator FadeOnly(GameObject go)
        {
            // Добавляем/получаем CanvasGroup, чтобы менять alpha
            CanvasGroup cg = go.GetComponent<CanvasGroup>();
            if (cg == null)
            {
                cg = go.AddComponent<CanvasGroup>();
                cg.alpha = 1f;
            }

            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                float t = elapsed / fadeDuration;
                if (cg != null)
                {
                    yield break;
                }
                cg.alpha = 1f - t;
                elapsed += Time.deltaTime;
                yield return null;
            }

            cg.alpha = 0f;
            Destroy(go);
        }
        
        private void SpawnAtWorldPosition(Vector3 worldPos, int value)
        {
            // Когда нет Collider-а, проще вынести повторяющийся код:
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(worldPos);
            if (screenPoint.z < 0f) return;

            RectTransform canvasRect = worldCanvas.transform as RectTransform;
            Vector2 anchoredPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                new Vector2(screenPoint.x, screenPoint.y),
                worldCanvas.renderMode == RenderMode.ScreenSpaceOverlay 
                    ? null 
                    : worldCanvas.worldCamera,
                out anchoredPos
            );

            GameObject go = Instantiate(floatingTextPrefab, worldCanvas.transform);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = anchoredPos;

            TMP_Text uiText = go.GetComponent<TMP_Text>();
            if (uiText != null)
            {
                uiText.text = (value >= 0 ? "+" : "") + value.ToString();
                uiText.color = (value >= 0) ? Color.green : Color.red;
            }


            StartCoroutine(FadeOnly(go));
        }
    }
}