using Controllers.Player;
using Data.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerData _data;
    [SerializeField] MovementController _player;
    [SerializeField] Animator _anim;
    [SerializeField] Transform _camera;
    [SerializeField] CharacterAudioController _ac;

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

        _player.onGrounded += _ac.GroundedSound;
        _player.onJump += _ac.JumpSound;

        switch (_player.state)
        {
            default:
                _anim.SetBool("Moving", _player.GetCurrentSpeed() > 0);
                _anim.SetBool("Fall", false);
                _anim.SetBool("Jump", false);
                _anim.SetBool("Sliding", false);
                break;
            case MovementState.Slide:
                _anim.SetBool("Fall", false);
                _anim.SetBool("Jump", false);
                _anim.SetBool("Moving", _player.GetCurrentSpeed() > 0);
                _anim.SetBool("Sliding", true);
                break;
            case MovementState.Fall:
                _anim.SetBool("Moving", _player.GetCurrentSpeed() > 0);
                _anim.SetBool("Fall", _player.GetCurrentVelocity().y < 0);
                _anim.SetBool("Jump", _player.GetCurrentVelocity().y > 0);
                break;
        }
    }
}
