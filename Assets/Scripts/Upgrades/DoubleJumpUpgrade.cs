using UnityEngine;

public class DoubleJumpUpgrade : MonoBehaviour
{
    [SerializeField] private PlayerScriptable playerScriptable;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerScriptable.doubleJumpUpgrader = true;
            playerScriptable.maxAirJumps = 1;
            gameObject.SetActive(false);
        }
    }


}
