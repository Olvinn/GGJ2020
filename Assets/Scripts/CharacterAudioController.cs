using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
    [SerializeField] AudioClip _jump, _fall;
    [SerializeField] AudioSource _as;

    private void Start()
    {
        
    }

    public void JumpSound()
    {
        _as.clip = _jump;
        _as.Play();
    }

    public void GroundedSound( float height)
    {
        if (Mathf.Abs(height) > 1)
        {
            _as.clip = _fall;
            _as.Play();
        }
    }
}
