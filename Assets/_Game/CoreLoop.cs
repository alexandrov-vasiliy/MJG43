using System;
using System.Collections;
using UnityEngine;

namespace _Game
{
    public class CoreLoop : MonoBehaviour
    {
        public int cardByTurn = 2;
        public bool isPlayerTurn = false;
        public bool playerFirstTurn = true;
        private void Awake()
        {
            G.coreLoop = this;
        }

        public IEnumerator PlayerDraw()
        {
           yield return G.playerHand.Draw();
        }

        public IEnumerator EnemyDraw()
        {
            yield return G.enemyHand.Draw();
        }

        public IEnumerator PlayerTurn()
        {
            isPlayerTurn = true;
            yield return null;
        }

        public void NextTurn()
        {
            StartCoroutine(EnemyTurn());
        }

        private IEnumerator EnemyTurn()
        {
            isPlayerTurn = false;
            Debug.Log("Enemy Play");
            
            yield return new WaitForSeconds(0.5f);

            if (G.enemyHand.cards.Count == 0)
            {
                OpenCards();
                yield break;
            }
            
            var card = G.enemyHand.GetRandomCard();
            
            G.board.PlayCard(card);
            G.enemyHand.RemoveCard(card);
            
            yield return new WaitForSeconds(0.5f);
            yield return PlayerTurn();
        }

        public IEnumerator StartRound()
        {
            yield return PlayerDraw();
            yield return EnemyDraw();
            if (playerFirstTurn)
            {
                yield return PlayerTurn();
            }
            else
            {
                yield return EnemyTurn();
            }
        }

        public void OpenCards()
        {
            StartCoroutine(G.board.RevealCards());
            float value = G.board.CalculateValue(G.board.playerCards);
            G.main.scoreText.text = value.ToString();
            isPlayerTurn = false;
            
        }
    }
}