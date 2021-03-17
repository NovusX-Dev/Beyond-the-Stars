using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    Rigidbody _rb;
    Vector2 _movement;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        moveCharachter(_movement);
    }

    void moveCharachter(Vector2 direction)
    {
        _rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }
}
