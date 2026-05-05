using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private float delay = 3f;
    [SerializeField] private float damageRadius = 20f;
    [SerializeField] private float explosionForce = 1200f;

    float countdown;
    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType { 
        Grenade 
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    
    
    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GranadeEffect();
                break;
        }
    }

    private void GranadeEffect()
    {
        // Visual effect
        GameObject explosionEffect = GlobalReferences.Instance.granadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        // Sound effect
        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.grenadeThrowSound);

        // Physical effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            //Add damage to nearby objects here if they have a health component
        }
    }
}
