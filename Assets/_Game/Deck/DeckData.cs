using System.Collections.Generic;
using UnityEngine;

namespace _Game.Deck
{
    [CreateAssetMenu(fileName = "DeckData", menuName = "Deck/Data", order = 0)]
    public class DeckData : ScriptableObject
    {
        public List<GameObject> cardPrefabs;
    }
}