using System;
using UnityEngine;

namespace _Game.Bell
{
    [RequireComponent(typeof(BoxCollider), typeof(Outline))]
    public class Bell : MonoBehaviour
    {
        private Outline outline;

        public bool canRing = false;
        private void Start()
        {
            outline = GetComponent<Outline>();
        }

        private void Update()
        {
            canRing = G.coreLoop.isPlayerTurn && G.board.playerCards.Count > 1;
            outline.enabled = canRing;
        }

        private void OnMouseDown()
        {
            if(!canRing) return;

           StartCoroutine( G.coreLoop.OpenCards());
        }
    }
}