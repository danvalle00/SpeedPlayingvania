using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStats", menuName = "ScriptableObjects/Player/PlayerStats")]
public class PlayerScriptable : ScriptableObject
{
    [Header("Horizontal Movement Stats")] public float maxSpeed;
    
    [Header("Dashing Stats")] public float dashRange;
    public float dashTimer;
    public float dashCooldown;
    
    [Header("AirControl Stats")] public float maxAirAcceleration;
    public float maxAirDeceleration;
    public float maxAirTurnSpeed;

    [Header("Jump Stats")] public float jumpHeight;
    public int maxAirJumps;
    [Range(0f, 0.3f)] public float coyoteTime;

    [Header("Gravity Stats")] public float upwardMovementMultiplier;
    public float downwardMovementMultiplier;

    [Header("Wall Jump Stats")] public float wallSlideMaxSpeed;
    [Tooltip("X Input == WallDirection; Wall Direction")]
    public Vector2 wallJumpClimb;
    [Tooltip("X Input == 0; No direction")] 
    public Vector2 wallJumpBounce;
    [Tooltip("X Input != WallDirection; Leaving the Wall")] 
    public Vector2 wallJumpLeap;
    public float wallJumpStickTime;
    
    [Header("Combat Stats")] public int health;
    public int attackDamage;
    public float attackRange;
    public float attackRate;
    public float attackKnockback;

    [Header("Upgrade Items Collected: ")] public bool doubleJumpUpgrader;
    public bool wallJumpUpgrader;
    public bool dashUpgrader;

}

