using Actions;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Damageable : MonoBehaviour{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    private Animator _animator;

    [SerializeField]
    private bool isInvincible;

    private float _timeSinceHit;
    public float invisibilityTimer = 0.25f;

    [SerializeField]
    private int maxHealth = 100;

    public int MaxHealth{
        get => maxHealth;
        set => maxHealth = value;
    }

    [SerializeField]
    private int health = 100;

    public int Health{
        get => health;
        private set{
            health = value;
            healthChanged?.Invoke(health, MaxHealth);
            if (health <= 0)
                IsAlive = false;
        }
    }

    [SerializeField]
    private bool isAlive = true;

    public bool IsAlive{
        get => isAlive;
        private set{
            isAlive = value;
            _animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set  " + value);
            if (value == false){
                damageableDeath?.Invoke();
            }
        }
    }

    public bool LockVelocity{
        get => _animator.GetBool(AnimationStrings.lockVelocity);
        private set => _animator.SetBool(AnimationStrings.lockVelocity, value);
    }

    private void Awake(){
        _animator = GetComponent<Animator>();
    }

    private void Update(){
        if (!isInvincible) return;
        if (_timeSinceHit > invisibilityTimer){
            isInvincible = false;
            _timeSinceHit = 0;
        }

        _timeSinceHit += Time.deltaTime;
    }

    public bool Hit(int damage, Vector2 knockback){
        if (!IsAlive || isInvincible) return false;
        Health -= damage;
        isInvincible = true;
        _animator.SetTrigger(AnimationStrings.hitTrigger);
        LockVelocity = true;
        damageableHit?.Invoke(damage, knockback);
        CharacterActions.Damaged.Invoke(gameObject, damage);
        return true;
    }

    public bool Heal(int healthRestore){
        if (!IsAlive || Health >= MaxHealth) return false;
        var maxHeal = Mathf.Max(MaxHealth - health, 0);
        var actualHeal = Mathf.Min(maxHeal, healthRestore);
        Health += actualHeal;
        CharacterActions.Healed(gameObject, actualHeal);
        return true;
    }
}