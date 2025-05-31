using System;
using TMPro;
using UnityEngine;

namespace _Game
{
    public class UI : MonoBehaviour
    {
        public TMP_Text debug;
        private void Awake()
        {
            G.ui = this;
        }
    }
}