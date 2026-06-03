using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreenEffect;

    public AudioSource playerChannel;
    public AudioClip playerHurtSound;
 
    public void RecibirDaño(int daño)
    {
        HP -= daño;
        if (HP <= 0)
        {
            print("Has muerto");
           
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GameOver");

        }
        else
        {
            // Protegido: si el AudioSource o el clip no están asignados, no peta
            // y la sangre se sigue mostrando igualmente.
            if (playerChannel != null && playerHurtSound != null)
            {
                playerChannel.PlayOneShot(playerHurtSound);
            }
            print("Has recibido " + daño + " puntos de daño. HP restante: " + HP);
            StartCoroutine(ShowBloodyScreenEffect());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            RecibirDaño(other.gameObject.GetComponent<ZombieHand>().daño);
        }
    }
    private IEnumerator ShowBloodyScreenEffect()
    {
        // --- LOGS DE DIAGNÓSTICO (quitar cuando funcione) ---
        Debug.Log("[Sangre] Coroutine arrancada. bloodyScreenEffect = " + (bloodyScreenEffect != null ? bloodyScreenEffect.name : "NULL"));

        // Por si el objeto estuviera desactivado, lo activamos
        bloodyScreenEffect.SetActive(true);

        var image = bloodyScreenEffect.GetComponent<Image>();
        Debug.Log("[Sangre] Image encontrada = " + (image != null) + " | activeInHierarchy = " + bloodyScreenEffect.activeInHierarchy);

        // Empezamos opaco (rojo de sangre visible)
        Color color = image.color;
        color.a = 1f;
        image.color = color;

        float duration = 3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            color.a = alpha;
            image.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos que queda totalmente transparente al final
        color.a = 0f;
        image.color = color;
    }

}
