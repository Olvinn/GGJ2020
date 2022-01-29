using MessengerTrigger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : ScriptTrigger
{
    [SerializeField] BoxCollider _collider;
    [SerializeField] Material _activeMat;
    [SerializeField] Renderer _rend;

    public bool isActive { 
        get { return _isActive; } 
        set 
        { 
            _isActive = value; 
            _collider.isTrigger = _isActive; 
            if (value)
                _rend.material = _activeMat;
        } }

    private bool _isActive;

    protected override void TriggerScript()
    {
        if (isActive)
        {
            base.TriggerScript();
        }
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
    }
#endif
}
