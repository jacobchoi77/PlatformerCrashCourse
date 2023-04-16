using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour{
    public UnityEvent noCollidersRemain;
    public List<Collider2D> detectedColliders = new();
    private Collider2D _col;

    private void Awake(){
        _col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other){
        detectedColliders.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other){
        detectedColliders.Remove(other);
        if (detectedColliders.Count <= 0){
            noCollidersRemain.Invoke();
        }
    }
}