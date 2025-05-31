using System;
using System.Collections;
using System.Collections.Generic;
using _Game;
using _Game.Card;
using DG.Tweening;
using UnityEngine;

public class Board : MonoBehaviour
{
    public float offset = 1.5f;

    public List<Card> playerCards = new List<Card>();

    public Transform playerCardsPlace;

    private void Awake()
    {
        G.board = this;
    }

    public void PlayCard(Card card)
    {
        card.inHand = false;
        var place = playerCardsPlace;

        card.transform.SetParent(place);

        float xPos = place.position.x + (playerCards.Count * offset);
        float yPos = place.position.y + (playerCards.Count * 0.001f);
        float zPos = place.position.z + (playerCards.Count * 0.001f);

        card.transform.DOMove(new Vector3(xPos, yPos, zPos), 0.2f);
        card.transform.DOLocalRotate(new Vector3(90, 0, 0), 0.2f);


        playerCards.Add(card);
    }

    public void ClearBoard()
    {
       
        foreach (var playerCard in playerCards)
        {
            Destroy(playerCard.gameObject);
        }

        playerCards.Clear();
    }

    public int CalculateValue(List<Card> cards)
    {
        int value = 0;

        foreach (var card in cards)
        {
            if (card.cardType == CardType.NEGATIVE)
            {
                value -= card.value;
            }
            else
            {
                value += card.value;
            }
        }

        return value;
    }

    public IEnumerator RevealCards()
    {
        foreach (var playerCard in playerCards)
        {
            playerCard.transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.2f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}