using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekingLaser : MonoBehaviour
{
    //Code Programming also: Homing Projectile

    [SerializeField] float speed = 8f;
    [SerializeField] float rotationSpeed = 3f;

    private Enemy _currentTarget;

    private Vector3 _direction;

    Rigidbody2D _myRB;

    private void Awake()
    {
        _myRB = GetComponent<Rigidbody2D>();    
    }

    void Start()
    {
        
       if(_currentTarget == null)
       {
           GetCurrentTarget();
       }
    }

    

    void Update()
    {
       if(_currentTarget != null)
        {
            _direction = (_currentTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            var rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        _myRB.velocity = new Vector2(_direction.x * speed, _direction.y * speed);
    }

    private void GetCurrentTarget()
    {
        float distanceToClosestTarget = Mathf.Infinity;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
           
            if(!currentEnemy.GetIsDying())
            {
                float distanceToEnemy = (currentEnemy.transform.position - transform.position).sqrMagnitude;
                if (distanceToEnemy < distanceToClosestTarget)
                {
                    distanceToClosestTarget = distanceToEnemy;
                    _currentTarget = currentEnemy;
                }
            }
        }
    }
}
