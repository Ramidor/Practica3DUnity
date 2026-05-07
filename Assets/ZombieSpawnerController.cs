using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieSpawnerController : MonoBehaviour
{
    public int initialZombiePerWave = 5;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float WaveCooldown = 10f;

    public bool inColdown;
    public float cooldownCounter = 0;

    public List<Enemy> currentZombiesAlive;
    public GameObject zombiePrefab;

    public TextMeshProUGUI waveOverText;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI waveCounterText;

    public void Start()
    {
        currentZombiesPerWave = initialZombiePerWave;
        StartNextWave();
        waveCounterText.text = "Ronda: " + currentWave.ToString();
        waveCounterText.gameObject.SetActive(true);
    }
    private void StartNextWave()
    {
        currentZombiesAlive.Clear();
        currentWave++;

        StartCoroutine(SpawnZombies());
    }
    private IEnumerator SpawnZombies()
    {
        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var newZombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
            Enemy enemyScript = newZombie.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                currentZombiesAlive.Add(enemyScript);
            }
            yield return new WaitForSeconds(spawnDelay);
        }
    }
    private void Update()
    {
        List<Enemy> zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombiesAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombiesAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        if (currentZombiesAlive.Count == 0 && !inColdown)
        {
            StartCoroutine(WaveCooldownRoutine());
        }
        if (inColdown)
        {
            cooldownCounter -= Time.deltaTime;

        }
        else
        {
            cooldownCounter = WaveCooldown;
        }
        cooldownText.text = "Next wave in: " + Mathf.Ceil(cooldownCounter).ToString() + "s";
        waveCounterText.text = "Ronda: " + currentWave.ToString();
    }
    private IEnumerator WaveCooldownRoutine()
    {
        inColdown = true;
        waveOverText.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(true);
        waveCounterText.gameObject.SetActive(false);
        yield return new WaitForSeconds(WaveCooldown);

        inColdown = false;
        waveOverText.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(false);
        waveCounterText.gameObject.SetActive(true);
        currentZombiesPerWave *= 2;
        StartNextWave();
    }
}
