using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Deck
{
    public class DeckShuffler : MonoBehaviour
    {
        public DeckData defaultDeck;
        public DeckData negativeDeck;
        public DeckData blankDeck;

        private List<GameObject> allCards = new List<GameObject>();

        private void Awake()
        {
            G.deckShuffler = this;
        }

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

        public Card.Card GenerateOneCard()
        {
            if (allCards == null || allCards.Count == 0)
            {
                Debug.LogError("Deck is empty!");
                return null;
            }

            var prefab = allCards[Random.Range(0, allCards.Count)];
    
            if (prefab == null)
            {
                Debug.LogError("Selected prefab is null");
                return null;
            }

            var instance = Instantiate(prefab);
            
            Card.Card card;
            
            if (!instance.TryGetComponent<Card.Card>(out card))
            {
                card = instance.AddComponent<Card.Card>();
            }

            card.InitInDeck();
            return card;
        }
        
    }
}