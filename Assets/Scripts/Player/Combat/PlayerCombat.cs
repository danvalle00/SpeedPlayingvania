using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
     
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerScriptable playerScriptable;
    
    private PlayerChecks _playerChecks;
    private Vector3 _originalPosition;
    
    private float _nextAttackTime;
    private bool _pressAttack;
    private float _directionY;
    
    private const float AttackPointY = 2f;
    public void OnBasicSwordAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _pressAttack = true;
        }
    }

    public void OnVertical(InputAction.CallbackContext context)
    {
        _directionY = context.ReadValue<float>();
    }

    private void Awake()
    {
        _originalPosition = attackPoint.localPosition;
        _playerChecks = attackPoint.GetComponentInParent<PlayerChecks>();
    }

    private void Update()
    {
        if (_directionY > 0) 
        {
            attackPoint.localPosition = new Vector3(0f, AttackPointY, 1);
        }
        else if (_directionY < 0 && !_playerChecks.GetGroundCheck())  
        {
            attackPoint.localPosition = new Vector3(0f, -AttackPointY, 1);
        }
        else
        {
            attackPoint.localPosition = _originalPosition;
        }
        
        if (Time.time >= _nextAttackTime && _pressAttack)
        {
            Debug.Log("attack!");
            _pressAttack = false;
            Attack();
            _nextAttackTime = Time.time + 1f / playerScriptable.attackRate;
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

    

