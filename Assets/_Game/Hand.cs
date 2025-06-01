using System.Collections;
using System.Collections.Generic;
using _Game;
using _Game.Card;
using DG.Tweening;
using UnityEngine;


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

    public void PickCard(Card card)
    {
        G.feel.PlayCardDraw();
        if (isFull)
        {
            return;
        }

        cards.Add(card);
        card.transform.SetParent(transform);
        UpdatePlaceAllCards();
    }

    public void RemoveCard(Card card)
    {
        card.inHand = false;
        cards.Remove(card);
        UpdatePlaceAllCards();
    }

    private void PlaceCard(Card card)
    {
        float spacingX = PLACEMENT_X_RANGE / cards.Count;
        float leftAnchorX = -1.9f;

        foreach (Card c in cards)
        {
            if (c != null)
            {
                int index = cards.IndexOf(c);
                float xPos = leftAnchorX + (spacingX * index);

                float zPos = index * ZSpace;
                float normalizedX = Mathf.Abs(xPos - leftAnchorX) / PLACEMENT_X_RANGE;
                normalizedX = (normalizedX * 2f) - 1f;

                float rotation = normalizedX * -11f;
                float yPos = 1.1f + (0.1f * -Mathf.Abs(normalizedX));

                c.transform.DOLocalRotate(new Vector3(0, 0, rotation), 0.2f);
                c.transform.DOLocalMove(new Vector3(xPos, yPos, zPos), 0.2f).OnComplete(() =>
                {
                    c.CardInHand();
                    c.inHand = isPlayer;
                });
            }
        }

        previuosCard = card;
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
        foreach (var card in cards)
        {
            PlaceCard(card);
        }
    }
}