using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public Weapon hoverWeapon = null;
    public AmmoBox hoverAmmoBox = null;

    public Throwable hoverThrowable = null;

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
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent<Weapon>() && hitObject.GetComponent<Weapon>().isActiveWeapon == false)
            {
                hoverWeapon = hitObject.gameObject.GetComponent<Weapon>();
                hoverWeapon.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                }
            }else
            {
                if (hoverWeapon)
                {
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                }
            }

            //AmmobBox
            if (hitObject.GetComponent<AmmoBox>())
            {
                hoverAmmoBox = hitObject.gameObject.GetComponent<AmmoBox>();
                hoverAmmoBox.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpAmmo(hoverAmmoBox);
                    Destroy(hitObject.gameObject);
                }
            }else
            {
                if (hoverAmmoBox)
                {
                    hoverAmmoBox.GetComponent<Outline>().enabled = false;
                }
            }

            //Throwable
            if (hitObject.GetComponent<Throwable>())
            {
                hoverThrowable = hitObject.gameObject.GetComponent<Throwable>();
                hoverThrowable.GetComponent<Outline>().enabled = true;
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    WeaponManager.Instance.PickUpThrowable(hoverThrowable);
                }
            }else
            {
                if (hoverThrowable)
                {
                    hoverThrowable.GetComponent<Outline>().enabled = false;
                }
            }


                
    }
}

}
