using UnityEngine;

public class HpFragment : MonoBehaviour
{
    [SerializeField] private PlayerScriptable playerScriptable;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerScriptable.hpFragment++;
            gameObject.SetActive(false);
            if (playerScriptable.hpFragment >= 4)
            {
                playerScriptable.hpFragment = 0;
                playerScriptable.health++;
            }
        }
    }

}
