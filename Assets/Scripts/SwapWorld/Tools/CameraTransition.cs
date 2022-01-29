using System;
using System.Collections;
using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;
using UnityEngine.UI;

namespace SwapWorld.Tools
{
    public class CameraTransition : WorldSwitchObject
    {
        [SerializeField] private RawImage whiteRawImage;
        [SerializeField] private RawImage blackRawImage;
        [SerializeField] private RectTransform maskTransform;

        private Coroutine _switchCoroutine;


        protected override void OnWorldChanged(WorldState state)
        {
            this.RestartCoroutine(ref _switchCoroutine, SwitchWorld(state));
        }


        private IEnumerator SwitchWorld(WorldState state)
        {
            var startSize = maskTransform.sizeDelta;
            var endSizeX = state == WorldState.White ? 0f : Screen.width + 100f;
            var endSize = new Vector2(endSizeX, startSize.y);

            yield return CoroutineExtensions.ElapsedCoroutine(1f,
                f => { maskTransform.sizeDelta = Vector2.Lerp(startSize, endSize, f); });
        }
    }
}