using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] Widget _loader;

    void Start()
    {
        _loader.Hide(2);
    }

    void Update()
    {
        
    }
}
