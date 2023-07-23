
using System;
using UnityEngine;

public class DashUpgrader : MonoBehaviour
{
    [SerializeField] private PlayerScriptable playerScriptable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerScriptable.dashUpgrader = true;
            gameObject.SetActive(false);

        }
    }

}
