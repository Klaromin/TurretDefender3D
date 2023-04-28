using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ArriveHome : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Slider _healtBarPrefab;
    private Slider _healthBar;
    private NavMeshAgent _ai;
    private int _currentHealth;
    
    
    void Start()
    {
        _ai = GetComponent<NavMeshAgent>();
        _ai.SetDestination(_destination.position);
        _ai.speed = _enemyData.Speed;
        _currentHealth = _enemyData.MaxHealth;
        _healthBar = Instantiate(_healtBarPrefab, this.transform.position, Quaternion.identity);
        _healthBar.transform.SetParent(GameObject.Find("UI").transform);
        _healthBar.maxValue = _enemyData.MaxHealth;
        _healthBar.value = _enemyData.MaxHealth;
    }


    void Update()
    {
        
        if(_ai.remainingDistance < 0.3f && _ai.hasPath)
        {
            LevelManager.Instance.RemoveEnemy();
            _ai.ResetPath();
            Destroy(_healthBar.gameObject);
            Destroy(this.gameObject, 0.1f);
        }

        if (_healthBar)
        {
            _healthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.2f);
        }
        
    }

    public void Hit(int power)
    {
        if (_healthBar)
        {
            _healthBar.value -= power;
            if (_healthBar.value <= 0)
            {
                Destroy(_healthBar.gameObject);
                Destroy(gameObject);
            }
        }
    }
    

    public void SetDestination(Transform destination)
    {
        _destination = destination;
    }
}
