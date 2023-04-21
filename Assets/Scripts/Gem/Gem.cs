using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] private GameObject gemFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            AudioManager.Instance.PlayJemSound();
            UIManager.Instance.AddCoin();
            Instantiate(gemFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
