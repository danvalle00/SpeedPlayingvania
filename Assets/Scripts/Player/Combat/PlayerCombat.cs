using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
     
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private PlayerScriptable playerScriptable;
    
    private Rigidbody2D _playerRigid;
    private PlayerChecks _playerChecks;
    
    private Vector3 _originalPosition;
    private float _originalRange;
    
    private float _nextAttackTime;
    private bool _pressAttack;
    private float _directionY;
    
    private const float PoggoForce = 20f;
    private const float AttackPointY = 2f;
    private const float AirAttackRange = 1.2f;
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
        _originalRange = playerScriptable.attackRange;
        _originalPosition = attackPoint.localPosition;
        _playerChecks = attackPoint.GetComponentInParent<PlayerChecks>();
        _playerRigid = attackPoint.GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_directionY > 0) 
        {
            attackPoint.localPosition = new Vector3(0f, AttackPointY, 1);
        }
        else if (_directionY < 0 && !_playerChecks.GetGroundCheck())
        {
            playerScriptable.attackRange = AirAttackRange;
            attackPoint.localPosition = new Vector3(0f, -AttackPointY, 1);
        }
        else
        {
            playerScriptable.attackRange = _originalRange;
            attackPoint.localPosition = _originalPosition;
        }
        
        if (Time.time >= _nextAttackTime && _pressAttack)
        {
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
            PoggoAirSlash();
            hitEnemy.GetComponent<EnemyHurting>().TakeDamage(playerScriptable.attackDamage);
        }
    }

    private void PoggoAirSlash()
    {
        if (!_playerChecks.GetGroundCheck()) // TODO:Implementar a condição de outros objetos além de inimigos
        {
            _playerRigid.velocity = new Vector2(_playerRigid.velocity.x / 2, PoggoForce);
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

    

