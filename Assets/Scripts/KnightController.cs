using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class KnightController : MonoBehaviour{
    public float walkAcceleration = 50f;

    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;

    private Rigidbody2D _rb;
    private TouchingDirections _touchingDirections;
    private Animator _animator;
    private Damageable _damageable;

    private enum WalkableDirection{
        Right,
        Left
    }

    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 _walkDirectionVector = Vector2.right;
    public DetectionZone attackZone;

    private WalkableDirection WalkDirection{
        get => _walkDirection;
        set{
            if (_walkDirection != value){
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
                _walkDirectionVector = value switch{
                    WalkableDirection.Right => Vector2.right,
                    WalkableDirection.Left => Vector2.left,
                    _ => _walkDirectionVector
                };
            }

            _walkDirection = value;
        }
    }

    public bool hasTarget;

    public bool HasTarget{
        get => hasTarget;
        set{
            hasTarget = value;
            _animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private bool CanMove => _animator.GetBool(AnimationStrings.canMove);
    private bool IsAlive => _animator.GetBool(AnimationStrings.isAlive);

    private void Awake(){
        _rb = GetComponent<Rigidbody2D>();
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

    private float AttackCooldown{
        get => _animator.GetFloat(AnimationStrings.attackCooldown);
        set => _animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }

    private void FixedUpdate(){
        if (_touchingDirections.IsGrounded && _touchingDirections.IsOnWall){
            FlipDirection();
        }

        if (IsAlive){
            if (_damageable.LockVelocity) return;
            if (CanMove && _touchingDirections.IsGrounded)
                _rb.velocity = new Vector2(
                    Mathf.Clamp(_rb.velocity.x + walkAcceleration * _walkDirectionVector.x * Time.fixedDeltaTime,
                        -maxSpeed, maxSpeed), _rb.velocity.y);
            else
                _rb.velocity = new Vector2(Mathf.Lerp(_rb.velocity.x, 0, walkStopRate), _rb.velocity.y);
        }
        else{
            _rb.velocity = Vector2.zero;
        }
    }

    private void FlipDirection(){
        WalkDirection = WalkDirection switch{
            WalkableDirection.Right => WalkableDirection.Left,
            WalkableDirection.Left => WalkableDirection.Right,
            _ => WalkDirection
        };
    }

    public void OnHit(int damage, Vector2 knockback){
        _rb.velocity = new Vector2(knockback.x, _rb.velocity.y * knockback.y);
    }

    public void OnCliffDetected(){
        if (_touchingDirections.IsGrounded){
            FlipDirection();
        }
    }
}