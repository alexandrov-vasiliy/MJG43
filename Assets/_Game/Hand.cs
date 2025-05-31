using System;
using System.Collections.Generic;
using System.Linq;
using _Game;
using _Game.Card;
using Unity.VisualScripting;
using UnityEngine;


public class Hand : MonoBehaviour
{
    public int maxAmount;

    public float firstX;

    public float lastX;

    public List<Card> cards = new List<Card>();

    private Card previuosCard;

    private void Awake()
    {
        G.hand = this;
    }

    public void GetCard(Card card)
    {
        cards.Add(card);
        card.transform.SetParent(transform);
        Debug.Log(cards.Count);
        PlaceCard(card);
    }

    private void PlaceCard(Card card)
    {
        var position = transform.position;

        if (cards.Count == 1)
        {
            card.transform.position = new Vector3(firstX + (lastX - firstX)/2, position.y, position.z);
            previuosCard = card;
            return;
        }

        float step = (lastX - firstX) / cards.Count;

        for (int i = 0; i < cards.Count; i++)
        {
            float x = firstX + step * i;

            cards[i].transform.position = new Vector3(x, position.y, position.z);
        }


        previuosCard = card;
    }
}