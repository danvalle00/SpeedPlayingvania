using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    private bool _groundCheck;
    [SerializeField] private Vector3 playerColliderOffset;
    [SerializeField] private float groundCheckLength = 0.95f;
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        var position = transform.position;
        _groundCheck = Physics2D.Raycast(position + playerColliderOffset, Vector2.down, groundCheckLength, groundLayer) ||
                      Physics2D.Raycast(position - playerColliderOffset, Vector2.down, groundCheckLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        Gizmos.color = _groundCheck ? Color.green : Color.yellow;
        var position = transform.position;
        Gizmos.DrawLine(position + playerColliderOffset,
            position + playerColliderOffset + Vector3.down * groundCheckLength);
        Gizmos.DrawLine(position - playerColliderOffset,
            position - playerColliderOffset + Vector3.down * groundCheckLength);
    }
    
    
    public bool GetGroundCheck()
    {
        return _groundCheck;
    }
}
