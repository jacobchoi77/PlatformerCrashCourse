using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeController : MonoBehaviour{
    public DetectionZone biteDetectionZone;
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public List<Transform> waypoints;
    public Collider2D deathCollider;

    private Animator _animator;
    private Rigidbody2D _rb;
    private Damageable _damageable;
    private int _waypointNum;
    private Transform _nextWaypoint;

    public bool hasTarget;

    public bool HasTarget{
        get => hasTarget;
        set{
            hasTarget = value;
            _animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private bool CanMove => _animator.GetBool(AnimationStrings.canMove);

    private void Awake(){
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
    }

    private void Start(){
        _nextWaypoint = waypoints[_waypointNum];
    }

    void Update(){
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate(){
        if (!_damageable.IsAlive) return;
        if (CanMove){
            Flight();
        }
        else{
            _rb.velocity = Vector3.zero;
        }
    }

    private void Flight(){
        var directionToWaypoint = (_nextWaypoint.position - transform.position).normalized;
        var distance = Vector2.Distance(_nextWaypoint.position, transform.position);
        _rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        if (!(distance <= waypointReachedDistance)) return;
        _waypointNum++;
        if (_waypointNum >= waypoints.Count){
            _waypointNum = 0;
        }

        _nextWaypoint = waypoints[_waypointNum];
    }

    private void UpdateDirection(){
        var localScale = transform.localScale;
        if (transform.localScale.x > 0 && _rb.velocity.x < 0 || transform.localScale.x < 0 && _rb.velocity.x > 0){
            transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
        }
    }

    public void OnDeath(){
        _rb.gravityScale = 2f;
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        deathCollider.enabled = true;
    }
}