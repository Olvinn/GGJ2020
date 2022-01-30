using SwapWorld.Base;
using SwapWorld.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] ForwardRendererData _data;
    [SerializeField] Widget _loader;
    [SerializeField] Volume _volume;

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
                    {
                        s.settings.material.SetFloat("_Inverse", 0);
                        Vignette vignette;
                        if (_volume.profile.TryGet(out vignette))
                        {
                            vignette.active = false;
                        }
                        ChromaticAberration aberration;
                        if (_volume.profile.TryGet(out aberration))
                        {
                            aberration.active = false;
                        }
                        break;
                    }
                case WorldState.Black:
                    {
                        s.settings.material.SetFloat("_Inverse", 1);
                        Vignette vignette;
                        if (_volume.profile.TryGet(out vignette))
                        {
                            vignette.active = true;
                        }
                        ChromaticAberration aberration;
                        if (_volume.profile.TryGet(out aberration))
                        {
                            aberration.active = true;
                        }
                        break;
                    }
            }
            _data.SetDirty();
        }));
    }

    void Update()
    {
        
    }

    public void LoadEngame()
    {
        LoaderDelay(2, () => SceneManager.LoadScene(2));
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
