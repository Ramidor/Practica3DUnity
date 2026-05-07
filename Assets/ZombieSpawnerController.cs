using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZombieSpawnerController : MonoBehaviour
{
    // --- LO NUEVO ---
    [Header("Spawn Points (Zonas Activas)")]
    public List<Transform> activeSpawnPoints = new List<Transform>();
    // ----------------

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
        // Seguridad: Si no hay puntos de spawn, avisamos y paramos
        if (activeSpawnPoints.Count == 0)
        {
            Debug.LogError("¡No hay puntos de spawn activos en la lista!");
            yield break;
        }

        for (int i = 0; i < currentZombiesPerWave; i++)
        {
            // --- LO NUEVO: Elegimos un punto activo al azar ---
            Transform randomSpawnPoint = activeSpawnPoints[Random.Range(0, activeSpawnPoints.Count)];
            
            // Le seguimos sumando tu offset para que no salgan exactamente fusionados si salen a la vez
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = randomSpawnPoint.position + spawnOffset;

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
        foreach (Enemy zombie in zombiesToRemove)
        {
            Destroy(zombie.gameObject);
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

    // --- LO NUEVO: La función que llamarán las puertas al abrirse ---
    public void UnlockNewSpawns(List<Transform> newSpawnsToAdd)
    {
        // Añadimos los spawns de la nueva zona a nuestra lista activa
        activeSpawnPoints.AddRange(newSpawnsToAdd);
        Debug.Log("¡Nueva zona desbloqueada! Spawns actuales: " + activeSpawnPoints.Count);
    }
}