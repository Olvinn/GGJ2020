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
        Vector3 dir = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
        _player.Move(dir);
        if (Input.GetAxis("Jump") > 0)
            _player.Jump();

        _rotationEffector = _player.GetCurrentSpeed() / _data.speed;
        _rotationEffector = Mathf.Clamp01(_rotationEffector);

        //_player.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_inertia), _rotationEffector * Time.deltaTime * 10);

        _cameraDirection = dir;
        _cameraDirection.y = 0;
        _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, Quaternion.LookRotation(_cameraDirection, Vector3.up), _rotationEffector * Time.deltaTime);
    }
}
