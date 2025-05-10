using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFSM : MonoBehaviour, IDamagable
{
    private enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die,
    }

    private MonsterState _state = MonsterState.Idle;

    public float idleDelayTime = 2f;

    private float _currentTime = 0;

    public float moveSpeed = 1f;

    public float attackRange = 3f;
    public float attackDelayTime = 1f;
    private int _attackDamage = 1;

    [SerializeField] private float hp = 3f;

    private Transform _tower;
    private NavMeshAgent _agent;
    private MeshRenderer _meshRenderer;

    private Transform _explosion;
    private ParticleSystem _expEffect;
    private AudioSource _expAudio;

    private void Start()
    {
        _tower = GameObject.FindWithTag("Tower").transform;

        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;

        _agent.speed = moveSpeed;

        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        _explosion = GameObject.Find("Explosion").transform;
        _expEffect = _explosion.GetComponent<ParticleSystem>();
        _expAudio = _explosion.GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        // Debug.Log($"Current State : {_state}");
        switch (_state)
        {
            case MonsterState.Idle :
                Idle();
                break;
            
            case MonsterState.Move :
                Move();
                break;
            
            case MonsterState.Attack :
                Attack();
                break;
            
            case MonsterState.Damage :
                // Damage();
                break;
            
            case MonsterState.Die :
                Die();
                break;
        }
    }

    private void Idle()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > idleDelayTime)
        {
            _state = MonsterState.Move;
            _agent.enabled = true;
        }
    }

    private void Move()
    {
        _agent.SetDestination(_tower.position);

        if (Vector3.Distance(transform.position, _tower.position) < attackRange)
        {
            _state = MonsterState.Attack;
            _agent.enabled = false;
        }
    }

    private void Attack()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > attackDelayTime)
        {
            _currentTime = 0f;

            Tower.Instance.CurrentHp -= _attackDamage;
        }
    }

    public void DamageAction(int damage, Vector3 hitPoint, Vector3 normal)
    {
        hp--;

        if (hp <= 0)
        {
            _explosion.position = transform.position;
            _expEffect.Play();
            _expAudio.Play();

            Destroy(gameObject);
            
            return;
        }
        
        _state = MonsterState.Damage;

        StopAllCoroutines();
        StartCoroutine(Damage());
    }

    private IEnumerator Damage()
    {
        _agent.enabled = false;

        Color originColor = _meshRenderer.material.color;

        _meshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _meshRenderer.material.color = originColor;

        _state = MonsterState.Idle;

        _currentTime = 0;
    }

    private void Die()
    {
        
    }
}
