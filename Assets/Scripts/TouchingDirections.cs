using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D), typeof(Animator))]
public class TouchingDirections : MonoBehaviour{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;

    private CapsuleCollider2D _touchingCol;
    private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] _wallHits = new RaycastHit2D[5];
    private readonly RaycastHit2D[] _ceilingHits = new RaycastHit2D[5];
    private Animator _animator;

    [SerializeField]
    private bool isGrounded;

    public bool IsGrounded{
        get => isGrounded;
        private set{
            isGrounded = value;
            _animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    [SerializeField]
    private bool isOnWall;

    public bool IsOnWall{
        get => isOnWall;
        private set{
            isOnWall = value;
            _animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool isOnCeiling;

    public bool IsOnCeiling{
        get => isOnCeiling;
        set{
            isOnCeiling = value;
            _animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private Vector2 WallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    private void Awake(){
        _touchingCol = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate(){
        IsGrounded = _touchingCol.Cast(Vector2.down, castFilter, _groundHits, groundDistance) > 0;
        IsOnWall = _touchingCol.Cast(WallCheckDirection, castFilter, _wallHits, wallDistance) > 0;
        IsOnCeiling = _touchingCol.Cast(Vector2.up, castFilter, _ceilingHits, ceilingDistance) > 0;
    }
}