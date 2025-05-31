using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game
{
    public enum CoreStatus
    {
        mainPart,
        opening,
        finish
    }
    
    
    public class CoreLoop : MonoBehaviour
    {
        public bool isPlayerTurn = false;
        public bool playerFirstTurn = true;
        public CoreStatus status;
        public int playerWins = 0;
        public int dealerWins = 0;

        public int round = 0;

        public event Action<int> OnRoundStart;

        private void Awake()
        {
            G.coreLoop = this;
        }

        private void Update()
        {
            G.ui.debug.text = $" round: {round}";
            G.ui.debug.text += $"\nplayer wins: {playerWins}";
            G.ui.debug.text += $"\ndealer wins: {dealerWins}";
        }

        public IEnumerator PlayerDraw()
        {
            status = CoreStatus.mainPart;
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

            if (G.board.playerCards.Count >= 4 || G.board.CalculateValue(G.board.playerCards) >= 30)
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

            yield return new WaitForSeconds(0.3f);
            yield return PlayerTurn();
        }

        public IEnumerator StartRound()
        {
            round++;
            OnRoundStart?.Invoke(round);

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
            status = CoreStatus.opening;
            StartCoroutine(G.board.RevealCards());
            G.main.scoreText.text = "You can use a brush";
            StartCoroutine(CheckForUsingBrush());
        }

        private IEnumerator CheckForUsingBrush()
        {
            float elapsed = 0f;
            while (!G.brush.isUsed && elapsed < 5f)
            {
                yield return null; // ждём следующий кадр
                elapsed += Time.deltaTime;
            }
            
            float value = G.board.CalculateValue(G.board.playerCards);
            CalculateWinner(value);
            G.main.scoreText.text = value.ToString();
            isPlayerTurn = false;
            yield return FinishRound();
        }

        private IEnumerator FinishRound()
        {
            status = CoreStatus.finish;
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
        }
    }
}