using UnityEngine;

public class HealthPickup : MonoBehaviour{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    private AudioSource pickupSource;

    private void Awake(){
        pickupSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        var damageable = other.GetComponent<Damageable>();
        if (damageable && damageable.Health < damageable.MaxHealth){
            var wasHealed = damageable.Heal(healthRestore);
            if (wasHealed){
                if (pickupSource)
                    AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
                Destroy(gameObject);
            }
        }
    }

    private void Update(){
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }
}