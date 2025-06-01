using System.Collections;
using System.Collections.Generic;
using _Game.Deck;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Hand playerHand;
        [SerializeField] private Hand enemyHand;

        public DeckShuffler deckShuffler;

        [FormerlySerializedAs("scoreText")] public TMP_Text finalResultTmp;
        private void Awake()
        {
            G.main = this;
            G.playerHand = playerHand;
            G.playerHand.isPlayer = true;
            G.enemyHand = enemyHand;
        }

        private void Start()
        {
            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            StartCoroutine(G.ui.Tutorial());
            G.deck.cardPrefabs = new List<GameObject>(deckShuffler.defaultDeck.cardPrefabs);
            GiveStartChips();
            G.deck.GenerateDeck();
            yield return new WaitForSeconds(1f);
            yield return G.coreLoop.StartRound();
        }

        private void GiveStartChips()
        {
            for (var index = 0; index < G.betSystem.playerChipHolders.Count; index++)
            {
                var playerChipHolder = G.betSystem.playerChipHolders[index];
                switch (index)
                {
                    case 0: 
                        playerChipHolder.GenerateChips(4);
                        break;
                    case 1:
                        playerChipHolder.GenerateChips(1);
                        break;
                    case 2:
                        playerChipHolder.GenerateChips(2);
                        break;

                }
            }
        }
    }
}
