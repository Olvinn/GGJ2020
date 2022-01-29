using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Common
{
    public class HiddenObjectInOneWorld : WorldSwitchObject
    {
        [SerializeField] private WorldState hiddenWorldState;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Collider objectCollider;


        private void Reset()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            objectCollider = GetComponent<Collider>();
        }

        protected override void OnWorldChanged(WorldState state)
        {
            if (state == hiddenWorldState) HideObject();
            else ShowObject();
        }

        protected virtual void HideObject()
        {
            if (meshRenderer)
                meshRenderer.enabled = false;

            if (objectCollider)
                objectCollider.enabled = false;
        }

        protected virtual void ShowObject()
        {
            if (meshRenderer)
                meshRenderer.enabled = true;

            if (objectCollider)
                objectCollider.enabled = true;
        }
    }
}