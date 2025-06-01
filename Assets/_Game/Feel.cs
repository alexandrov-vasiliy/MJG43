using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Game
{
    public class Feel : MonoBehaviour
    {
        [SerializeField] private MMF_Player chipMoveFeedback;
        [SerializeField] private MMF_Player chipGivesFeedback;

        [SerializeField] private MMF_Player cardHoverFeedback;
        [SerializeField] private MMF_Player cardPlaceFeedback;
        [SerializeField] private MMF_Player cardDrawFeedback;
        [SerializeField] private MMF_Player bellFeedback;
        [SerializeField] private MMF_Player cardFlipFeedback;
        [SerializeField] private MMF_Player sayFeedback;


        private void Awake()
        {
            G.feel = this;
        }

        public void PlaySay()
        {
            sayFeedback.PlayFeedbacks();
        }
        
        public void PlayChipMove()
        {
            chipMoveFeedback.PlayFeedbacks();
        }

        public void PlayChiGives()
        {
            chipGivesFeedback.PlayFeedbacks();
        }
        public void PlayCardHover()
        {
            cardHoverFeedback.PlayFeedbacks();   
        }

        public void PlayCardPlace()
        {
            cardPlaceFeedback.PlayFeedbacks();
        }
        
        public void PlayCardDraw()
        {
            cardDrawFeedback.PlayFeedbacks();
        }
        
        public void PlayBell()
        {
            bellFeedback.PlayFeedbacks();
        }

        public void PlayCardFlip()
        {
            cardFlipFeedback.PlayFeedbacks();
        }
        
    }
}