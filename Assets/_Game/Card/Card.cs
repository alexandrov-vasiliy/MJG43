using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace _Game.Card
{
    public enum CardType
    {
        DEFAULT,
        NEGATIVE
    }
    
    [RequireComponent(typeof(BoxCollider))]
    public class Card : MonoBehaviour
    {
        public ECardSuit cardSuit;
        public int value;

        
        public float hoverOffset = 0.2f;
        public CardType cardType = CardType.DEFAULT;

        public CardTooltip tooltip;
        
        public bool inHand = false;
        //public bool isOnBoard = false;
        public bool isFaceUp = false;
        
        
        public void InitInHand()
        {
            if (TryGetComponent<HoverLift>(out var lift))
            {
                lift.SavePosition();
            }
            else
            {
                gameObject.AddComponent<HoverLift>().SavePosition();
            }
        }

        private void Awake()
        {
            GetComponent<BoxCollider>().size = new Vector3(0.666006148f, 0.974437535f, 0.0017745205f);
        }
        
        public void CardInHand() 
        {
            InitInHand();
        }

        private void OnMouseDown()
        {
            if (inHand && G.coreLoop.isPlayerTurn)
            {
                G.playerHand.RemoveCard(this);
                G.board.PlayCard(this);
                G.coreLoop.NextTurn();
            }
        }

        public void SetCard(Card reference)
        {
            value = reference.value;
            cardType = reference.cardType;
            cardSuit = reference.cardSuit;
        }
        
        
        public (ECardSuit suit, int parsedValue) ParseCardName(string cardName)
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

        public void InitInDeck()
        {
            var ( suit , parsedValue)= ParseCardName(gameObject.name);
            cardSuit = suit;
            value = parsedValue;

            tooltip = gameObject.AddComponent<CardTooltip>();
        }

        public int GetCardValue()
        {
            return cardType == CardType.NEGATIVE ? -value : value;
        }
    }

}