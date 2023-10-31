using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObject : MonoBehaviour
{
    [SerializeField] float destroydelay = 3f;

    CameraShake _camera;

    private void Awake()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraShake>();
    }

    void Start()
    {
        StartCoroutine(_camera.Shake(1f, 0.75f));
        Destroy(gameObject, destroydelay);
    }

   
}
