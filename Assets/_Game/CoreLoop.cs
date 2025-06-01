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
                yield return OpenCards();
                yield break;
            }
            float coinFlip = Random.Range(0, 1);
            if (coinFlip == 0)
            {
                if (G.board.playerCards.Count >= 3)
                {
                    float r = Random.Range(0f, 1f);
                    Debug.Log(r);
                    if (r >= 0.5f)
                    {
                        yield return OpenCards();
                        yield break;
                    }
                }
            }
            else
            {
                if (G.board.CalculateValue(G.board.playerCards) >= 21)
                {
                    float r = Random.Range(0f, 1f);
                    Debug.Log(r);
                    if (r >= 0.4f)
                    {
                        yield return OpenCards();
                        yield break;
                    }
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
            G.cameraSwitcher.SetCamera(G.cameraSwitcher.vcMain);
            round++;
            OnRoundStart?.Invoke(round);
            yield return PlayerDraw();
            yield return EnemyDraw();
            
            yield return G.betSystem.StartPlaceBet();

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

        public IEnumerator OpenCards()
        {   
            G.cameraSwitcher.SetCamera(G.cameraSwitcher.vcFront);
            yield return new WaitForSeconds(2);
            yield return G.board.RevealCards();
            float value = G.board.CalculateValue(G.board.playerCards);
            yield return CalculateWinner(value);
            isPlayerTurn = false;
            yield return  FinishRound();
        }

        private IEnumerator FinishRound()
        {
            yield return new WaitForSeconds(2f);
            G.board.ClearBoard();
            G.main.scoreText.text = "";

            yield return StartRound();
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
            yield return CalculateWinner(value);
            G.main.scoreText.text = value.ToString();
            isPlayerTurn = false;
            yield return FinishRound();
        }
        
        private IEnumerator CalculateWinner(float value)
        {
            bool isPlayerWins = (value >= 21 && isPlayerTurn) || (value < 21 && !isPlayerTurn);

            if (isPlayerWins)
            {
                playerWins++;
                yield return G.betSystem.WinReward();
            }
            else
            {
                dealerWins++;
                G.betSystem.LoseClear();
            }
        }
        
        
    }
}