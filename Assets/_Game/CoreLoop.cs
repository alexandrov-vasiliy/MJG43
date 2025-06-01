using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Game
{
    public enum CoreStatus
    {
        ChooseCard,
        Opening,
        Bet
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
            status = CoreStatus.ChooseCard;
            G.ui.SayPlayCard();
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
            G.ui.SayMyTurn();
            float think = Random.Range(0.5f, 2f);
            yield return new WaitForSeconds(1f);

            if (G.enemyHand.cards.Count == 0)
            {
                G.ui.SayOpen();
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
                        G.ui.SayOpen();
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
                        G.ui.SayOpen();
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
            if (round == 10)
            {
                SceneManager.LoadScene("Final Scene");
            }
            
            
            G.cameraSwitcher.SetCamera(G.cameraSwitcher.vcMain);
            round++;
            OnRoundStart?.Invoke(round);
            yield return PlayerDraw();
            yield return EnemyDraw();

            status = CoreStatus.Bet;
            yield return G.betSystem.StartPlaceBet();
            status = CoreStatus.ChooseCard;
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
            status = CoreStatus.Opening;
            G.feel.PlayBell();
            G.cameraSwitcher.SetCamera(G.cameraSwitcher.vcFront);
            yield return new WaitForSeconds(0.7f);
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
            G.main.finalResultTmp.text = "";

            yield return StartRound();
        }

        private IEnumerator CalculateWinner(float value)
        {
            bool isPlayerWins = (value >= 21 && isPlayerTurn) || (value < 21 && !isPlayerTurn);

            if (isPlayerWins)
            {
                G.ui.SayPlayerWin();
                playerWins++;
                yield return G.betSystem.WinReward();
            }
            else
            {
                G.ui.SayPlayerLose();
                dealerWins++;
                G.betSystem.LoseClear();
            }
        }
        
        
    }
}