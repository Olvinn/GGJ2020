using Controllers.Player;
using Data.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerData _data;
    [SerializeField] MovementController _player;
    [SerializeField] Transform _camera;

    float _rotationEffector;
    Vector3 _cameraDirection;

    void Start()
    {
        _player.Init(_data);
    }

    void Update()
    {
        Vector3 dir = Camera.main.worldToCameraMatrix * new Vector3(Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        _player.Move(dir);
        if (Input.GetAxis("Jump") > 0)
            _player.Jump();

        _rotationEffector = _player.GetCurrentSpeed() / _data.speed;
        _rotationEffector = Mathf.Clamp01(_rotationEffector);
    }
}
