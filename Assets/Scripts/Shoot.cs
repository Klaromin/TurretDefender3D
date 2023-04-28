using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _guns;
    
    private GameObject _currentTarget;
    private ArriveHome _currentTargetCode;
    public TurretProperities TurretProperities;
    private Quaternion _bodyStartRotation;
    private Quaternion _gunStartRotation;
    private bool _isTurretAtCooldown = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("goob") && _currentTarget == null)
        {
            _currentTarget = other.gameObject;
            _currentTargetCode = _currentTarget.GetComponent<ArriveHome>();
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
        _gunStartRotation = _guns.localRotation;
    }
    
    private void Update()
    {
        if (_currentTarget == null)
        {
            _guns.localRotation = Quaternion.Slerp(_guns.localRotation, _gunStartRotation,
                Time.deltaTime * TurretProperities.TurnSpeed);
        
            _body.rotation = Quaternion.Slerp(_body.transform.rotation, 
                _bodyStartRotation, Time.deltaTime * TurretProperities.TurnSpeed);
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
                Quaternion.LookRotation(relativeTargetPosition - _guns.position), Time.deltaTime * TurretProperities.TurnSpeed);
        
            _body.rotation = Quaternion.Slerp(_body.transform.rotation, 
                Quaternion.LookRotation(aimAt - _body.transform.position), Time.deltaTime * TurretProperities.TurnSpeed);
           
            Vector3 dirToTarget = _currentTarget.transform.position - _guns.transform.position;

            if (Vector2.Angle(dirToTarget, _guns.transform.forward) < 10) //10 is the accuracy
            {
                if (Random.Range(0, 100) < TurretProperities.Accuracy)
                {
                    ShootTarget();
                }
            }
        }
    }

    private void Cooldown()
    {
        _isTurretAtCooldown = true;
    }

    void ShootTarget()
    {
        if (_currentTarget)
        {
            _currentTargetCode.Hit((int)TurretProperities.Damage);
            Invoke("Cooldown", TurretProperities.ReloadTime);
        }
    }
}