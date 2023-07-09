using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class WallInteractor : MonoBehaviour
{
    private bool WallJumping { get; set; }
    
    // stats TODO: implement the character stats in the scriptable object
    [SerializeField, Range(0.1f, 5f)] private float wallSlideMaxSpeed;
    [SerializeField] private Vector2 wallJumpClimb = new(4f, 12f);
    [SerializeField] private Vector2 wallJumpBounce = new(10.7f, 10f);
    [SerializeField] private Vector2 wallJumpLeap = new(14f, 12f);
    [SerializeField, Range(0.05f, 0.5f)] private float wallJumpStickTime;
    
    // calculations
    private PlayerChecks _playerChecks;
    private Rigidbody2D _playerRigid;
    private Vector2 _velocity;
    private bool _onWall, _onGround, _desiredJump;
    private float _wallDirectionX, _directionX, _wallStickCounter;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_onWall && !_onGround)
        {
            if (context.started)
            {
                _desiredJump = true;
            }
        }
    }
    
    public void OnMovement(InputAction.CallbackContext value)
    {
        _directionX = value.ReadValue<float>();
    }
    
    private void Start()
    {
        _playerChecks = GetComponent<PlayerChecks>();
        _playerRigid = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        _velocity = _playerRigid.velocity;
        _onWall = _playerChecks.GetWallCheck();
        _onGround = _playerChecks.GetGroundCheck();
        _wallDirectionX = _playerChecks.ContactNormal.x;
        
        #region Wall Slide
        
        if (_onWall)
        {
            if (_velocity.y < -wallSlideMaxSpeed)
            {
                _velocity.y = -wallSlideMaxSpeed;
            }
        }
        #endregion

        #region Wall Stick

        if (_playerChecks.GetWallCheck() && !_playerChecks.GetGroundCheck() && !WallJumping)
        {
            if (_wallStickCounter > 0)
            {
                _velocity.x = 0;
                if (_directionX != 0 && Mathf.Sign(_directionX) == Mathf.Sign(_playerChecks.ContactNormal.x))
                {
                    _wallStickCounter -= Time.deltaTime;
                }
                else
                {
                    _wallStickCounter = wallJumpStickTime;
                }
            }
            else
            {
                _wallStickCounter = wallJumpStickTime;
            }
        }
        
        #endregion
        
        #region Wall Jump

        if (_onWall && _velocity.x == 0 || _onGround)
        {
            WallJumping = false;
        }
        
        if (_desiredJump)
        {
            if (Mathf.Sign(-_wallDirectionX) == MathF.Sign(_directionX))
            {
                _velocity = new Vector2(wallJumpClimb.x * _wallDirectionX, wallJumpClimb.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else if (_directionX == 0)
            {
                _velocity = new Vector2(wallJumpBounce.x * _wallDirectionX, wallJumpBounce.y);
                WallJumping = true;
                _desiredJump = false;
            }
            else
            {
                _velocity = new Vector2(wallJumpLeap.x * _wallDirectionX, wallJumpLeap.y);
                WallJumping = true;
                _desiredJump = false;
            }
        }
        
        #endregion
        
        _playerRigid.velocity = _velocity;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        _playerChecks.EvaluateCollision(other);
        if (_playerChecks.GetWallCheck() && !_playerChecks.GetGroundCheck() && WallJumping)
        {
            _playerRigid.velocity = Vector2.zero;
        }
    }
}
