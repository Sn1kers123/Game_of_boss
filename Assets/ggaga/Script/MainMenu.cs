using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPanel; // Панель меню
    public GameObject player; // Игрок

    private void Start()
    {
        Time.timeScale = 0f; // Останавливаем игру
        menuPanel.SetActive(true);
        player.SetActive(false); // Отключаем игрока
    }

    public void StartGame()
    {
        Time.timeScale = 1f; // Запускаем игру
        menuPanel.SetActive(false);
        player.SetActive(true); // Включаем игрока
    }

    public void ExitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit(); // Закрытие игры
    }
}