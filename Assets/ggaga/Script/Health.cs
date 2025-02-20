using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // ������������ ��������
    private int currentHealth; // ������� ��������
    private Enemy enemy; // ������ �� ��������� Enemy, ���� ������ �������� ������
    private Boss boss;

    public int CurrentHealth => currentHealth; // �������� ��� ��������� �������� ��������

    private void Start()
    {
        currentHealth = maxHealth; // ������������� �������� � ��������
        enemy = GetComponent<Enemy>(); // �������� ��������� Enemy (���� �� ����)
        boss = GetComponent<Boss>();
    }

    // ����� ��������� �����
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // ���� �������� ��� 0, �������

        currentHealth -= damage; // �������� ����
        Debug.Log($"{gameObject.name} ������� {damage} �����! HP: {currentHealth}");

        if (enemy != null)
        {
            enemy.TakeDamage(damage); // �������� ���� �����
        }


        if (boss != null) boss.TakeDamage(damage); 


        if (currentHealth <= 0)
        {
            Die(); // ���� �������� �����������, �������� ������
        }
    }

    // ����� ������ ������� (���� ����������)
    private void Die()
    {
        Debug.Log($"{gameObject.name} ����!");
        
    }
}
