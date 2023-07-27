using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class EnemyHurting : MonoBehaviour
{
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private PlayerScriptable playerStats;
    [SerializeField] private Transform playerTransform;
    private Collider2D _enemyCollider;
    private Rigidbody2D _rigidbody2D;
    private int _maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _enemyCollider = GetComponent<Collider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _maxHealth = enemyStats.health;
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(int attackDamage)
    {
        _currentHealth -= attackDamage;
        // animator shit (hurt)
        // _rigidbody2D.AddForce(new Vector2(playerTransform.localScale.x * playerStats.attackKnockback, 0f), ForceMode2D.Impulse);
        
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // animator shit die
        _rigidbody2D.velocity = Vector2.zero;
        _enemyCollider.enabled = false;
        _rigidbody2D.isKinematic = true;
        enabled = false;
    }

    
}
