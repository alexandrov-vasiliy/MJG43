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

            Say("Добро пожаловать! Сначала сделай ставку и я объясню правила.");
            yield return new WaitUntil(() => tutorialStep > 0);
            Say("Перед началом раунда ты ставишь фишки на кон.");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("У каждого из нас в руке по 4 карты. Мы по очереди играем по одной карте на стол. В слепую, ты не видишь мои карты а я не вижу твои.");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("Выбери карту и сыграй.");
            yield return new WaitUntil(() => tutorialStep > 1);
            Say("После того как ты разыгрываешь карту ход переходит ко мне.");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("Вместо того чтобы играть карту, ты можешь нажать на колокольчик — и вскрыть все карты на столе.");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("Если при вскрытии сумма карт на столе _больше 21_ — тот, кто нажал на колокольчик, побеждает.  ");
            yield return new WaitForSeconds(tutorialTextDelay);
            Say("Если сумма _21 или меньше_ — он проигрывает.");
            yield return new WaitForSeconds(tutorialTextDelay);

            Say(" Если никто не нажимает на колокольчик, карты вскрывает тот, у кого раньше всех закончатся карты.  ");
            yield return new WaitForSeconds(tutorialTextDelay);

            Say("Так что долго тянуть не получится — следи за руками.");
            yield return new WaitUntil(() => tutorialStep > 2);
            Say(" После вскрытия раунд завершается. Мы добираем карты, чтобы снова было по 4, и следующий раунд начинает другой игрок.");

            tutorialComplete = true;
        }

        public void Say(string text)
        {
            G.feel.PlaySay();
            dealerTmp.ShowText(text);
        }
    }
}