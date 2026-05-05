using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name);

            CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit " + collision.gameObject.name);

            CreateBulletImpactEffect(collision);

            Destroy(gameObject);
        }
<<<<<<< Updated upstream
=======
        if (collision.gameObject.CompareTag("Zombie"))
        {
            print("hit " + collision.gameObject.name);
            collision.gameObject.GetComponent<Enemy>().RecibirDaño(bulletDamage);
            CreateBloodEffect(collision);

            Destroy(gameObject);
        }
>>>>>>> Stashed changes
    }

    private void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objectWeHit.transform);

        
    }
<<<<<<< Updated upstream
    
}
=======
    private void CreateBloodEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(GlobalReferences.Instance.sangreEffect, contact.point, Quaternion.LookRotation(contact.normal));

        bloodSprayPrefab.transform.SetParent(objectWeHit.transform);
    }
}
>>>>>>> Stashed changes
