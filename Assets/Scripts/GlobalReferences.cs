using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; private set; }

    public GameObject bulletImpactEffectPrefab;
    public TextMeshProUGUI ammoCounterText;

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

}
