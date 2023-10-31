using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldDurability : MonoBehaviour
{
    [SerializeField] int durability = 3;
    [SerializeField] Color fullDurability, damaged, destroyed;

    private int _currentDurability;

    BossAI _boss;
    SpriteRenderer _spriteR;

    private void Awake()
    {
        _boss = GetComponentInParent<BossAI>();
        _spriteR = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _currentDurability = durability;
    }

    private void Update()
    {
        switch (_currentDurability)
        {
            case 3:
                _spriteR.color = fullDurability;
                break;
            case 2:
                _spriteR.color = damaged;
                break;
            case 1:
                _spriteR.color = destroyed;
                break;
        }
    }

    public void DamageDurability()
    {
        _currentDurability--;

        if (_currentDurability < 1)
        {
            _boss.ShieldState(false);
            _boss.AttackState();
        }
    }
}
