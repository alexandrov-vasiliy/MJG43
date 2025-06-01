using UnityEngine;
using UnityEngine.UI;

namespace _Game
{
    public class TestClick : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(Test);
        }

        public void Test()
        {
            Debug.Log("Кнопка нажата!");

        }
    }
}