using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
   private Rigidbody _rb;
   private AudioSource _audioSource;

   [SerializeField] private float timeBetweenFall = 0.25f;
   [SerializeField] private float timeBetweenTurnOff = 5f;

   [SerializeField] private GameObject gem;
   

   private void Awake()
   {
      _rb = GetComponent<Rigidbody>();
      _audioSource = GetComponent<AudioSource>();
   }

   private void Start()
   {
      if (Random.Range(0,100)>90)
      {
         Vector3 temp = transform.position;
         temp.y += 2f;
         Instantiate(gem, temp, Quaternion.identity);
      }  
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag(TagManager.PLAYER_TAG))
      {
         StartFalling();
      }
   }

   private void StartFalling()
   {
      StartCoroutine(_StartFallingCo());
   }

   private IEnumerator _StartFallingCo()
   {
      yield return new WaitForSeconds(timeBetweenFall);
      _rb.isKinematic = false;
      _audioSource.Play();

      yield return new WaitForSeconds(timeBetweenTurnOff);
      gameObject.SetActive(false);
   }
   
}
