using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    public Weapon hoverWeapon = null;
    public AmmoBox hoverAmmoBox = null;

    public Throwable hoverThrowable = null;

    [Header("Interfaz de Usuario (UI)")]
    public TextMeshProUGUI textoInteraccion;

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
                if (WeaponManager.Instance.activeWeaponSlot.GetComponent<Weapon>() != null)
                {

                    if (WeaponManager.Instance.activeWeaponSlot.GetComponent<Weapon>().name != hitObject.GetComponent<Weapon>().name)
                    {
                        textoInteraccion.text = "Pulsar [E] para abrir comprar " + hitObject.GetComponent<Weapon>().name + " [Coste: " + hitObject.GetComponent<Weapon>().cost.ToString() + "]";
                    }
                    else
                    {
                        textoInteraccion.text = "Pulsar [E] para comprar municion" + hitObject.GetComponent<Weapon>().name + " [Coste: " + hitObject.GetComponent<Weapon>().costAmmo.ToString() + "]";
                    }
                }
                else
                {
                    textoInteraccion.text = "Pulsar [E] para abrir comprar " + hitObject.GetComponent<Weapon>().name + " [Coste: " + hitObject.GetComponent<Weapon>().cost.ToString() + "]";
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                    if (WeaponManager.Instance.activeWeaponSlot.GetComponent<Weapon>() != null)
                    {
                        if (WeaponManager.Instance.activeWeaponSlot.GetComponent<Weapon>().name != hitObject.GetComponent<Weapon>().name)
                        {
                            PuntuacionManager.instance.GastarPuntos(hitObject.GetComponent<Weapon>().cost);
                        }
                        else
                        {
                            PuntuacionManager.instance.GastarPuntos(hitObject.GetComponent<Weapon>().costAmmo);
                        }
                    }
                    else
                    {
                        PuntuacionManager.instance.GastarPuntos(hitObject.GetComponent<Weapon>().cost);
                    }

                }

            }
            else
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

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpAmmo(hoverAmmoBox);
                    Destroy(hitObject.gameObject);
                }
            }
            else
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

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.PickUpThrowable(hoverThrowable);
                }
            }
            else
            {
                if (hoverThrowable)
                {
                    hoverThrowable.GetComponent<Outline>().enabled = false;
                }
            }



        }
    }

}
