using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikaze : Enemy
{
    [Header("Kamikaze")] 
    [SerializeField]  float _speedMultiplier = 1.25f;

    private bool _canKamikaze = false;
    private Vector3 _lastPlayerPos = Vector3.zero;
    private Vector3 _direction = Vector3.zero;

    protected override void Update()
    {
        base.Update();

        if (transform.position.y <= -6f)
        {
            _canKamikaze = false;
        }
    }

    protected override void MoveDirection()
    {
        if (!_canKamikaze)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(_direction * (speed * _speedMultiplier)* Time.deltaTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Kamikaze Location"))
        {
            StartCoroutine(GetLastPlayerPosition());
            _canKamikaze = true;
        }
    }

    IEnumerator GetLastPlayerPosition()
    {
        _lastPlayerPos = _player.transform.position;
        _direction = _lastPlayerPos - transform.position;
        yield return null;
    }
}
