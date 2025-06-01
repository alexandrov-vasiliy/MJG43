using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSystem : MonoBehaviour
{
    public void RestartToMainScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("SampleScene"); // Замените "MainScene" на имя вашей основной сцены
    }
}