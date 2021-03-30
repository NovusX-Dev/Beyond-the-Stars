using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossStates { Idle, Attack, Die }
    BossStates _states;

    [SerializeField] private int _health = 5;
    [SerializeField] Vector3 _stopPosition;
    [SerializeField] GameObject _shields;
    [SerializeField] GameObject _explosion;
    [SerializeField] GameObject _laserContainer;
    [SerializeField] float _nextFire = 0.35f;
    
    [Header("Audio")]
    [SerializeField] AudioClip _bossEntering;
    [SerializeField] AudioClip _bossMusic;
    [SerializeField] AudioClip _bossHurt;
    [SerializeField] AudioClip _bossDeath;

    
    private bool _hasShields = true;
    private bool _enteringGame = true;
    private float _canFire = -1f;

    Animator _anim;
    Player _player;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        AudioSource.PlayClipAtPoint(_bossEntering, Camera.main.transform.position);
        AudioManager.Instance.BossMusic(_bossMusic, 3.25f);
        _states = BossStates.Idle;
        _player.ChangeBossAction(true);
    }

    void Update()
    {
        if (transform.position.y > _stopPosition.y)
        {
            transform.Translate(Vector3.down * 3 * Time.deltaTime);
        }

        if (_enteringGame && transform.position.y <= _stopPosition.y)
        {
            _states = BossAI.BossStates.Attack;
            _enteringGame = false;
            _player.ChangeBossAction(false);
        }

        switch (_states)
        {
            case BossStates.Idle:
                ShieldState(true);
                break;
            case BossStates.Attack:
                ShieldState(false);
                Invoke("AttackPlayer", 1.5f);
                break;
            case BossStates.Die:
                StartCoroutine(BossDeathSequence());
                break;
        }
    }
    private void AttackPlayer()
    {
        if (Time.time > _canFire)
        {
            _canFire = Time.time + _nextFire;
            FireLasers();
        }
    }

    protected void FireLasers()
    {
        var laserBossContainer = Instantiate(_laserContainer, transform.position, Quaternion.identity);
        Laser[] lasers = laserBossContainer.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
            lasers[i].AssignEnemyLaser();
        }
    }

    private void DamageBoss()
    {
        if (_hasShields)
        {
            _shields.GetComponent<EnemyShieldDurability>().DamageDurability();
        }
        else
        {
            _health--;
            AudioSource.PlayClipAtPoint(_bossHurt, Camera.main.transform.position);

            _anim.SetTrigger("hurt");
            _canFire = -1;
            _states = BossStates.Idle;

            if (_health <= 0)
            {
                _anim.SetTrigger("death");
                AudioSource.PlayClipAtPoint(_bossDeath, Camera.main.transform.position);
                AudioManager.Instance.StopMusic();
                _player.ChangeBossAction(true);
                _states = BossStates.Die;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") && !other.GetComponent<Laser>().GetIsEnemyLaser())
        {
            DamageBoss();
            Destroy(other.gameObject);
        }
    }

    IEnumerator BossDeathSequence()
    {
        yield return new WaitForSeconds(5.5f);

        _player.ChangeBossAction(false);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Instantiate(_explosion, transform.position + new Vector3(2,0,0), Quaternion.identity);
        Instantiate(_explosion, transform.position + new Vector3(-2, 0, 0), Quaternion.identity);
        Instantiate(_explosion, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        WaveManager.enemiesRemaining--;
        UIManager.Instance.UpdateEnemiesRemainingUI(WaveManager.enemiesRemaining);
        Destroy(gameObject);
    }

    public void ShieldState(bool isActive)
    {
        _hasShields = isActive;
        _shields.SetActive(isActive);
    }

    public void AttackState()
    {
        _states = BossStates.Attack;
    }
}
