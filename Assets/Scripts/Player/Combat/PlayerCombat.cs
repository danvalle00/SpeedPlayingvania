using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    // basic slash 
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    
    // Stats from Scriptable Object
    [SerializeField] private PlayerScriptable playerScriptable;
    // basic slash
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
        // basic slash attack rate
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
    
    private void Attack() // need refactor
    {
        // animator shit (attack)
        var hitEnemy = Physics2D.OverlapCircle(attackPoint.position, playerScriptable.attackRange, enemyLayer);
        if (hitEnemy)
        {
            hitEnemy.GetComponent<EnemyHurting>().TakeDamage(playerScriptable.attackDamage);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, playerScriptable.attackRange);
    }
}

    

