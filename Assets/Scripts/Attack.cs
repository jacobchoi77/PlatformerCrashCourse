using UnityEngine;

public class Attack : MonoBehaviour{
    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D other){
        var damageable = other.GetComponent<Damageable>();
        if (damageable == null) return;
        var deliveredKnockback =
            transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
        damageable.Hit(attackDamage, deliveredKnockback);
    }
}