using Sirenix.Serialization;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class Spawn : MonoBehaviour
{

    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _homeLocation;
    [SerializeField] private float _spawnDelay;
    private float _spawnRate = 0.3f;
    private int _count;
    private int _maxCount = 10;
    
    void Start()
    {
        _count = 0;
        _maxCount = 10;
        InvokeRepeating("Spawner", _spawnDelay,_spawnRate);
    }

    private void Spawner()
    {
        if (IsAbleToSpawn())
        {
            // GameObject go = Instantiate(_prefab, transform.position,Quaternion.identity);
            GameObject go = Instantiate(_prefab, this.transform);
            go.GetComponent<ArriveHome>().SetDestination(_homeLocation);
            _count++;
        }
    }

    private bool IsAbleToSpawn()
    {
        if (_count < _maxCount)
        {
            return true;
        }

        return false;
    }

    public int GetMaxCount()
    {
        return _maxCount;
    }

}