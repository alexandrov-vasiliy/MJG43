using System;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace _Game
{
    public class UI : MonoBehaviour
    {
        public TMP_Text debug;
        public Canvas overlayCanvas;
        public GameObject tooltip;
        public FloatingValueSpawner floatingValueSpawner;

        public TypewriterByCharacter dealerTmp;
        private void Awake()
        {
            G.ui = this;
        }

        public void SayPlayCard()
        {
            dealerTmp.ShowText("<bounce>Play you card!</bounce>");
        }

        public void SayMyTurn()
        {
            dealerTmp.ShowText("<bounce>My turn.</bounce>");
        }

        public void SayOpen()
        {
            dealerTmp.ShowText("<bounce>Open cards</bounce>");
        }

        public void SayPlayerLose()
        {
            dealerTmp.ShowText("<bounce>I'll take the money for myself.</bounce>");
        }
        public void SayPlayerWin()
        {
            dealerTmp.ShowText("<bounce>Enjoy yourself while you can.</bounce>");
        }

        
        
        public void ClearText()
        {
            dealerTmp.ShowText("");
        }
    }
}