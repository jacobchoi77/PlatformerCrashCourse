using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour{
    private Vector2 _moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;

    private Rigidbody2D _rb;
    private Animator _animator;
    private TouchingDirections _touchingDirections;

    private Damageable _damageable;

    [SerializeField]
    private bool isMoving;

    private bool IsMoving{
        get => isMoving;
        set{
            isMoving = value;
            _animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool isRunning;

    private bool IsRunning{
        get => isRunning;
        set{
            isRunning = value;
            _animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private float CurrentMoveSpeed{
        get{
            if (CanMove){
                if (IsMoving && !_touchingDirections.IsOnWall){
                    if (_touchingDirections.IsGrounded){
                        return IsRunning ? runSpeed : walkSpeed;
                    }

                    return airWalkSpeed;
                }

                return 0;
            }

            return 0;
        }
    }


    private bool _isFacingRight = true;

    private bool IsFacingRight{
        get => _isFacingRight;
        set{
            if (_isFacingRight != value){
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    private bool CanMove => IsAlive && _animator.GetBool(AnimationStrings.canMove);

    private bool IsAlive => _animator.GetBool(AnimationStrings.isAlive);

    private void Awake(){
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate(){
        if (!_damageable.LockVelocity)
            _rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, _rb.velocity.y);

        _animator.SetFloat(AnimationStrings.yVelocity, _rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context){
        _moveInput = context.ReadValue<Vector2>();
        if (IsAlive){
            IsMoving = _moveInput != Vector2.zero;
            SetFacingDirection(_moveInput);
        }
        else{
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput){
        if (moveInput.x > 0 && !IsFacingRight){
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight){
            IsFacingRight = false;
        }
    }


    public void OnRun(InputAction.CallbackContext context){
        if (context.started){
            IsRunning = true;
        }
        else if (context.canceled){
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context){
        if (context.started && _touchingDirections.IsGrounded && CanMove){
            _animator.SetTrigger(AnimationStrings.jumpTrigger);
            _rb.velocity = new Vector2(_rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context){
        if (context.started){
            _animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    
    public void OnRangedAttack(InputAction.CallbackContext context){
        if (context.started){
            _animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback){
        _rb.velocity = new Vector2(knockback.x, _rb.velocity.y + knockback.y);
    }
}