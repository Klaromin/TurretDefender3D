using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertigoDemo.Helpers;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject[] _spawnPoints;
    private int _totalEnemies;
    void Start()
    {
        _spawnPoints = GameObject.FindGameObjectsWithTag("spawn");
        foreach (var sp in _spawnPoints)
        {
            _totalEnemies += sp.GetComponent<Spawn>().GetMaxCount();
        }
        Debug.Log(_totalEnemies);
    }


    void Update()
    {
        
    }

    public void RemoveEnemy()
    {
        _totalEnemies--;
        if (_totalEnemies <= 0)
        {
            Debug.Log("Level over");
        }
    }
}
