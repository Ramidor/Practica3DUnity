using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
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

    public GameObject muzzleEffect;
    private Animator animator;

    //Loading
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;


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
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize;
    }


    void Update()
    {   
        if(bulletsLeft <= 0 && isShooting)
        {
            SoundManager.Instance.emptySoundM1911.Play();
        }

        if(currentShootingMode == ShootingMode.Auto)
        {
            // Hold to shoot, release to stop shooting
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }else if(currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // Press to shoot, release to stop shooting
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
        {
            Reload();
        }
        
        // Auto reload when trying to shoot with no bullets left
        if(readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
        {
            //Reload();
        }

        if (isShooting && readyToShoot && bulletsLeft > 0)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        // Update the ammo counter UI
        GlobalReferences.Instance.ammoCounterText.text = $"{bulletsLeft/bulletsPerBurst} / {magazineSize/bulletsPerBurst}";
    }

    private void FireWeapon()
    {   
        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("recoil");

        SoundManager.Instance.shootingSoundM1911.Play();

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

    private void Reload()
    {
        SoundManager.Instance.reloadingSoundM1911.Play();
        isReloading = true;
        Invoke("FinishReloading", reloadTime);
    }

    private void FinishReloading()
    {
        bulletsLeft = magazineSize;
        isReloading = false;

    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        // Shooting from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
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
