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

                // 1. Buscamos el arma que tenemos equipada (el HIJO del slot)
                Weapon armaEquipada = null;
                if (WeaponManager.Instance.activeWeaponSlot.transform.childCount > 0)
                {
                    armaEquipada = WeaponManager.Instance.activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
                }

                // 2. Si tenemos un arma en las manos, comparamos los nombres
                if (armaEquipada != null)
                {
                    if (armaEquipada.name != hoverWeapon.name)
                    {
                        // --- ES UN ARMA DISTINTA (COMPRAR ARMA NUEVA) ---
                        textoInteraccion.text = "Pulsar [E] para comprar " + hoverWeapon.name + " [Coste: " + hoverWeapon.cost.ToString() + "]";

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            hoverWeapon.GetComponent<Outline>().enabled = false;
                            WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                            PuntuacionManager.instance.GastarPuntos(hoverWeapon.cost);
                            hoverWeapon = null;
                        }
                    }
                    else
                    {
                        // --- ES LA MISMA ARMA (COMPRAR MUNICIÓN) ---
                        textoInteraccion.text = "Pulsar [E] para comprar munición de " + hoverWeapon.name + " [Coste: " + hoverWeapon.costAmmo.ToString() + "]";

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            // IMPORTANTE: Aquí deberías llamar a una función para dar munición, NO volver a coger el arma entera.
                            // Por ejemplo: WeaponManager.Instance.BuyAmmoFor(hoverWeapon.name);
                            if (WeaponManager.Instance.totalPistolAmmo < 60 && armaEquipada.name == "M1911")
                            {
                                hoverWeapon.GetComponent<Outline>().enabled = false;
                                WeaponManager.Instance.PickUpWeapon(hitObject.gameObject); // <-- Ojo, esto te equipa el arma de nuevo.
                                PuntuacionManager.instance.GastarPuntos(hoverWeapon.costAmmo);
                                hoverWeapon = null;
                            }
                            else if (WeaponManager.Instance.totalRifleAmmo < 180 && armaEquipada.name == "AK74")
                            {
                                hoverWeapon.GetComponent<Outline>().enabled = false;
                                WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                                PuntuacionManager.instance.GastarPuntos(hoverWeapon.costAmmo);
                                hoverWeapon = null;
                            }
                            else if (WeaponManager.Instance.totalShotgunAmmo < 40 && armaEquipada.name == "Shotgun")
                            {
                                hoverWeapon.GetComponent<Outline>().enabled = false;
                                WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                                PuntuacionManager.instance.GastarPuntos(hoverWeapon.costAmmo);
                                hoverWeapon = null;
                            }
                        }
                    }
                }
                else
                {
                    // --- ES UN ARMA DISTINTA (COMPRAR ARMA NUEVA) ---
                    textoInteraccion.text = "Pulsar [E] para comprar " + hoverWeapon.name + " [Coste: " + hoverWeapon.cost.ToString() + "]";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        hoverWeapon.GetComponent<Outline>().enabled = false;
                        WeaponManager.Instance.PickUpWeapon(hitObject.gameObject);
                        PuntuacionManager.instance.GastarPuntos(hoverWeapon.cost);
                        hoverWeapon = null;
                    }
                }

            }
            else
            {
                if (hoverWeapon)
                {
                    hoverWeapon.GetComponent<Outline>().enabled = false;
                    textoInteraccion.text = ""; // Limpiamos el texto al dejar de mirar
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
            //Throwable
            if (hitObject.GetComponent<Throwable>())
            {
                hoverThrowable = hitObject.gameObject.GetComponent<Throwable>();
                hoverThrowable.GetComponent<Outline>().enabled = true;

                // Mostramos el texto con el coste
                textoInteraccion.text = "Pulsar [E] para comprar Granadas [Coste: " + hoverThrowable.cost.ToString() + "]";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Si la función devuelve true (teníamos menos de 5 granadas)
                    if (WeaponManager.Instance.PickUpThrowable(hoverThrowable))
                    {
                        // Le cobramos los puntos
                        PuntuacionManager.instance.GastarPuntos(hoverThrowable.cost);
                    }
                    else
                    {
                        // Opcional: Podrías cambiar el textoInteraccion a "Ya tienes el máximo de granadas"
                        Debug.Log("Ya estás a tope de granadas, no gastas puntos.");
                    }
                }
            }
            else
            {
                if (hoverThrowable)
                {
                    hoverThrowable.GetComponent<Outline>().enabled = false;
                    textoInteraccion.text = ""; // Limpiamos el texto
                }
            }


        }
    }

}
