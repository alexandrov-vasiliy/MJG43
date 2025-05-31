using System;
using UnityEngine;

namespace _Game.Deck
{
    public class DeckShuffler : MonoBehaviour
    {
        public DeckData defaultDeck;
        public DeckData negativeDeck;
        public DeckData blankDeck;

        private void Start()
        {
            G.coreLoop.OnRoundStart += HandleRoundStart;
        }

        private void OnDestroy()
        {
            G.coreLoop.OnRoundStart -= HandleRoundStart;
        }

        private void HandleRoundStart(int round)
        {
            if (round == 3)
            {
                G.deck.cardPrefabs.AddRange(G.main.deckShuffler.negativeDeck.cardPrefabs);
                G.deck.GenerateDeck();
            }

            if (round == 5)
            {
                G.deck.cardPrefabs.AddRange(G.main.deckShuffler.blankDeck.cardPrefabs);
                G.deck.GenerateDeck();

            }

        }
    }
}