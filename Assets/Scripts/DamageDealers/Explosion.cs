using Assets.Scripts.MISC;
using UnityEngine;

public class Explosion : DamageDealer
{
    [SerializeField]
    protected float _force = 50;
    public void FinishExplode()
    {
        Destroy(gameObject);
    }

    public override void ReactToCollision(GameObject collision)
    {
        var collisionRB = collision.GetComponent<Rigidbody2D>();
        if (collisionRB == null ) 
            return;
        var vectorToPush = collision.transform.position - transform.position;
        collisionRB.AddForce(vectorToPush * _force);
    }
}
