using UnityEngine;

public class EnemyHurting : MonoBehaviour
{
    [SerializeField] private EnemyStats enemyStats;
    private Collider2D _enemyCollider;
    private int _maxHealth;
    private int _currentHealth;

    private void Start()
    {
        _enemyCollider = GetComponent<Collider2D>();
        _maxHealth = enemyStats.health;
        _currentHealth = _maxHealth;
    }
    public void TakeDamage(int attackDamage)
    {
        _currentHealth -= attackDamage;
        // animator shit (hurt)
        if (_currentHealth <= 0)
        {
            Die();
        }
             
    }

    private void Die()
    {
        // animator shit die
        _enemyCollider.enabled = false;
        enabled = false;
    }
}
