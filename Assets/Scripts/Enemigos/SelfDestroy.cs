using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
   public float destroyTime ;
    void Start()
    {
        StartCoroutine(DestroySelf(destroyTime));        
    }
    private IEnumerator DestroySelf(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
   
}
