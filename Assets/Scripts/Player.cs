using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float xOffset = 0;

    void Start()
    {
        transform.position = Vector3.zero;
    }

    void Update()
    {
        CalculateMovement();

    }

    private void CalculateMovement()
    {
        float horiontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horiontalInput, verticalInput, 0);

        transform.Translate(direction * moveSpeed * Time.deltaTime);

        BindingMovement();
    }

    private void BindingMovement()
    {
        float xBound = 11.5f;
        float yBound = -4f;

        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, 0);
        }
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3((-xBound), transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yBound, 1), 0);
    }

    
}
