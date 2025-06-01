using System;
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
        private void Awake()
        {
            G.ui = this;
        }
    }
}