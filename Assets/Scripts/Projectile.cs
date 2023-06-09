using UnityEngine;

public class Projectile : MonoBehaviour{
    public Vector2 moveSpeed = new Vector2(3f, 0);

    public int damage = 10;
    private Rigidbody2D _rb;
    public Vector2 knockback = new(0, 0);

    private void Awake(){
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        _rb.velocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D other){
        var damageable = other.GetComponent<Damageable>();
        if (damageable == null) return;
        var deliveredKnockback =
            transform.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
        var gotHit = damageable.Hit(damage, deliveredKnockback);
        if (gotHit)
            Destroy(gameObject);
    }
}