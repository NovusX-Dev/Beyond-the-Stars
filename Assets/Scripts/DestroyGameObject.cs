using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] float destroydelay = 3f;

    void Start()
    {
        Destroy(gameObject, destroydelay);
    }

   
}
