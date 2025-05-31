using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game
{
    public class CoreLoop : MonoBehaviour
    {
        public int cardByTurn = 2;
        public bool isPlayerTurn = false;
        public bool playerFirstTurn = true;

        public int playerWins = 0;
        public int dealerWins = 0;
        
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

            if (G.board.CalculateValue(G.board.playerCards) >= 30)
            {
                float r = Random.Range(0f, 1f);
                Debug.Log(r);
                if (r >= 0.6f)
                {     
                    OpenCards();
                    yield break;
                }
           
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

            playerFirstTurn = !playerFirstTurn;
        }

        public void OpenCards()
        {
            StartCoroutine(G.board.RevealCards());
            float value = G.board.CalculateValue(G.board.playerCards);
            CalculateWinner(value);
            G.main.scoreText.text = value.ToString();
            isPlayerTurn = false;
            StartCoroutine(FinishRound());
        }

        private IEnumerator FinishRound()
        {
            yield return new WaitForSeconds(2f);
            G.board.ClearBoard();
            yield return StartRound();
        }

        private void CalculateWinner(float value)
        {
            bool isPlayerWins = (value >= 21 && isPlayerTurn) || (value < 21 && !isPlayerTurn);

            if (isPlayerWins)
            {
                playerWins++;
            }
            else
            {
                dealerWins++;
            }

            G.ui.debug.text = $"player wins: {playerWins}";
            G.ui.debug.text += $"\ndealer wins: {dealerWins}";
        }
    }
}