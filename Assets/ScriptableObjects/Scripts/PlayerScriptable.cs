using UnityEngine;

[CreateAssetMenu(fileName = "New PlayerStats", menuName = "ScriptableObjects/Player/PlayerStats")]
public class PlayerScriptable : ScriptableObject
{
    public int health;
    public int resource;
    public int attackDamage;
    
    // acesso por item
    public bool doubleJump;
    
}

