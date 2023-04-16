using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    [SerializeField]
    private int maxHealth = 100;

    private Animator _animator;

    public int MaxHealth{
        get => maxHealth;
        set => maxHealth = value;
    }

    [SerializeField]
    private int health = 100;

    public int Health{
        get => health;
        set{
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
        set{
            isAlive = value;
            _animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set  " + value);
            if (value == false){
                damageableDeath.Invoke();
            }
        }
    }

    public bool LockVelocity{
        get => _animator.GetBool(AnimationStrings.lockVelocity);
        set => _animator.SetBool(AnimationStrings.lockVelocity, value);
    }

    [SerializeField]
    private bool isInvincible;

    private float _timeSinceHit;
    public float invisibilityTimer = 0.25f;

    private void Awake(){
        _animator = GetComponent<Animator>();
    }

    private void Update(){
        if (isInvincible){
            if (_timeSinceHit > invisibilityTimer){
                isInvincible = false;
                _timeSinceHit = 0;
            }

            _timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback){
        if (IsAlive && !isInvincible){
            Health -= damage;
            isInvincible = true;
            _animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore){
        if (IsAlive && Health < MaxHealth){
            var maxHeal = Mathf.Max(MaxHealth - health, 0);
            var actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }

        return false;
    }
}