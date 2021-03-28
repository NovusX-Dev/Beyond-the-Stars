using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZigZag : Enemy
{
    [Header("Zig Zag")]
    [SerializeField] float xAngle = 3f;
    [SerializeField] float changeAngleTimer = 3f;

    protected override void Start()
    {
        base.Start();

        if (transform.position.x > 0)
        {
            xAngle *= -1;
        }
        else
        {
            xAngle *= 1;
        }

        StartCoroutine(ChangeAngleRoutine());
    }

    protected override void MoveDirection()
    {
        transform.Translate(new Vector3(xAngle, -1 * speed, 0) * Time.deltaTime);
    }

    IEnumerator ChangeAngleRoutine()
    {
        while (true)
        {
            xAngle *= 1;
            yield return new WaitForSeconds(changeAngleTimer);
            xAngle *= -1;
            yield return new WaitForSeconds(changeAngleTimer);
        }
    }
}
