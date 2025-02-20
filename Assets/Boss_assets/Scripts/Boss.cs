using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;
    private bool isFlipped = false;
    private BoxCollider2D boxCollider;
    public int maxHP = 100;
    private int currentHP;
    public Animator _animator;

    private bool isInvulnerable = false;
    public float invulnerabilityDuration = 1f;
    private bool isDead = false;

    public LifeBar lifebar;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        currentHP = maxHP;

        if (lifebar == null)
        {
            lifebar = GetComponentInChildren<LifeBar>();
            if (lifebar == null)
            {
                Debug.LogError("❌ Ошибка: LifeBar не найден! Убедись, что он прикреплён к боссу.");
                return;
            }
        }

        lifebar.maxHealth = maxHP;
        lifebar.SetHP(currentHP);
        Debug.Log("✅ LifeBar найден и настроен!");
    }


    public void TakeDamage(int damage)
    {
        if (isInvulnerable || isDead) return;

        currentHP -= damage;
        _animator.SetTrigger("LowTakeDamage");

        if (lifebar != null)
            lifebar.SetHP(currentHP);

        if (currentHP > 0)
        {
            StartCoroutine(InvulnerabilityTimer());
        }
        else
        {
            lifebar.HideBar();
            Die();
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        Debug.Log("Босс неуязвим!");
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
        Debug.Log("Босс снова может получать урон!");
    }

    private void Die()
    {
        _animator.SetBool("isDie", true);
        GetComponent<Collider2D>().enabled = false;
        isDead = true;
        this.enabled = false;
    }

    void Update()
    {
        LookAtPlayer();
    }

    public void LookAtPlayer()
    {
        if (player == null) return;

        if (transform.position.x > player.position.x && !isFlipped)
        {
            Flip();
        }
        else if (transform.position.x < player.position.x && isFlipped)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFlipped = !isFlipped;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        if (boxCollider != null)
        {
            Vector2 offset = boxCollider.offset;
            offset.x *= -1;
            boxCollider.offset = offset;
        }
    }
}
