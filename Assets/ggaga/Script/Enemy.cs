using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHP = 100; // Максимальное здоровье врага
    private int currentHP; // Текущее здоровье врага
    private Animator _animator; // Ссылка на компонент Animator для анимации врага
    private bool isDead = false; // Флаг, показывающий, что враг мертв
    private bool isTakingDamage = false; // Флаг получения урона, предотвращает повторную анимацию

    private void Awake()
    {
        _animator = GetComponent<Animator>(); // Получаем компонент Animator
        currentHP = maxHP; // Устанавливаем текущее здоровье на максимум
    }

    // Метод получения урона
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Если враг уже мертв, игнорируем урон

        if (!isTakingDamage) // Если не выполняется анимация получения урона
        {
            currentHP -= damage; // Вычитаем урон из текущего здоровья
            Debug.Log($"Враг получил урон: {damage}. Осталось HP: {currentHP}");

            if (currentHP > 0)
            {
                StartCoroutine(PlayTakeDamageAnimation()); // Запускаем анимацию получения урона
            }
            else
            {
                Die(); // Если здоровье закончилось, вызываем метод смерти
            }
        }
    }

    // Корутина для анимации получения урона
    private System.Collections.IEnumerator PlayTakeDamageAnimation()
    {
        isTakingDamage = true; // Устанавливаем флаг, чтобы предотвратить повторную анимацию
        _animator.SetTrigger("TakeDamage"); // Запускаем анимацию получения урона
        Debug.Log("Запуск анимации получения урона!");

        // Ждем окончания текущей анимации
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        isTakingDamage = false; // Сбрасываем флаг после окончания анимации
    }

    // Метод смерти врага
    private void Die()
    {
        if (isDead) return; // Если враг уже мертв, ничего не делаем

        isDead = true; // Устанавливаем флаг смерти
        Debug.Log("Функция Die вызвана!");
        _animator.SetTrigger("Die"); // Запускаем анимацию смерти

        // Отключаем коллайдер, чтобы игрок не мог взаимодействовать с мертвым врагом
        GetComponent<Collider2D>().enabled = false;

        // Запускаем корутину для удаления объекта после анимации смерти
        StartCoroutine(WaitForDeathAnimation());
    }

    // Корутина ожидания окончания анимации смерти перед удалением объекта
    private System.Collections.IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length); // Ждём окончания анимации смерти
        Destroy(gameObject); // Удаляем объект из сцены
    }
}
