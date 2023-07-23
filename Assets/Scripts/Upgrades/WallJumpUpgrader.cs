using UnityEngine;

public class WallJumpUpgrader : MonoBehaviour
{
    [SerializeField] private PlayerScriptable playerScriptable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            playerScriptable.wallJumpUpgrader = true;
            gameObject.SetActive(false);
        }
    }
}
