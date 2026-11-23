using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Follower : MonoBehaviour
{
    private const float DestinationUpdateInterval = 0.1f;
    private const float DirectionThreshold = 0.04f;

    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _moveSpeedVariation = 0.2f;
    [SerializeField] private CharacterFacing _characterFacing;

    private NavMeshAgent _agent;
    private Transform _target;
    private float _nextDestinationUpdateTime;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.autoBraking = false;
    }

    private void OnDisable()
    {
        _target = null;

        if (_agent.isOnNavMesh)
        {
            _agent.ResetPath();
        }
    }

    public void Initialize(Transform target)
    {
        _target = target;
        _agent.speed = Mathf.Max(0f, _moveSpeed + Random.Range(-_moveSpeedVariation, _moveSpeedVariation));
        _agent.avoidancePriority = Random.Range(0, 100);
        _nextDestinationUpdateTime = 0f;
    }

    private void Update()
    {
        if (_target == null || _agent.isOnNavMesh == false)
        {
            return;
        }

        if (Time.time >= _nextDestinationUpdateTime)
        {
            _agent.SetDestination(_target.position);
            _nextDestinationUpdateTime = Time.time + DestinationUpdateInterval;
        }

        Vector2 direction = _agent.velocity;

        if (direction.sqrMagnitude > DirectionThreshold * DirectionThreshold)
        {
            _characterFacing.SetDirection(direction);
        }
    }
}