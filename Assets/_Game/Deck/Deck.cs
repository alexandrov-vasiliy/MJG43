using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using _Game.Card;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Deck
{
    public class Deck : MonoBehaviour
    {
        [Header("Список префабов карт")]
        [Tooltip("Перетащите сюда все префабы ваших карт")]
        public List<GameObject> cardPrefabs;

        [Header("Настройки позиции стопки")]
        [Tooltip("Точка, откуда будет начинаться стопка")]
        public Transform deckOrigin;

        [Tooltip("Смещение между соседними картами (например, Vector3(0, 0.02f, 0) для лёгкого подъёма по Y)")]
        public Vector3 cardOffset = new Vector3(0f, 0.02f, 0f);

        [Header("Дополнительно")]
        [Tooltip("Если включено, колода сгенерируется автоматически в Start()")]
        public bool shuffleOnStart = true;

        // Очередь игровых объектов-карт (GameObject), чтобы можно было вызывать DrawCard()
        private Queue<Card.Card> cardQueue = new Queue<Card.Card>();

        // Вспомогательный список, куда сохраняем перемешанные префабы до инстанциирования
        private List<GameObject> shuffledPrefabs = new List<GameObject>();

        private void Awake()
        {
            G.deck = this;
        }
    
        /// <summary>
        /// Перемешивает исходный список префабов, создаёт стопку на сцене
        /// и заполняет очередь cardQueue так, чтобы Dequeue() возвращал "верхнюю" карту.
        /// </summary>
        public void GenerateDeck()
        {
            // Проверка на то, что префабы заданы
            if (cardPrefabs == null || cardPrefabs.Count == 0)
            {
                Debug.LogWarning("DeckGenerator: список cardPrefabs пуст или не задан!");
                return;
            }

            // 1) Копируем все префабы во временный список
            shuffledPrefabs = new List<GameObject>(cardPrefabs);

            // 2) Перемешиваем shuffledPrefabs (алгоритм Фишера–Йетса)
            for (int i = shuffledPrefabs.Count - 1; i > 0; i--)
            {
                int rnd = Random.Range(0, i + 1);
                GameObject temp = shuffledPrefabs[i];
                shuffledPrefabs[i] = shuffledPrefabs[rnd];
                shuffledPrefabs[rnd] = temp;
            }

            // 3) Сначала очищаем предыдущую стопку в сцене (если она была)
            if (deckOrigin != null)
            {
                for (int i = deckOrigin.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(deckOrigin.GetChild(i).gameObject);
                }
            }
            else
            {
                // Если deckOrigin не задан, чистим дочерние объекты объекта-скрипта
                for (int i = transform.childCount - 1; i >= 0; i--)
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                }
            }

            // 4) Очищаем текущую очередь перед заполнением новой
            cardQueue.Clear();

            // 5) Будем хранить список созданных объектов, чтобы потом правильно заполнить очередь
            List<GameObject> instantiatedCards = new List<GameObject>();

            // 6) Инстанциируем карты и сразу выстраиваем стопкой
            //    Делаем это по порядку shuffledPrefabs[0] -> shuffledPrefabs[last].
            //    shuffledPrefabs[0] окажется внизу стопки, а shuffledPrefabs[last] — наверху.
            for (int i = 0; i < shuffledPrefabs.Count; i++)
            {
                GameObject prefab = shuffledPrefabs[i];
                if (prefab == null)
                    continue;

                Vector3 spawnPos;
                Quaternion spawnRot;
                if (deckOrigin != null)
                {
                    // Позиция и ротация исходя из deckOrigin
                    spawnPos = deckOrigin.position + deckOrigin.TransformVector(cardOffset * i);
                    spawnRot = deckOrigin.rotation;
                }
                else
                {
                    // Если deckOrigin не задан, используем transform этого объекта
                    spawnPos = transform.position + cardOffset * i;
                    spawnRot = transform.rotation;
                }

                // Инстанциируем карту
                GameObject cardObj = Instantiate(prefab, spawnPos, spawnRot);
                cardObj.transform.rotation = Quaternion.Euler(90, 0, 0);
                // Делаем её дочерней (для порядка в иерархии)
                if (deckOrigin != null)
                    cardObj.transform.SetParent(deckOrigin);
                else
                    cardObj.transform.SetParent(transform);

                instantiatedCards.Add(cardObj);
            }

            // 7) Теперь у нас есть список instantiatedCards, упорядоченный от "низа стопки" к "верху стопки"
            //    Чтобы очередь Dequeue() возвращала "верхнюю" карту, заполняем её в обратном порядке:
            //    сначала enqueue последний элемент (верх), потом предпоследний и т.д.
            for (int i = instantiatedCards.Count - 1; i >= 0; i--)
            {
                Card.Card card;
                if (!instantiatedCards[i].TryGetComponent<Card.Card>(out card))
                {
                    card = instantiatedCards[i].AddComponent<Card.Card>();
                }

                var ( suit , value)= ParseCardName(card.gameObject.name);
                card.cardSuit = suit;
                card.value = value;
                Debug.Log($" added card to deck: {card.gameObject.name}, suit {card.cardSuit}, value: {card.value}"); 
                cardQueue.Enqueue(card);
            }

            Debug.Log($"DeckGenerator: колода сгенерирована и выложена стопкой из {shuffledPrefabs.Count} карт.");
        }

        /// <summary>
        /// Берёт «верхнюю» карту из стопки (очереди) и возвращает ссылку на GameObject.
        /// Если очередь пуста, возвращает null.
        /// </summary>
        public Card.Card DrawCard()
        {
            if (cardQueue.Count == 0)
            {
                Debug.LogWarning("DeckGenerator: попытка взять карту, но колода пуста!");
                GenerateDeck();
            }

            // 1) Получаем верхнюю карту
            Card.Card topCard = cardQueue.Dequeue();

            return topCard;
        }

        /// <summary>
        /// Для отладки: возвращает количество карт, оставшихся в очереди (стопке).
        /// </summary>
        public int CardsRemaining()
        {
            return cardQueue.Count;
        }

        /*private void OnMouseDown()
    {
        if(G.hand.isFull) return;
        
        if (CardsRemaining() == 0)
        {
            GenerateDeck();
        }
        G.hand.GetCard(DrawCard());
    }*/

        public (ECardSuit suit, int value) ParseCardName(string cardName)
        {
            // Удаляем (Clone) из названия
            if (cardName.Contains("("))
                cardName = cardName.Substring(0, cardName.IndexOf('('));

            // Шаблон: Card_(Масть)(Номинал)
            var match = Regex.Match(cardName, @"Card_([A-Za-z]+?)(\d+|Jack|Queen|King|Ace)");
            if (match.Success)
            {
                string suitStr = match.Groups[1].Value;
                string valueStr = match.Groups[2].Value;

                if (Enum.TryParse<ECardSuit>(suitStr, true, out var suit))
                {
                    int numericValue = valueStr switch
                    {
                        "Jack" => 10,
                        "Queen" => 10,
                        "King" => 10,
                        "Ace" => 11,
                        _ => int.TryParse(valueStr, out int numVal)
                            ? numVal
                            : throw new Exception($"Не удалось преобразовать номинал: {valueStr}")
                    };

                    return (suit, numericValue);
                }
            }

            throw new Exception($"Не удалось определить масть и номинал для карты {cardName}");
        }

    }
}
