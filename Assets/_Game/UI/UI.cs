using System;
using System.Collections;
using Febucci.UI;
using TMPro;
using UnityEngine;

namespace _Game
{
    public enum TutorialSteps
    {
        
    }
    public class UI : MonoBehaviour
    {
        public TMP_Text debug;
        public Canvas overlayCanvas;
        public GameObject tooltip;
        public FloatingValueSpawner floatingValueSpawner;

        public TypewriterByCharacter dealerTmp;

        public int tutorialStep = 0;
        public float tutorialTextDelay = 3f;

        public bool tutorialComplete = false;
        private void Awake()
        {
            G.ui = this;
        }

        public void SayPlayCard()
        {
            if(!tutorialComplete) return;
            Say("<bounce>Play you card!</bounce>");
        }

        public void SayMyTurn()
        {
            if(!tutorialComplete) return;

            Say("<bounce>My turn.</bounce>");
        }

        public void SayOpen()
        {
            if(!tutorialComplete) return;

            Say("<bounce>Open cards</bounce>");
        }

        public void SayPlayerLose()
        {
            if(!tutorialComplete) return;

            Say("<bounce>I'll take the money for myself.</bounce>");
        }
        public void SayPlayerWin()
        {
            if(!tutorialComplete) return;

            Say("<bounce>Enjoy yourself while you can.</bounce>");
        }

        
        
        public void ClearText()
        {
            if(!tutorialComplete) return;

            Say("");
        }

        public IEnumerator Tutorial()
        {
            if(tutorialComplete) yield break;

            Say("Welcome! Each of us has 4 cards in hand.");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("Before the round starts, you place your chips in the pot.");
            yield return new WaitUntil(() => tutorialStep > 0);
            Say("We take turns playing one card at a time, face down. You can’t see my cards, and I can’t see yours.");
            yield return new WaitForSeconds(tutorialTextDelay + 1f);
            Say("Pick a card and play it.");
            yield return new WaitUntil(() => tutorialStep > 1);
            Say("After you play your card, it’s my turn.");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("Instead of playing, you can hit the bell — and reveal all cards on the table.");
            yield return new WaitForSeconds(tutorialTextDelay + 2f);
            Say("If the total is over 21 when revealed, the one who hit the bell wins.");
            yield return new WaitForSeconds(tutorialTextDelay + 2f);
            Say("If the total is 21 or less, they lose.");
            yield return new WaitForSeconds(tutorialTextDelay + 1f);

            Say("If no one rings the bell, the cards are revealed when one player runs out of cards.");
            yield return new WaitForSeconds(tutorialTextDelay + 2f);

            Say("So you can't stall too long — keep an eye on the hands.");
            yield return new WaitUntil(() => tutorialStep > 2);
            Say("After revealing, the round ends. We draw back to 4 cards, and the next round starts with the other player.");

            tutorialComplete = true;
        }

        public void Say(string text)
        {
            G.feel.PlaySay();
            dealerTmp.ShowText(text);
        }
    }
}