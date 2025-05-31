using UnityEngine;

namespace _Game
{
    public class Main : MonoBehaviour
    {
        private void Awake()
        {
            G.main = this;
        }
    }
}
