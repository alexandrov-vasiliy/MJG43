using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace _Game.Deck
{
    public class DeckShuffler : MonoBehaviour
    {
        public DeckData defaultDeck;
        public DeckData negativeDeck;
        public DeckData blankDeck;

        private List<GameObject> allCards = new List<GameObject>();

        private void Start()
        {
            allCards.AddRange(
                defaultDeck.cardPrefabs
                    .Concat(negativeDeck.cardPrefabs)
                    .Concat(blankDeck.cardPrefabs)
            );
            
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

        public GameObject GenerateOneCard()
        {
            if (allCards == null || allCards.Count == 0)
            {
                Debug.LogError("Deck is empty!");
                return null;
            }

            var random = new System.Random();
            var prefab = allCards[random.Next(allCards.Count)];
    
            if (prefab == null)
            {
                Debug.LogError("Selected prefab is null");
                return null;
            }

            var instance = Instantiate(prefab);
            return instance;
        }
        
    }
}