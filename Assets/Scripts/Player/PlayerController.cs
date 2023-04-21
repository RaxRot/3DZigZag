using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private BoxCollider _boxCollider;
    private bool _isMoveForward;

    [SerializeField] private float speed = 4f;

    [SerializeField] private float yDeadPosition = 1f;
    private bool _isPlayerDied;
    
    public Transform modelHolder;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        _isMoveForward = true;
    }

    private void FixedUpdate()
    {
      MoveCar();
    }

    private void Update()
    {
        CheckInput();
        
        CheckForDead();
    }

    private void CheckInput()
    {
        if (GameManager.Instance.CanPlay)
        {
            GameManager.Instance.ActivateTileSpawner();
            
            if (Input.GetMouseButtonDown(0))
            {
                _isMoveForward = !_isMoveForward;
            }
        }
    }

    private void MoveCar()
    {
        if (GameManager.Instance.CanPlay)
        {
            if (_isMoveForward)
            {
                _rb.velocity = new Vector3(0f, _rb.velocity.y, speed);
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                _rb.velocity = new Vector3(speed, _rb.velocity.y, 0f);
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            }
        }
    }

    private void CheckForDead()
    {
        if (transform.position.y<=yDeadPosition && !_isPlayerDied)
        {
            _isPlayerDied = true;
            _boxCollider.isTrigger = true;
            GameManager.Instance.PlayerDied();
        }
    }
}
