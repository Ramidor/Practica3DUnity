using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource shootingChannel;
    
    public AudioClip M1911Shot;
    public AudioClip AK74Shot;

    public AudioSource reloadingChannel;

    public AudioClip reloadingSoundM1911;
    public AudioClip reloadingSoundAK74;
    
    public AudioSource emptySoundM1911;


    public AudioSource throwablesChannel;
    public AudioClip grenadeThrowSound;

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

    public void PlayShootingSound(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                shootingChannel.PlayOneShot(M1911Shot);
                break;
            case WeaponType.Rifle:
                shootingChannel.PlayOneShot(AK74Shot);
                break;
        }
    }

    public void PlayReloadingSound(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Pistol:
                reloadingChannel.PlayOneShot(reloadingSoundM1911);
                break;
            case WeaponType.Rifle:
                reloadingChannel.PlayOneShot(reloadingSoundAK74);
                break;
        }
    }
}
