using System.Collections;
using UnityEngine;

public class Boss_attack : MonoBehaviour
{
    public LayerMask _attackMask;
    public Rigidbody2D rb;
    public Transform player;

    [Header("Настройки атаки 1")]
    public int _damage = 10;
    public float _attackRange = 5f;
    public Vector3 _attackOffset;

    public Vector2 attackDirection = Vector2.right;

    [Header("Настройки атаки 3")]
    public int _damage3 = 20;
    public float _jumpForce = 10f;
    public float _fallSpeed = 20f;
    public float _attackRange3 = 2f;

    public Vector3 _attackOffset3;

    [Header("Настройки прыжкка для атаки 3")]
    public float maxFallSpeedX = 5f; // Максимальная горизонтальная скорость
    public float jumpRayDistance = 1f; // Дальность проверки перед прыжком
    public LayerMask groundMask; // Слой земли/стен

    private bool isAttacking = false;
    private bool isFalling = false;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Attack1()
    {
        Vector3 position = transform.position + (Vector3)(attackDirection * _attackOffset.x) + Vector3.up * _attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(position, _attackRange, _attackMask);
        if (colInfo != null)
        {
            colInfo.GetComponent<Health>()?.TakeDamage(_damage);
            Debug.Log($"урон прошел!!! нанесено {_damage}");
        }
    }

    public void Attack3()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            GetComponent<Animator>().SetTrigger("Attack3"); // Запускаем анимацию
        }
    }

    // **Событие анимации**: Начало прыжка


    public void StartJump()
    {
        // Проверяем стены слева и справа
        bool wallRight = Physics2D.Raycast(transform.position, Vector2.right, jumpRayDistance, groundMask);
        bool wallLeft = Physics2D.Raycast(transform.position, Vector2.left, jumpRayDistance, groundMask);

        // Если стена есть, уменьшаем силу прыжка
        float jumpPower = (wallRight || wallLeft) ? _jumpForce / 2f : _jumpForce;

        // **Добавляем горизонтальное ускорение в сторону игрока**
        float jumpDirection = (player.position.x > transform.position.x) ? 1 : -1; // Определяем направление
        float jumpDistance = 4f; // Увеличиваем дистанцию (можно менять)

        // **Обновляем скорость прыжка**
        rb.velocity = new Vector2(jumpDirection * jumpDistance, jumpPower);
    }



    // **Событие анимации**: Начало падения


    public void StartFalling()
    {
        isFalling = true;
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);

        // Проверяем, есть ли стены между пауком и игроком
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPosition - (Vector2)transform.position, Mathf.Abs(targetPosition.x - transform.position.x), groundMask);

        // Если на пути есть стена, сбрасываем горизонтальную скорость
        float fallSpeedX = hit.collider != null ? 0 : (targetPosition.x - transform.position.x) * 2;

        // Ограничиваем скорость
        fallSpeedX = Mathf.Clamp(fallSpeedX, -maxFallSpeedX, maxFallSpeedX);

        rb.velocity = new Vector2(fallSpeedX, -_fallSpeed);
    }



    // **Событие анимации**: Удар о землю
    public void DealDamage()
    {
        isFalling = false;

        // **Сброс горизонтальной скорости**
        rb.velocity = new Vector2(0, rb.velocity.y);

        Collider2D colInfo3 = Physics2D.OverlapCircle(transform.position, _attackRange3, _attackMask);
        if (colInfo3 != null)
        {
            colInfo3.GetComponent<Health>()?.TakeDamage(_damage3);
            Debug.Log($"Урон от прыжка прошел!!! Нанесено {_damage3}");
        }
    }


    // **Событие анимации**: Конец атаки
    public void EndAttack()
    {
        isAttacking = false;

        // **Полный сброс скорости**
        rb.velocity = Vector2.zero;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFalling && ((1 << collision.gameObject.layer) & _attackMask) != 0)
        {
            collision.GetComponent<Health>()?.TakeDamage(_damage3);
            Debug.Log($"Урон во время падения! Нанесено {_damage3}");
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position + _attackOffset;
        Gizmos.DrawWireSphere(position, _attackRange);

        Gizmos.color = Color.blue;
        Vector3 position3 = transform.position + _attackOffset3;
        Gizmos.DrawWireSphere(position3, _attackRange3);
    }
}
