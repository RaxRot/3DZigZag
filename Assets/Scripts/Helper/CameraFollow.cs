using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        _target = GameObject.FindWithTag(TagManager.PLAYER_TAG).transform;
    }

    private void LateUpdate()
    {
        if (_target)
        {
            transform.position = _target.position + offset;
        }
    }
}
