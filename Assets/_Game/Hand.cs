using System;
using System.Collections;
using System.Collections.Generic;
using _Game;
using _Game.Card;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class Hand : MonoBehaviour
{
    public int maxAmount;
    public List<Card> cards = new List<Card>();

    private Card previuosCard;
    private float PLACEMENT_X_RANGE = 1.7f;
    public float ZSpace = -0.4f;
    public bool isFull => cards.Count >= maxAmount;
    public bool isPlayer = false;

    public Card GetRandomCard()
    {
        return cards[Random.Range(0, cards.Count)];
    }

    private void Update()
    {
        PlaceCard();
    }

    public void PickCard(Card card)
    {
        G.feel.PlayCardDraw();
        if (isFull)
        {
            return;
        }

        cards.Add(card);
        card.transform.SetParent(transform);
    }

    public void RemoveCard(Card card)
    {
        card.inHand = false;
        cards.Remove(card);
    }

    private void PlaceCard()
    {
        float spacingX = PLACEMENT_X_RANGE / cards.Count;
        float leftAnchorX = -1.9f;

        foreach (Card c in cards)
        {
            if (c != null)
            {
                int index = cards.IndexOf(c);
                float xPos = leftAnchorX + (spacingX * index);
                Debug.Log($"Place  Card {c.name}, index: {index}");
                
                float zPos = index * ZSpace;
                float normalizedX = Mathf.Abs(xPos - leftAnchorX) / PLACEMENT_X_RANGE;
                normalizedX = (normalizedX * 2f) - 1f;

                float rotation = normalizedX * -11f;
                float yPos = 1.1f + (0.1f * -Mathf.Abs(normalizedX));
                
                if (c.isHover)
                {
                    yPos += 0.4f;
                }

                c.transform.localPoggitgsition = Vector3.Lerp(c.transform.localPosition, new Vector3(xPos, yPos, zPos),  Time.deltaTime * 8f );
                c.transform.localRotation  = Quaternion.Euler(new Vector3(0, 0, rotation));
                c.inHand = isPlayer;

            }
        }
    }
    

    public IEnumerator Draw()
    {
        int needToDraw = maxAmount - cards.Count;
        for (int i = 0; i < needToDraw; i++)
        {
            yield return new WaitForSeconds(0.4f);
            this.PickCard(G.deck.DrawCard());
        }
    }

    public void UpdatePlaceAllCards()
    {
        PlaceCard();
    }

    public void SwitchCard(Card addedCard, Card removedCard)
    {
        int index = cards.IndexOf(removedCard);
        cards[index] = addedCard;
        Destroy(removedCard.gameObject);
        addedCard.transform.SetParent(transform);

        UpdatePlaceAllCards();
    }
}