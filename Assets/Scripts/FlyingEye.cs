using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour{
    public DetectionZone biteDetectionZone;
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public List<Transform> waypoints;
    public Collider2D deathCollider;

    private Animator animator;
    private Rigidbody2D rb;
    private Damageable _damageable;
    private int waypointNum = 0;
    private Transform nextWaypoint;

    public bool hasTarget = false;

    public bool HasTarget{
        get => hasTarget;
        set{
            hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    private void Awake(){
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        _damageable = GetComponent<Damageable>();
    }

    private void Start(){
        nextWaypoint = waypoints[waypointNum];
    }

    void Update(){
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate(){
        if (_damageable.IsAlive){
            if (CanMove){
                Flight();
            }
            else{
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight(){
        var directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        var distance = Vector2.Distance(nextWaypoint.position, transform.position);
        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        if (distance <= waypointReachedDistance){
            waypointNum++;
            if (waypointNum >= waypoints.Count){
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection(){
        var localScale = transform.localScale;
        if (transform.localScale.x > 0 && rb.velocity.x < 0 || transform.localScale.x < 0 && rb.velocity.x > 0){
            transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
        }
    }

    public void OnDeath(){
        rb.gravityScale = 2f;
        rb.velocity = new Vector2(0, rb.velocity.y);
        deathCollider.enabled = true;
    }
}