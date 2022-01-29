using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;
using UnityEngine.Serialization;

namespace SwapWorld.Common
{
    public class HiddenObjectInOneWorld : WorldSwitchObject
    {
        [SerializeField] private WorldState hiddenWorldState;
        [FormerlySerializedAs("meshRenderer")] [SerializeField] private Renderer objectRenderer;
        [SerializeField] private Collider objectCollider;


        private void Reset()
        {
            objectRenderer = GetComponent<Renderer>();
            objectCollider = GetComponent<Collider>();
        }

        protected override void OnWorldChanged(WorldState state)
        {
            if (state == hiddenWorldState) HideObject();
            else ShowObject();
        }

        protected virtual void HideObject()
        {
            if (objectRenderer)
                objectRenderer.enabled = false;

            if (objectCollider)
                objectCollider.enabled = false;
        }

        protected virtual void ShowObject()
        {
            if (objectRenderer)
                objectRenderer.enabled = true;

            if (objectCollider)
                objectCollider.enabled = true;
        }
    }
}