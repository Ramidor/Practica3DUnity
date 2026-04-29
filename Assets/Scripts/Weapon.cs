using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;

    // Shooting control variables
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;

    // Burst
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    // Spread
    public float spreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float bulletPrefabLifeTime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;
    
    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
    }


    void Update()
    {
       if(currentShootingMode == ShootingMode.Auto)
        {
            // Hold to shoot, release to stop shooting
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }else if(currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // Press to shoot, release to stop shooting
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (isShooting && readyToShoot)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {   
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        // Instantiate the bullet and apply force to it
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        // Poiting the bullet in the shooting direction
        bullet.transform.forward = shootingDirection;

        // shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        // Destroy the bullet after a certain time to prevent memory leaks
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        // Check if we are in burst mode and if we have bullets left in the burst
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if(currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the center of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            // If the ray hits something
            targetPoint = hit.point;
        }
        else
        {
            // If the ray doesn't hit anything
            targetPoint = ray.GetPoint(100); 
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        // return the direction with added spread
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}
