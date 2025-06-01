using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Game.Bets
{
    public class BetSystem: MonoBehaviour
    {

        public List<ChipHolder> playerChipHolders = new List<ChipHolder>();
        public List<ChipHolder> dealerChipHolders = new List<ChipHolder>();

        public TMP_Text betResultTmp;
        public Button betConfirmButton;
        
        public bool isBetConfirm = false;
        public bool isBettingStage = false;

        private int betResultValue = 0;
        private bool finishGame = false;
        private void Awake()
        {
            G.betSystem = this;
        }

        private void Start()
        {
            foreach (var playerChipHolder in playerChipHolders)
            {
                playerChipHolder.OnClick += HandleClickPlaceBet;
            }     
            
            foreach (var dealerChipHolder in dealerChipHolders)
            {
                dealerChipHolder.OnClick += HandleClickRemoveBet;
            }
            
            betConfirmButton.onClick.AddListener(ConfirmBet);
        }

        private void Update()
        {
            if (isBettingStage )
            {
                if (betResultValue > 0)
                {
                    betResultTmp.text = $"Bet: {betResultValue}";
                    betConfirmButton.gameObject.SetActive(true);
                }
                else
                {
                    betResultTmp.text = "Place your bet!";
                    betConfirmButton.gameObject.SetActive(false);
                }
               
            }
            else
            {
                if (betConfirmButton == null)
                {
                    return;
                }
                betConfirmButton.gameObject.SetActive(false);
                betResultTmp.text = "";
            }
        }

        private void OnDestroy()
        {
            foreach (var playerChipHolder in playerChipHolders)
            {
                playerChipHolder.OnClick -= HandleClickPlaceBet;
            }
            foreach (var dealerChipHolder in dealerChipHolders)
            {
                dealerChipHolder.OnClick -= HandleClickRemoveBet;
            }
            betConfirmButton.onClick.RemoveListener(ConfirmBet);

        }

        private void ConfirmBet()
        {
            Debug.Log("Confirm Bet");
            isBetConfirm = true;
            isBettingStage = false;
        }
        
        private void HandleClickPlaceBet(ChipHolder holder)
        {
            if(!isBettingStage) return;
            
            var someDealerHolder = dealerChipHolders.Find((dealerChipHolder) => dealerChipHolder.type == holder.type);
            
            if(someDealerHolder == null) return;
            if(holder.MoveTo(someDealerHolder))
                betResultValue += holder.chipPrefab.value;
            
        }
        
        private void HandleClickRemoveBet(ChipHolder holder)
        {
            if(!isBettingStage) return;
            var someHolder = playerChipHolders.Find((playerHolder) => playerHolder.type == holder.type);

            if(someHolder == null) return;
            if(holder.MoveTo(someHolder))
                betResultValue -= holder.chipPrefab.value;
            
        }

        public IEnumerator StartPlaceBet()
        {
            G.ui.ClearText();
            isBettingStage = true;
            HighilteHolders(playerChipHolders, true);
            HighilteHolders(dealerChipHolders, true);
            
            G.cameraSwitcher.SetCamera(G.cameraSwitcher.vcBets);
            yield return new WaitUntil(() => isBetConfirm);
            
            isBetConfirm = false;
            isBettingStage = false;

            if (G.ui.tutorialStep == 0)
            {
                G.ui.tutorialStep++;
            }
            
            HighilteHolders(playerChipHolders, false);
            HighilteHolders(dealerChipHolders, false);


            G.cameraSwitcher.SetCamera(G.cameraSwitcher.vcMain);
        }

        public void LoseClear()
        {
            foreach (var dealerChipHolder in dealerChipHolders)
            {
                dealerChipHolder.Clear();
            }
            betResultValue = 0;
            betResultTmp.text = "";

            foreach (var holder in playerChipHolders)
            {
                if (holder.holdedChips.Count != 0)
                {
                    finishGame = false;
                    continue;
                }

                finishGame = true;
            }

            if (finishGame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            
        }

        public IEnumerator WinReward()
        {
            // Пробегаемся по всем холдерам ставок у дилера
            foreach (var dealerHolder in dealerChipHolders)
            {
                // Находим у игрока холдер того же типа фишек
                var playerHolder = playerChipHolders.Find(ph => ph.type == dealerHolder.type);
                if (playerHolder == null) continue;

                // Сколько фишек сейчас в холдере дилера
                int count = dealerHolder.holdedChips.Count;

                // 1) Переместим каждую фишку из холдера дилера к игроку
                for (int i = 0; i < count; i++)
                {
                    dealerHolder.MoveTo(playerHolder);
                    yield return new WaitForSeconds(0.1f);

                }

                // 2) Сгенерируем у игрока столько же фишек того же типа,
                //    чтобы итоговое количество восполнилось в 2 раза
                yield return new WaitForSeconds(0.3f);
                playerHolder.GenerateChips(count);
            }

            // Сбрасываем сумму ставки
            betResultValue = 0;
            // При желании можно обновить отображение: например, очистить текст
            betResultTmp.text = "";
        }

        private void HighilteHolders(List<ChipHolder> holders, bool value)
        {
            foreach (var holder in holders)
            {
                holder.SetHighLight(value);
            }

        }
    }
}