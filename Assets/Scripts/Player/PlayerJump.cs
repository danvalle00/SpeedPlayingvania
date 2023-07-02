using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerJump : MonoBehaviour
{
    // components
    private Rigidbody2D _playerRigid;
    private PlayerChecks _playerChecks;

    private Vector2 _velocity;
    private float _jumpSpeed;

    // stats and options
    [SerializeField] private float jumpHeight;
    [SerializeField] private int maxAirJumps;
    [SerializeField] private bool variableJumpHeight;
    [SerializeField] private float timeToJumpApex;
    
    // gravity stats
    [SerializeField] private float upwardMovementMultiplier;
    [SerializeField] private float downwardMovementMultiplier;
    [SerializeField] private float jumpCutOff;
    [SerializeField] private float maxSpeedFall;
    private float _gravMultiplier;

    private float _defaultGravityScale;


    // state
    [SerializeField] private bool canJumpAgain;
    private bool _desiredJump;
    private bool _pressingJump;
    private bool _currentlyJumping;
    [SerializeField] private bool groundCheck;

    // private float multipleJumpCounter;
    private void Start()
    {
        _playerChecks = GetComponent<PlayerChecks>();
        _playerRigid = GetComponent<Rigidbody2D>();
        _defaultGravityScale = 1f;

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
        SetPhysics();
        groundCheck = _playerChecks.GetGroundCheck();
        _velocity = _playerRigid.velocity;
        if (_desiredJump)
        {
            DoAJump();
            _playerRigid.velocity = _velocity;
            return;
        }
        
        CalculateGravity();
    }
    
    private void SetPhysics()
    {
        var newGravity = new Vector2(0, -2f * jumpHeight / (timeToJumpApex * timeToJumpApex));
        _playerRigid.gravityScale = newGravity.y / Physics2D.gravity.y * _gravMultiplier;
    }
    
    private void CalculateGravity() // this can be refactored if i decided the numbers for theses upward/downward multipliers be zero
    {
        if (_playerRigid.velocity.y > 0.01f)
        {
            if (groundCheck)
            {
                _gravMultiplier = _defaultGravityScale;
            }
            else
            {
                if (variableJumpHeight)
                {
                    if (_pressingJump && _currentlyJumping)
                    {
                        _gravMultiplier = upwardMovementMultiplier;
                    }
                    else
                    {
                        _gravMultiplier = jumpCutOff;
                    }
                }
                else
                {
                    _gravMultiplier = _defaultGravityScale;
                }
            }

        }
        else if (_playerRigid.velocity.y < -0.01f)
        {
            _gravMultiplier = groundCheck ? _defaultGravityScale : downwardMovementMultiplier;
        }
        else
        {
            if (groundCheck)
            {
                _gravMultiplier = _defaultGravityScale;
            }
        }
        
        _playerRigid.velocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -maxSpeedFall, 100));
    }
    
    private void DoAJump()
    {
        if (groundCheck || canJumpAgain)
        {
            _desiredJump = false;
            canJumpAgain = (maxAirJumps == 1 && canJumpAgain == false);

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _playerRigid.gravityScale * jumpHeight);
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
            _currentlyJumping = true;
        }
    }
}
