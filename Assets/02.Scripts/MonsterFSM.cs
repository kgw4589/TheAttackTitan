using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MonsterFSM : MonoBehaviour, ITitan, IGrabable
{
    private enum MonsterState
    {
        None,
        Idle,
        Move,
        Attack,
        Damage,
        Rest,
        Grabbed,
        Die,
    }

    private MonsterState _state = MonsterState.Idle;

    public MonsterStatusScriptable monsterStatus;

    public GameObject originObject;
    public GameObject ragDollObject;

    private int _leftLife = 1;
    private float _currentHp;

    private float _currentTime = 0;

    private Transform _tower;
    private NavMeshAgent _agent;
    
    private Transform _explosion;
    private AudioSource _audioSource;
    private Animator _animator;

    private void Start()
    {
        GameManager.Instance.gameOverAction += GameOverAction;
        
        _tower = GameObject.FindWithTag("Tower").transform;

        _animator = GetComponent<Animator>();
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.enabled = false;

        _agent.speed = monsterStatus.moveSpeed;

        _explosion = GameObject.Find("Explosion").transform;
        _audioSource = _explosion.GetComponent<AudioSource>();

        _leftLife = monsterStatus.maxLife;
        _currentHp = monsterStatus.maxHp;
    }
    
    private void Update()
    {
        switch (_state)
        {
            case MonsterState.Idle:
                Idle();
                break;

            case MonsterState.Move:
                Move();
                break;

            case MonsterState.Attack:
                Attack();
                break;
            
            case MonsterState.Damage:
            case MonsterState.Rest:
            case MonsterState.Grabbed:
            case MonsterState.Die:
            case MonsterState.None:
                return;
        }
    }

    private void Idle()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > monsterStatus.idleDelayTime)
        {
            _state = MonsterState.Move;
            _animator.SetTrigger("IdleToMove");

            _agent.enabled = true;
        }
    }

    private void Move()
    {
        _agent.isStopped = false;
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

    public void ScratchBody(Vector3 hitPoint, Vector3 normal)
    {
        if (_state is MonsterState.None or MonsterState.Die or MonsterState.Rest or MonsterState.Grabbed)
        {
            return;
        }

        _currentHp--;

        if (_currentHp <= 0)
        {
            _state = MonsterState.Rest;
            StopAllCoroutines();
            StartCoroutine(Rest());
            return;
        }

        _state = MonsterState.Damage;
        StopAllCoroutines();
        StartCoroutine(Damage(hitPoint, normal));
    }

    private IEnumerator Damage(Vector3 hitPoint, Vector3 normal)
    {
        _agent.enabled = false;
        _audioSource.PlayOneShot(monsterStatus.damagedAudio);

        StartCoroutine(PlayBloodVfx(monsterStatus.ccTime, hitPoint, normal));

        _animator.SetTrigger("MoveToIdle");

        yield return new WaitForSeconds(monsterStatus.ccTime);

        _state = MonsterState.Move;
        _animator.SetTrigger("IdleToMove");

        _currentTime = 0;
    }

    private IEnumerator Rest()
    {
        _state = MonsterState.Rest;

        _animator.SetTrigger("Rest");
        _agent.isStopped = true;

        yield return new WaitForSeconds(monsterStatus.restTime);

        _currentHp = monsterStatus.maxHp;
        _agent.isStopped = false;

        _currentTime = 0f;
        _state = MonsterState.Idle;
        _animator.SetTrigger("RestToIdle");
    }

    public void Grabbed()
    {
        _state = MonsterState.Grabbed;

        originObject.SetActive(false);
        ragDollObject.SetActive(true);

        _animator.enabled = false;
        _agent.enabled = false;
    }

    public void UnGrabbed()
    {
        originObject.SetActive(true);
        ragDollObject.SetActive(false);

        _agent.enabled = true;
        _animator.enabled = true;

        _state = MonsterState.Idle;
    }

    public void SliceNeck(Vector3 hitPoint, Vector3 normal)
    {
        _audioSource.PlayOneShot(monsterStatus.neckSliceAudio);

        _currentHp = 0;

        if (--_leftLife > 0)
        {
            _state = MonsterState.Rest;
            StopAllCoroutines();
            StartCoroutine(Rest());
            return;
        }
        
        StopAllCoroutines();
        StartCoroutine(PlayBloodVfx(monsterStatus.dieLeftTime, hitPoint, normal));
        StartCoroutine(Die());
    }

    private IEnumerator PlayBloodVfx(float duration, Vector3 hitPoint, Vector3 normal)
    {
        GameObject blood = Instantiate(monsterStatus.bloodVfx, transform);
        blood.SetActive(true);
        blood.transform.position = hitPoint;
        blood.transform.forward = normal;

        yield return new WaitForSeconds(duration);
        
        Destroy(blood);
    }

    private IEnumerator Die()
    {
        _state = MonsterState.Die;

        GameManager.Instance.gameOverAction -= GameOverAction;

        _agent.isStopped = true;

        _audioSource.PlayOneShot(monsterStatus.dieAudio);
        _animator.SetTrigger("Die");

        yield return new WaitForSeconds(monsterStatus.dieLeftTime);

        GameManager.Instance.LeftTitan -= 1;
        Destroy(gameObject);
    }

    private void GameOverAction()
    {
        _state = MonsterState.None;
        StopAllCoroutines();
        _agent.enabled = false;
        this.enabled = false;
    }
}
