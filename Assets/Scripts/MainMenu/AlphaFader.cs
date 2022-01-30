using System.Collections;
using SwapWorld.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class AlphaFader : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MaskableGraphic uiImage;
        [SerializeField] private bool randomStart;
        [SerializeField] private float speedMin;
        [SerializeField] private float speedMax;
        private IAlphaAdapter _adapter;

        private void Awake()
        {
            if (spriteRenderer != null)
                _adapter = new SpriteAdapter(spriteRenderer);
            else if (uiImage != null)
                _adapter = new ImageAdapter(uiImage);
        }

        private void Start()
        {
            _adapter?.ChangeAlpha(0);
            StartCoroutine(StartAlpha());
        }


        private IEnumerator StartAlpha()
        {
            var randomTime = Random.Range(0f, 8f);
            var speed = Random.Range(speedMin, speedMax);
            if (randomStart)
                yield return new WaitForSeconds(randomTime);

            while (true)
            {
                yield return CoroutineExtensions.ElapsedCoroutine(1f / speed, _adapter.ChangeAlpha);
                yield return null;
                yield return CoroutineExtensions.ElapsedCoroutine(1f / speed, f => _adapter.ChangeAlpha(1f - f));
                yield return new WaitForSeconds(randomTime);
            }
        }


        private interface IAlphaAdapter
        {
            void ChangeAlpha(float value);
        }

        private class SpriteAdapter : IAlphaAdapter
        {
            private Color _color;
            private readonly SpriteRenderer _renderer;

            public void ChangeAlpha(float value)
            {
                _color.a = value;

                if (_renderer)
                    _renderer.color = _color;
            }

            public SpriteAdapter(SpriteRenderer renderer)
            {
                _renderer = renderer;
                _color = _renderer.color;
            }
        }


        private class ImageAdapter : IAlphaAdapter
        {
            private Color _color;
            private readonly MaskableGraphic _renderer;

            public void ChangeAlpha(float value)
            {
                _color.a = value;

                if (_renderer)
                    _renderer.color = _color;
            }

            public ImageAdapter(MaskableGraphic renderer)
            {
                _renderer = renderer;
                _color = _renderer.color;
            }
        }
    }
}