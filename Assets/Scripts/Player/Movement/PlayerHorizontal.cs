using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHorizontal : MonoBehaviour
{
    private Rigidbody2D _playerRigid;
    private PlayerChecks _playerChecks;

    // stats  TODO: implement the character stats in the scriptable object
    [SerializeField] private float maxSpeed;
    // in this game acceleration works only midair, on ground the player doesnt have acceleration;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxDeceleration;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float maxAirAcceleration;
    [SerializeField] private float maxAirDeceleration;
    [SerializeField] private float maxAirTurnSpeed;
    
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
            
        
        _desiredVelocity = new Vector2(_directionX, 0f) * maxSpeed;
    }

    private void FixedUpdate()
    {
        _groundCheck = _playerChecks.GetGroundCheck();
        _velocity = _playerRigid.velocity;
        
        
        if (_useAcceleration)
        {
            RunWithAcceleration();
        }
        else
        {
            if (_groundCheck)
            {
                Run();
            }
            else
            {
                RunWithAcceleration();
            }
        }
            
    }
            
             
            
    private void Run()
    {
        _velocity.x = _desiredVelocity.x;
        _playerRigid.velocity = _velocity;
    }

    private void RunWithAcceleration()
    {
        // tweak this numbers in the inspector;
        _acceleration = _groundCheck ? maxAcceleration : maxAirAcceleration;
        _turnSpeed = _groundCheck ? maxTurnSpeed : maxAirTurnSpeed;
        _deceleration = _groundCheck ? maxDeceleration : maxAirDeceleration;

        if (_pressingKey)
        {
            if (Mathf.Sign(_directionX) != Mathf.Sign(_velocity.x))
            {
                _maxSpeedChange = _turnSpeed * Time.deltaTime;
            }
            else
            {
                _maxSpeedChange = _acceleration * Time.deltaTime;
            }
        }
        else
        {
            _maxSpeedChange = _deceleration * Time.deltaTime;
        }
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
        _playerRigid.velocity = _velocity;
    }
}
