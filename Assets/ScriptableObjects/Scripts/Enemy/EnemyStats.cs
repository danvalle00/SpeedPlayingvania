using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    public int health;
    
    public Sprite sprite;
    public Animator animatorController;
    
}
