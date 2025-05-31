using System;
using UnityEngine;

namespace _Game.Bell
{
    [RequireComponent(typeof(BoxCollider), typeof(Outline))]
    public class Bell : MonoBehaviour
    {
        private Outline outline;

        private void Start()
        {
            outline = GetComponent<Outline>();
        }

        private void Update()
        {
            outline.enabled = G.coreLoop.isPlayerTurn;
        }

        private void OnMouseDown()
        {
            if(!G.coreLoop.isPlayerTurn) return;

            G.coreLoop.OpenCards();
        }
    }
}