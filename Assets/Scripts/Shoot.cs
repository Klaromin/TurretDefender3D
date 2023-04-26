using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private GameObject _currentTarget;
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _guns;
    private Quaternion _bodyStartRotation;
    private Quaternion _gunStartRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("goob") && _currentTarget == null)
        {
            _currentTarget = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == _currentTarget)
        _currentTarget = null;
    }

    private void Start()
    {
        _bodyStartRotation = _body.rotation;
        _gunStartRotation = _guns.rotation;
    }

    private void Update()
    {
        if (_currentTarget == null)
        {
            _guns.rotation = Quaternion.Slerp(_guns.rotation, _gunStartRotation,
                 Time.deltaTime);
        
            _body.rotation = Quaternion.Slerp(_body.transform.rotation, 
                _bodyStartRotation, Time.deltaTime);
        }
        else
        {
            Vector3 aimAt = new Vector3(_currentTarget.transform.position.x, _body.transform.position.y,
                _currentTarget.transform.position.z);

            float distToTarget = Vector3.Distance(aimAt, _guns.position);
            Vector3 relativeTargetPosition = _guns.position + (_guns.forward * distToTarget);
            relativeTargetPosition = new Vector3(relativeTargetPosition.x, _currentTarget.transform.position.y, 
                relativeTargetPosition.z);
        
        
        
            _guns.rotation = Quaternion.Slerp(_guns.rotation,
                Quaternion.LookRotation(relativeTargetPosition - _guns.position), Time.deltaTime);
        
            _body.rotation = Quaternion.Slerp(_body.transform.rotation, 
                Quaternion.LookRotation(aimAt - _body.transform.position), Time.deltaTime);
        }



    }
}
