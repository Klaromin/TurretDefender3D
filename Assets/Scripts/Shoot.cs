using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Transform _body;
    [SerializeField] private Transform _guns;
    [SerializeField] private AudioSource _firingSound;
    [SerializeField] private TurretProperities _turretPorperities;
    
    private GameObject _currentTarget;
    private ArriveHome _currentTargetCode;
    private Quaternion _bodyStartRotation;
    private Quaternion _gunStartRotation;
    private bool _isTurretAtCooldown = true;

    private void Start()
    {
        _bodyStartRotation = _body.rotation;
        _gunStartRotation = _guns.localRotation;
    }
    
    private void Update()
    {
        TargetingSystem();
    }

    private void TargetingSystem()
    {
        if (_currentTarget == null)
        {
            _guns.localRotation = Quaternion.Slerp(_guns.localRotation, _gunStartRotation,
                Time.deltaTime * _turretPorperities.TurnSpeed);
        
            _body.rotation = Quaternion.Slerp(_body.transform.rotation, 
                _bodyStartRotation, Time.deltaTime * _turretPorperities.TurnSpeed);
        }
        else
        {
            var currentTargetPosition = _currentTarget.transform.position;
            Vector3 aimAt = new Vector3(currentTargetPosition.x, _body.transform.position.y,
                currentTargetPosition.z);

            var gunPosition = _guns.position;
            float distToTarget = Vector3.Distance(aimAt, gunPosition);
            Vector3 relativeTargetPosition = gunPosition + (_guns.forward * distToTarget);
            relativeTargetPosition = new Vector3(relativeTargetPosition.x, currentTargetPosition.y, 
                relativeTargetPosition.z);

            _guns.rotation = Quaternion.Slerp(_guns.rotation,
                Quaternion.LookRotation(relativeTargetPosition - gunPosition), Time.deltaTime * _turretPorperities.TurnSpeed);
        
            _body.rotation = Quaternion.Slerp(_body.transform.rotation, 
                Quaternion.LookRotation(aimAt - _body.transform.position), Time.deltaTime * _turretPorperities.TurnSpeed);
           
            Vector3 dirToTarget = currentTargetPosition - _guns.transform.position;

            if (Vector2.Angle(dirToTarget, _guns.transform.forward) < _turretPorperities.AngleAccuracy) //10 is the accuracy
            {
                if (Random.Range(0, 100) < _turretPorperities.Accuracy)
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
        if (_currentTarget && _isTurretAtCooldown)
        {
            _currentTargetCode.Hit((int)_turretPorperities.Damage);
            _firingSound.Play();
            _isTurretAtCooldown = false;
            Invoke(nameof(Cooldown), _turretPorperities.ReloadTime);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("goob") )
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
}