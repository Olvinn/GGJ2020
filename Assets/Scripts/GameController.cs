using SwapWorld.Base;
using SwapWorld.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] ForwardRendererData _data;
    [SerializeField] Widget _loader;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    void Start()
    {
        _loader.Hide(5);
        WorldStateEventManger.OnWorldHasChanged += ChangeWorld;
    }

    public void ChangeWorld(WorldState state)
    {
        StartCoroutine(LoaderDelay(0, () => {
            var s = _data.rendererFeatures[0] as BlitMaterialFeature;
            switch (state)
            {
                case WorldState.White:
                    s.settings.material.SetFloat("_Inverse", 0);
                    break;
                case WorldState.Black:
                    s.settings.material.SetFloat("_Inverse", 1);
                    break;
            }
            _data.SetDirty();
        }));
    }

    void Update()
    {
        
    }

    private void OnDisable()
    {
        var s = _data.rendererFeatures[0] as BlitMaterialFeature;
        s.settings.material.SetFloat("_Inverse", 0);
        _data.SetDirty();
    }

    IEnumerator LoaderDelay(float delay, Action callback)
    {
        _loader.Show(delay);
        yield return new WaitForSeconds(delay);
        _loader.Hide(delay);
        callback?.Invoke();
    }
}
