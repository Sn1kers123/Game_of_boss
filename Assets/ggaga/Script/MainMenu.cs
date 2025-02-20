using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menuPanel; // ������ ����
    public GameObject player; // �����

    private void Start()
    {
        Time.timeScale = 0f; // ������������� ����
        menuPanel.SetActive(true);
        player.SetActive(false); // ��������� ������
    }

    public void StartGame()
    {
        Time.timeScale = 1f; // ��������� ����
        menuPanel.SetActive(false);
        player.SetActive(true); // �������� ������
    }

    public void ExitGame()
    {
        Debug.Log("����� �� ����...");
        Application.Quit(); // �������� ����
    }
}