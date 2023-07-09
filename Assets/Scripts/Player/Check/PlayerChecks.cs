using System;
using UnityEngine;

public class PlayerChecks : MonoBehaviour
{
    private bool GroundCheck { get; set; }
    
    private bool OnWall { get; set; }
    public Vector2 ContactNormal { get; private set; }
    
    private PhysicsMaterial2D _material;
    private void OnCollisionEnter2D(Collision2D other)
    {
        EvaluateCollision(other);
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        EvaluateCollision(other);
        
    }
    private void OnCollisionExit2D()
    {
        GroundCheck = false;
        OnWall = false;
    }
    
    public void EvaluateCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
           ContactNormal = collision.GetContact(i).normal;
           GroundCheck |= ContactNormal.y >= 0.9f;
           OnWall |= MathF.Abs(ContactNormal.x) >= 0.9f;
        }
    }
    public bool GetGroundCheck()
    {
        return GroundCheck;
    }
    public bool GetWallCheck()
    {
        return OnWall;
    }
}
