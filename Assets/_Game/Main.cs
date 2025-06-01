using System.Collections;
using System.Collections.Generic;
using _Game.Deck;
using TMPro;
using UnityEngine;

namespace _Game
{
    public class Main : MonoBehaviour
    {
        [SerializeField] private Hand playerHand;
        [SerializeField] private Hand enemyHand;
        [SerializeField] private Brush _brush;

        public DeckShuffler deckShuffler;

        public TMP_Text scoreText;
        private void Awake()
        {
            G.brush = _brush;
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
                        playerChipHolder.GenerateChips(2);
                        break;
                    case 2:
                        playerChipHolder.GenerateChips(1);
                        break;

                }
            }
        }
    }
}
