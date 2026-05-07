using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<GameObject> weaponsSlots;
    public GameObject armaInicial;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
    public int totalShotgunAmmo = 0;

    [Header("Throwables")]
    public int grenades = 0;
    public float throwForce = 10f;
    public GameObject grenadePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0;
    public float maxForceMultiplier = 2f;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponsSlots[0];
        if (armaInicial != null)
        {
            PickUpWeapon(armaInicial);
        }

    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponsSlots)
        {
            if (weaponSlot != activeWeaponSlot)
            {
                weaponSlot.SetActive(false);
            }
            else
            {
                weaponSlot.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if (Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > maxForceMultiplier)
            {
                forceMultiplier = maxForceMultiplier;
            }
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (grenades > 0)
            {
                ThrowLethal();
            }

            forceMultiplier = 0;
        }


    }


    #region  || ---- Weapon ---- ||
    public void PickUpWeapon(GameObject pickedWeapon)
    {
        // 1. En lugar de robar el arma original, creamos un clon exacto de ella
        GameObject clonDelArma = Instantiate(pickedWeapon);
        clonDelArma.GetComponent<Outline>().enabled = false; // El clon empieza sin estar equipado, para que no haya problemas al soltar el arma original de la pared

        // Opcional: Le quitamos la etiqueta "(Clone)" del nombre para mantenerlo limpio
        clonDelArma.name = pickedWeapon.name;

        if (pickedWeapon.name == "M1911")
        {

            totalPistolAmmo += 40;
            if (totalPistolAmmo > 60) // El máximo de munición que puedes tener para una pistola es 40, así que si te pasas, lo ajustamos a ese máximo
            {
                totalPistolAmmo = 60;
            }
        }
        else if (pickedWeapon.name == "AK74")
        {
            totalRifleAmmo += 120;
            if (totalRifleAmmo > 180) // El máximo de munición que puedes tener para un rifle es 120, así que si te pasas, lo ajustamos a ese máximo
            {
                totalRifleAmmo = 180;
            }
        }
        else if (pickedWeapon.name == "Shotgun")
        {
            totalShotgunAmmo += 40;
            if (totalShotgunAmmo > 40) // El máximo de munición que puedes tener para una escopeta es 40, así que si te pasas, lo ajustamos a ese máximo
            {
                totalShotgunAmmo = 40;
            }
        }

        // 2. Nos equipamos el clon
        AddWeaponIntoActiveSlot(clonDelArma);
    }

    private void AddWeaponIntoActiveSlot(GameObject weaponToEquip)
    {
        // Soltamos el arma actual (ya no necesita saber de dónde viene la nueva)
        DropCurrentWeapon();

        weaponToEquip.transform.SetParent(activeWeaponSlot.transform);

        Weapon weapon = weaponToEquip.GetComponent<Weapon>();

        weaponToEquip.transform.localPosition = weapon.spawnPosition;
        weaponToEquip.transform.localEulerAngles = weapon.spawnRotation;

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon()
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            // Desvinculamos el arma vieja del jugador para que caiga al mundo real
            weaponToDrop.transform.SetParent(null);

            // La soltamos justo delante del jugador y un poquito elevada para que caiga al suelo de forma natural
            weaponToDrop.transform.position = transform.position + transform.forward * 1.5f + Vector3.up * 1f;
        }
    }

    public void SwitchActiveSlot(int slotIndex)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponsSlots[slotIndex];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }

    #endregion

    #region || ---- Throwables ---- ||
    public bool PickUpThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                return PickUpGrenade();
            default:
                return false;
        }
    }

    private bool PickUpGrenade()
    {
        if (grenades < 5) // Si tenemos hueco
        {
            // En el CoD, cuando compras en la pared te rellenan las granadas al máximo de golpe
            grenades = 5;

            // ¡Hemos borrado el Destroy() de aquí para que la pared no se quede vacía!

            HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
            return true; // Confirmamos que se ha realizado la compra
        }

        return false; // Rechazamos la compra porque ya tiene 5
    }
    private void ThrowLethal()
    {
        GameObject lethalPrefab = grenadePrefab;

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, throwableSpawn.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.VelocityChange);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        grenades -= 1;

        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    }

    #endregion

    #region || ---- Ammo ---- ||

    internal void PickUpAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;
            case AmmoBox.AmmoType.ShotgunAmmo:
                totalShotgunAmmo += ammo.ammoAmount;
                break;
        }
    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponType thisWeaponType)
    {
        switch (thisWeaponType)
        {
            case Weapon.WeaponType.Pistol:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponType.Rifle:
                totalRifleAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponType.Shotgun:
                totalShotgunAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(Weapon.WeaponType thisWeaponType)
    {
        switch (thisWeaponType)
        {
            case Weapon.WeaponType.Pistol:
                return totalPistolAmmo;
            case Weapon.WeaponType.Rifle:
                return totalRifleAmmo;
            case Weapon.WeaponType.Shotgun:
                return totalShotgunAmmo;
            default:
                return 0;
        }
    }

    #endregion

}