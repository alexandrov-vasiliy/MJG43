using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Card
{
    public class Card : MonoBehaviour
    {
        public ECardSuit cardSuit;
        public int value;
        
        public void OnMouseDown()
        {
            G.hand.GetCard(this);
        }
    
        
    }

}