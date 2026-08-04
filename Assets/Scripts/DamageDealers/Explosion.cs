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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionRB = collision.gameObject.GetComponent<Rigidbody2D>();
        if (collisionRB == null ) 
            return;
        var vectorToPush = collision.transform.position - transform.position;
        collisionRB.AddForce(vectorToPush * _force);
        collisionRB.AddTorque(_force/10);
    }
}
