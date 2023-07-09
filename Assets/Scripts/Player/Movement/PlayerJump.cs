using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerJump : MonoBehaviour
{
    // components
    private Rigidbody2D _playerRigid;
    private PlayerChecks _playerChecks;
    private Vector2 _velocity;


    // stats and options
    [SerializeField] private float jumpHeight;
    [SerializeField] private int maxAirJumps;
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime;
    
    // gravity stats
    [SerializeField] private float upwardMovementMultiplier;
    [SerializeField] private float downwardMovementMultiplier;
     
    
    // calculations
    private float _jumpSpeed, _defaultGravityScale, _coyoteCounter;
    private int _jumpPhase;
    private bool _pressingJump, _groundCheck, _desiredJump, _isJumping, _isJumpReset;
    
    
    private void Awake()
    {
        _playerChecks = GetComponent<PlayerChecks>();
        _playerRigid = GetComponent<Rigidbody2D>();
        _defaultGravityScale = 1f;
        _isJumpReset = true;

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _desiredJump = true;
            _pressingJump = true;
        }
        if (context.canceled)
        {
            _pressingJump = false;
        }
    }

    
    private void FixedUpdate()
    {
        _groundCheck = _playerChecks.GetGroundCheck();
        _velocity = _playerRigid.velocity;

        if (_groundCheck && _playerRigid.velocity.y == 0)
        {
            _jumpPhase = 0;
            _coyoteCounter = coyoteTime;
            _isJumping = false;

        }
        else
        {
            _coyoteCounter -= Time.deltaTime;
        }
        
        if (_desiredJump && _isJumpReset)
        {
            _isJumpReset = false;
            _desiredJump = false;
            DoAJump();
        }
        else if (!_desiredJump)
        {
            _isJumpReset = true;
        }
        CalculateGravity();
        _playerRigid.velocity = _velocity;
    }
    
    private void CalculateGravity()
    {
        if (_pressingJump && _playerRigid.velocity.y > 0f)
        {
            _playerRigid.gravityScale = upwardMovementMultiplier;
        }
        else if (!_pressingJump || _playerRigid.velocity.y < 0f)
        {
            _playerRigid.gravityScale = downwardMovementMultiplier;
        }
        else if (_playerRigid.velocity.y == 0)
        {
            _playerRigid.gravityScale = _defaultGravityScale;
        }
    }
        
        
        
    
    private void DoAJump()
    {
        if (_coyoteCounter > 0f || (_jumpPhase < maxAirJumps && _isJumping))        
        {
            if (_isJumping)
            {
                _jumpPhase += 1;
            }
            _coyoteCounter = 0;
             
                
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            _isJumping = true;
            
            // second jump speed calculations, here you can tweaks the numbers later for a lower jump height when doing
            // the second jump (hollow knight: first jump= 4x character, second jump 2x character more or less)
            // This will ensure the jump is the exact same strength, no matter your velocity.
            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_playerRigid.velocity.y);
            }
            
            _velocity.y += _jumpSpeed;
        }
    }
}
            
            
