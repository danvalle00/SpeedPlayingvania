using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    
    // Stats from Scriptable Object
    [SerializeField] private PlayerScriptable playerScriptable;

    private float _nextAttackTime;
    private bool _pressAttack;

    public void OnBasicSwordAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _pressAttack = true;
        }
    }
    
    private void Update()
    {
        if (Time.time >= _nextAttackTime)
        {
            if (_pressAttack)
            {
                _pressAttack = false;
                Attack();
                _nextAttackTime = Time.time + 1f / playerScriptable.attackRate;
            }
        }
            
    }
    
    private void Attack()
    {
        // animator shit (attack)
        var hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, playerScriptable.attackRange, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyHurting>().TakeDamage(playerScriptable.attackDamage);
        }
       
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, playerScriptable.attackRange);
    }
}

    

