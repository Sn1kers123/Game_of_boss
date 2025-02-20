using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public LayerMask enemyLayer; // Слой врагов

    public Transform attackPoint; // Точка атаки
    public Vector2 attackSize = new Vector2(2f, 1f); // Размер зоны атаки для обычного удара
    public Vector2 attack2Size = new Vector2(1f, 2f); // Размер зоны атаки для заряженного удара
    public int attackDamage = 20; // Урон атаки
    public int attack2Damage = 35; // Урон заряженного удара

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool isAttacking = false;
    private float attackHoldTime = 0f;
    private bool isHoldingAttack = false;   

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Move();
        HandleAttack();
    }

    void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(hor, ver).normalized;

        if (!isAttacking) // Движение запрещено во время атаки
        {
            _rigidbody2D.velocity = movement * speed;
            _animator.SetBool("isWalking", movement.sqrMagnitude > 0);
        }

        if (hor > 0)
        {
            _spriteRenderer.flipX = false;
            FlipAttackPoint(false);
        }
        else if (hor < 0)
        {
            _spriteRenderer.flipX = true;
            FlipAttackPoint(true);
        }
    }

    void HandleAttack()
    {
        if (isAttacking) return;

        if (Input.GetMouseButtonDown(0))
        {
            attackHoldTime = Time.time;
            isHoldingAttack = true;
        }

        if (Input.GetMouseButtonUp(0) && isHoldingAttack)
        {
            float heldTime = Time.time - attackHoldTime;

            if (heldTime >= 0.3f)
            {
                StartAttack("attack2"); // Заряженный удар
            }
            else
            {
                StartAttack("attack"); // Обычный удар
            }

            isHoldingAttack = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            StartAttack("attack3"); // Атака луком
        }
    }

    void StartAttack(string attackType)
    {
        isAttacking = true;
        _rigidbody2D.velocity = Vector2.zero;

        _animator.SetBool("isWalking", false);
        _animator.SetBool("isAttacking", true);
        _animator.SetTrigger(attackType);
    }

    public void EndAttack()
    {
        isAttacking = false;
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isWalking", false);
    }

    public void DealDamage(string attackType)
    {
        if (attackPoint == null)
        {
            Debug.LogError("⚠️ Ошибка: AttackPoint не назначен!");
            return;
        }

        Vector2 currentAttackSize = attackSize;
        int currentAttackDamage = attackDamage;

        if (attackType == "attack2")
        {
            currentAttackSize = attack2Size;
            currentAttackDamage = attack2Damage;
        }

        Collider2D[] enemiesHit = Physics2D.OverlapBoxAll(attackPoint.position, currentAttackSize, 0, enemyLayer);

        Debug.Log($"🔴 Проверка атаки ({attackType}): найдено врагов - {enemiesHit.Length}");

        foreach (Collider2D enemy in enemiesHit)
        {
            Debug.Log("⚔️ Обнаружен враг: " + enemy.name);
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(currentAttackDamage);
                Debug.Log($"⚔️ Игрок ударил {enemy.name} ({attackType}) на {currentAttackDamage} урона!");
            }
            else
            {
                Debug.Log("⚠️ Ошибка: у объекта " + enemy.name + " нет компонента Health!");
            }
        }
    }

    private void FlipAttackPoint(bool isFlipped)
    {
        if (attackPoint != null)
        {
            float direction = isFlipped ? -1f : 1f;
            attackPoint.localPosition = new Vector3(direction * Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.position, attackSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackPoint.position, attack2Size);
    }
}