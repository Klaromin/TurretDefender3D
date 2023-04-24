using UnityEngine;
using UnityEngine.AI;

public class ArriveHome : MonoBehaviour
{
    [SerializeField] private Transform _destination;
    
    private NavMeshAgent _ai;
    void Start()
    {
        _ai = GetComponent<NavMeshAgent>();
        _ai.SetDestination(_destination.position);
    }


    void Update()
    {
        if(_ai.remainingDistance < 0.3f && _ai.hasPath)
        {
            LevelManager.Instance.RemoveEnemy();
            _ai.ResetPath();
            Destroy(this.gameObject, 0.1f);
        }
        
    }

    public void SetDestination(Transform destination)
    {
        _destination = destination;
    }
}
