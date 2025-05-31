using System;
using System.Collections.Generic;
using System.Linq;
using _Game;
using _Game.Card;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public class Hand : MonoBehaviour
{
    public int maxAmount;
    public List<Card> cards = new List<Card>();

    private Card previuosCard;
    private float PLACEMENT_X_RANGE = 1.7f;
    public float ZSpace = -0.4f;
    public bool isFull => cards.Count >= maxAmount;

    private void Awake()
    {
        G.hand = this;
    }

    public void GetCard(Card card)
    {
        if (isFull)
        {
            return;
        }
        cards.Add(card);
        card.transform.SetParent(transform);
        Debug.Log(cards.Count);
        PlaceCard(card);
    }


    private void PlaceCard(Card card)
    {
        var position = transform.position;
        float spacingX = PLACEMENT_X_RANGE / cards.Count;
        float leftAnchorX = -1.9f;
        
        foreach (Card c in cards)
        {
            if (c != null)
            {

                int index = cards.IndexOf(c);
                float xPos = leftAnchorX + (spacingX * index);
                
                float zPos = index * ZSpace;
                Debug.Log($" index: {index} zPos: {zPos} ");
                float normalizedX = Mathf.Abs(xPos - leftAnchorX) / PLACEMENT_X_RANGE;
                normalizedX = (normalizedX * 2f) - 1f;

                float rotation = normalizedX * -11f;
                float yPos = 1.1f + (0.1f * -Mathf.Abs(normalizedX));
                
                c.transform.DOLocalMove(new Vector3(xPos, yPos,  zPos), 0.4f);
                c.transform.localRotation = Quaternion.Euler(5.5f, 0, rotation);
            }
        }

        previuosCard = card;
    }

}