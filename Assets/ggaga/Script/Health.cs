using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Максимальное здоровье
    private int currentHealth; // Текущее здоровье
    private Enemy enemy; // Ссылка на компонент Enemy, если объект является врагом
    private Boss boss;

    public int CurrentHealth => currentHealth; // Свойство для получения текущего здоровья

    private void Start()
    {
        currentHealth = maxHealth; // Устанавливаем здоровье в максимум
        enemy = GetComponent<Enemy>(); // Получаем компонент Enemy (если он есть)
        boss = GetComponent<Boss>();
    }

    // Метод получения урона
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Если здоровье уже 0, выходим

        currentHealth -= damage; // Вычитаем урон
        Debug.Log($"{gameObject.name} получил {damage} урона! HP: {currentHealth}");

        if (enemy != null)
        {
            enemy.TakeDamage(damage); // Передаем урон врагу
        }


        if (boss != null) boss.TakeDamage(damage); 


        if (currentHealth <= 0)
        {
            Die(); // Если здоровье закончилось, вызываем смерть
        }
    }

    // Метод смерти объекта (если необходимо)
    private void Die()
    {
        Debug.Log($"{gameObject.name} умер!");
        
    }
}
