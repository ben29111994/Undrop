using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Key : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Target"))
        {
            var key = PlayerPrefs.GetInt("Key");
            key++;
            PlayerPrefs.SetInt("Key", key);
            transform.DOMoveY(transform.position.y + 2, 0.3f);
            Destroy(gameObject, 0.31f);
        }
    }
}
