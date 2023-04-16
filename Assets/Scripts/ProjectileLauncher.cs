using UnityEngine;

public class ProjectileLauncher : MonoBehaviour{
    public GameObject projectilePrefab;
    public Transform launchPoint;

    public void FireProjectile(){
        var projectile = Instantiate(projectilePrefab, launchPoint.transform.position,
            projectilePrefab.transform.rotation);
        var origScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            origScale.x * transform.localScale.x > 0 ? 1 : -1, origScale.y, origScale.z
        );
    }
}