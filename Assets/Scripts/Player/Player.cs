using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreenEffect;
 
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
        if (bloodyScreenEffect.activeInHierarchy == false)
        {
            bloodyScreenEffect.SetActive(true);
        }
        var image = bloodyScreenEffect.GetComponent<Image>();

        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
        float duration = 3f;
        float elapsed = 0f;
        while (elapsed < duration)
        {

            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (bloodyScreenEffect.activeInHierarchy)
        {

            bloodyScreenEffect.SetActive(false);
        }
    }

}
