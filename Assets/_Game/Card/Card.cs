using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Card
{
    [RequireComponent(typeof(BoxCollider))]
    public class Card : MonoBehaviour
    {
        public ECardSuit cardSuit;
        public int value;

        public bool inHand = false;
        public float hoverOffset = 0.2f;

        public void Init()
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
            Init();
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
    }

}