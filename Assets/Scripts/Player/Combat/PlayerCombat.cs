
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCombat : MonoBehaviour
{
    
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerScriptable playerScriptable;
    
    // TODO: implement the character stats in the scriptable object
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRate;
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
                _nextAttackTime = Time.time + 1f / attackRate;
            }
        }
            
    }
    
    private void Attack()
    {
        // animator shit (attack)
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach (Collider2D enemy in hitEnemies)
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
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

    

