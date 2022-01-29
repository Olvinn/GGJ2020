using Controllers.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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
                    MinMaxCurve c = em.rateOverDistance;
                    c.constant = Mathf.Max(0, c.constant - Time.deltaTime);
                    em.rateOverDistance = c;
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
        h = Mathf.Abs(h);
        var shape = _smahPuf.shape;
        shape.radius = h * .1f;
        var main = _smahPuf.main;
        main.startSize = h * 0.1f;
        var emis = _smahPuf.emission;
        Burst b = emis.GetBurst(0);
        b.count = h * 2;
        emis.SetBurst(0, b);
        _smahPuf.Play();
    }
}
