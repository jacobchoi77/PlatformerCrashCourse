using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class Knight : MonoBehaviour{
    [FormerlySerializedAs("walkSpeed")]
    public float walkAcceleration = 50f;

    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;

    private Rigidbody2D rb;
    private TouchingDirections _touchingDirections;
    private Animator _animator;
    private Damageable _damageable;

    public enum WalkableDirection{
        Right,
        Left
    }

    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 _walkDirectionVector = Vector2.right;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    public WalkableDirection WalkDirection{
        get => _walkDirection;
        set{
            if (_walkDirection != value){
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
                _walkDirectionVector = value == WalkableDirection.Right ? Vector2.right :
                    value == WalkableDirection.Left ? Vector2.left : _walkDirectionVector;
            }

            _walkDirection = value;
        }
    }

    public bool hasTarget = false;

    public bool HasTarget{
        get => hasTarget;
        set{
            hasTarget = value;
            _animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove => _animator.GetBool(AnimationStrings.canMove);
    public bool IsAlive => _animator.GetBool(AnimationStrings.isAlive);

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
    }

    private void Update(){
        HasTarget = attackZone.detectedColliders.Count > 0;
        if (AttackCooldown > 0){
            AttackCooldown -= Time.deltaTime;
        }
    }

    public float AttackCooldown{
        get => _animator.GetFloat(AnimationStrings.attackCooldown);
        set => _animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }

    private void FixedUpdate(){
        if (_touchingDirections.IsGrounded && _touchingDirections.IsOnWall){
            FlipDirection();
        }

        if (IsAlive){
            if (!_damageable.LockVelocity){
                if (CanMove && _touchingDirections.IsGrounded)
                    rb.velocity = new Vector2(
                        Mathf.Clamp(rb.velocity.x + walkAcceleration * _walkDirectionVector.x * Time.fixedDeltaTime,
                            -maxSpeed, maxSpeed), rb.velocity.y);
                else
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
        else{
            rb.velocity = Vector2.zero;
        }
    }

    private void FlipDirection(){
        WalkDirection = WalkDirection == WalkableDirection.Right ? WalkableDirection.Left :
            WalkDirection == WalkableDirection.Left ? WalkableDirection.Right : WalkDirection;
    }

    public void OnHit(int damage, Vector2 knockback){
        rb.velocity = new Vector2(knockback.x, rb.velocity.y * knockback.y);
    }

    public void OnCliffDetected(){
        if (_touchingDirections.IsGrounded){
            FlipDirection();
        }
    }
}