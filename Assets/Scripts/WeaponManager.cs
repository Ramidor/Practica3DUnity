using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<GameObject> weaponsSlots;

    public GameObject activeWeaponSlot;
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
    }

    public void PickUpWeapon(GameObject pickedWeapon)
    {
        AddWeaponIntoActiveSlot(pickedWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedWeapon)
    {
        DropCurrentWeapon(pickedWeapon);

        pickedWeapon.transform.SetParent(activeWeaponSlot.transform);

        Weapon weapon = pickedWeapon.GetComponent<Weapon>();

        pickedWeapon.transform.localPosition = weapon.spawnPosition;
        pickedWeapon.transform.localEulerAngles = weapon.spawnRotation;

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickedWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedWeapon.transform.localRotation;
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
}
