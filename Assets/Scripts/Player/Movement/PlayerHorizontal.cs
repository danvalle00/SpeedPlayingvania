using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontal : MonoBehaviour
{
    private Rigidbody2D _playerRigid;
    private PlayerChecks _playerChecks;
    [SerializeField] private PlayerScriptable playerScriptable;
    
    // calculations
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    private float _directionX;
    private float _acceleration;
    private float _turnSpeed;
    private float _deceleration;
    private float _maxSpeedChange;
    
    // states
    private bool _pressingKey;
    private bool _groundCheck;
    private bool _useAcceleration;
    
    //dash
    public bool isDashing;
    private bool _canDash = true;
    private bool _dashKeyPressed;

    private void Awake()
    {
        _playerChecks = GetComponent<PlayerChecks>();
        _playerRigid = GetComponent<Rigidbody2D>();
    }
        
        
    public void OnMovement(InputAction.CallbackContext value)
    {
        _directionX = value.ReadValue<float>();
    }
    public void OnDash(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            _dashKeyPressed = true;
        }
        
        if (value.canceled)
        {
            _dashKeyPressed = false;
        }
    }
    
    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        if (_directionX != 0)
        {
            transform.localScale = new Vector3(_directionX > 0 ? 1 : -1, 1, 1);
            _pressingKey = true;
        }
        else
        {
            _pressingKey = false;
        }
        
        if (_dashKeyPressed && _canDash)
        {
            StartCoroutine(Dash());
            _dashKeyPressed = false;
        }
        
        _desiredVelocity = new Vector2(_directionX, 0f) * playerScriptable.maxSpeed;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        
        _groundCheck = _playerChecks.GetGroundCheck();
        _velocity = _playerRigid.velocity;
        if (_groundCheck)
        {
            _canDash = true;
            Run();
        }
        else
        { 
            AccelerationAirControl();
        }
        
    }
    
    private void Run()
    {
        _velocity.x = _desiredVelocity.x;
        _playerRigid.velocity = _velocity;
    }

    private void AccelerationAirControl()
    {
        if (_pressingKey)
        {
            if (Mathf.Sign(_directionX) != Mathf.Sign(_velocity.x))
            {
                _maxSpeedChange = playerScriptable.maxAirTurnSpeed * Time.deltaTime;
            }
            else
            {
                _maxSpeedChange = playerScriptable.maxAirAcceleration * Time.deltaTime;
            }
        }
        else
        {
            _maxSpeedChange = playerScriptable.maxAirDeceleration * Time.deltaTime;
        }
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
        _playerRigid.velocity = _velocity;
    }
    private IEnumerator Dash()
    {
        _canDash = false;
        isDashing = true;
        var initialGravity = _playerRigid.gravityScale;
        _playerRigid.gravityScale = 0f;
        _playerRigid.velocity = new Vector2(transform.localScale.x * playerScriptable.dashRange, 0f);
        yield return new WaitForSeconds(playerScriptable.dashTimer);
        _playerRigid.gravityScale = initialGravity;
        isDashing = false;
    }
        

}
