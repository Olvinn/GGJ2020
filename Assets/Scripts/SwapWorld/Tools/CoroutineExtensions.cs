using System;
using System.Collections;
using UnityEngine;

namespace SwapWorld.Tools
{
    public static class CoroutineExtensions
    {
        public static void RestartCoroutine(this MonoBehaviour mono, ref Coroutine coroutine, IEnumerator enumerator)
        {
            if (coroutine != null) mono.StopCoroutine(coroutine);
            coroutine = mono.StartCoroutine(enumerator);
        }

        public static IEnumerator ElapsedCoroutine(float time,
            Action<float> onProgress = null,
            Action onComplete = null,
            bool useUnscaledTime = true)
        {
            var elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var percentage = Mathf.Clamp01(elapsedTime / time);
                onProgress?.Invoke(percentage);
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}