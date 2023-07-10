using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontal : MonoBehaviour
{
    private Rigidbody2D _playerRigid;
    private PlayerChecks _playerChecks;
    
    // Stats from Scriptable Object
    [SerializeField] private PlayerScriptable playerScriptable;
    
    // calculations
    private Vector2 _desiredVelocity;
    private Vector2 _velocity;
    private float _directionX;
    private float _acceleration;
    private float _turnSpeed;
    private float _deceleration;
    private float _maxSpeedChange;
    
    private bool _pressingKey;
    private bool _groundCheck;
    private bool _useAcceleration;
    private void Awake()
    {
        _playerChecks = GetComponent<PlayerChecks>();
        _playerRigid = GetComponent<Rigidbody2D>();
    }
        
    public void OnMovement(InputAction.CallbackContext value)
    {
        _directionX = value.ReadValue<float>();
    }
    
    private void Update()
    {
        if (_directionX != 0)
        {
            transform.localScale = new Vector3(_directionX > 0 ? 1 : -1, 1, 1);
            _pressingKey = true;
        }
        else
        {
            _pressingKey = false;
        }


        _desiredVelocity = new Vector2(_directionX, 0f) * playerScriptable.maxSpeed;
    }

    private void FixedUpdate()
    {
        _groundCheck = _playerChecks.GetGroundCheck();
        _velocity = _playerRigid.velocity;
        if (_groundCheck)
        { 
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
}
