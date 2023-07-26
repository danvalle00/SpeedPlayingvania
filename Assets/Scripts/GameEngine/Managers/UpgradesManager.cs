using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private PlayerScriptable playerScriptable;
    public bool doubleJump;
    public bool wallJump;
    public bool dash;
    public bool resetHp;
    private void Update()
    {
        switch (doubleJump)
        {
            case true:
                playerScriptable.maxAirJumps = 1;
                playerScriptable.doubleJumpUpgrader = true;
                break;
            case false:
                playerScriptable.maxAirJumps = 0;
                playerScriptable.doubleJumpUpgrader = false;
                break;
        }
        
        playerScriptable.dashUpgrader = dash switch
        {
            true => true,
            false => false
        };

        playerScriptable.wallJumpUpgrader = wallJump switch
        {
            true => true,
            false => false
        };

        if (resetHp)
        {
            playerScriptable.health = 3;
        }
    }
            
}
