using Controllers.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PufController : MonoBehaviour
{
    [SerializeField] ParticleSystem _ambientPuf, _smahPuf;
    [SerializeField] MovementController _player;

    void Start()
    {
        _player.onGrounded += Grounded;
    }

    void Update()
    {
        switch (_player.state)
        {
            case MovementState.Fall:
                {
                    var em = _ambientPuf.emission;
                    em.rateOverDistance = 0;
                    break;
                }
            default:
                {
                    var em = _ambientPuf.emission;
                    em.rateOverDistance = 2;
                    break;
                }
        }
    }

    void Grounded(float h)
    {
        _smahPuf.Play();
    }
}
