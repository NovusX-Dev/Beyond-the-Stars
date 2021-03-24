using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldsDurability : MonoBehaviour
{
    [SerializeField] int durability = 3;
    [SerializeField] Color fullDurability, damaged, destroyed;

    private int _currentDurability;

    Player _player;
    SpriteRenderer _spriteR;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
        _spriteR = GetComponent<SpriteRenderer>();
    }

    private void Start()
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

        if(_currentDurability < 1)
        {
            _player.OnShieldPowerUp(false);
        }
    }
}
