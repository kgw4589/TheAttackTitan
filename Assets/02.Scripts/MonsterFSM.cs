using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MonsterFSM : MonoBehaviour, ITitan
{
    private enum MonsterState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Rest,
        Die,
    }

    private MonsterState _state = MonsterState.Idle;

    public MonsterStatusScriptable monsterStatus;

    private float _currentHp;

    private float _currentTime = 0;

    private Transform _tower;
    private NavMeshAgent _agent;
    
    private Transform _explosion;
    private ParticleSystem _expEffect;
    private AudioSource _expAudio;

    private void Start()
    {
        _tower = GameObject.FindWithTag("Tower").transform;

        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;

        _agent.speed = monsterStatus.moveSpeed;

        _explosion = GameObject.Find("Explosion").transform;
        _expEffect = _explosion.GetComponent<ParticleSystem>();
        _expAudio = _explosion.GetComponent<AudioSource>();

        _currentHp = monsterStatus.maxHp;
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
            
            case MonsterState.Rest :
                break;
            
            case MonsterState.Die :
                Die();
                break;
        }
    }

    private void Idle()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > monsterStatus.idleDelayTime)
        {
            _state = MonsterState.Move;
            _agent.enabled = true;
        }
    }

    private void Move()
    {
        _agent.SetDestination(_tower.position);

        if (Vector3.Distance(transform.position, _tower.position) < monsterStatus.attackRange)
        {
            _state = MonsterState.Attack;
            _agent.enabled = false;
        }
    }

    private void Attack()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime > monsterStatus.attackDelayTime)
        {
            _currentTime = 0f;

            Tower.Instance.CurrentHp -= monsterStatus.attackDamage;
        }
    }
    
    public void ScratchBody()
    {
        _currentHp--;

        if (_currentHp <= 0)
        {
            _state = MonsterState.Rest;

            StartCoroutine(Rest());
            
            return;
        }
        
        _state = MonsterState.Damage;

        StopAllCoroutines();
        StartCoroutine(Damage());
    }
    
    private IEnumerator Damage()
    {
        _agent.enabled = false;
        
        yield return new WaitForSeconds(0.1f);

        _state = MonsterState.Idle;

        _currentTime = 0;
    }

    private IEnumerator Rest()
    {
        _agent.enabled = false;

        yield return new WaitForSeconds(monsterStatus.restTime);

        _currentHp = monsterStatus.maxHp;
        _agent.enabled = true;

        _state = MonsterState.Idle;
    }

    public void SliceNeck()
    {
        _currentHp = 0;

        _state = MonsterState.Die;
    }


    private void Die()
    {
        _explosion.position = transform.position;
        _expEffect.Play();
        _expAudio.Play();

        Destroy(gameObject);
    }
}
