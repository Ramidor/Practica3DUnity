using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;


    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unAtiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject middleDot;

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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponType)}";

            Weapon.WeaponType model = activeWeapon.thisWeaponType;
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if(unActiveWeapon)
            {
                unAtiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponType);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unAtiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.grenades <= 0)
        {
            lethalUI.sprite = greySlot;
        }
    }


    private Sprite GetWeaponSprite(Weapon.WeaponType model)
    {
        switch (model)
        {
            case Weapon.WeaponType.Pistol:
                return Resources.Load<GameObject>("PistolM1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponType.Rifle:
                return Resources.Load<GameObject>("RifleAK74_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponType model)
    {
        switch (model)
        {
            case Weapon.WeaponType.Pistol:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponType.Rifle:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponsSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    internal void UpdateThrowables(Throwable.ThrowableType throwable)
    {
        switch (throwable)
        {
            case Throwable.ThrowableType.Grenade:
                lethalAmountUI.text = $"{WeaponManager.Instance.grenades}";
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}