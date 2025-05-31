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

        public DeckShuffler deckShuffler;

        public TMP_Text scoreText;
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
            G.deck.cardPrefabs = new List<GameObject>(deckShuffler.defaultDeck.cardPrefabs);
            G.deck.GenerateDeck();
            yield return new WaitForSeconds(1f);
            yield return G.coreLoop.StartRound();
        }
    }
}
