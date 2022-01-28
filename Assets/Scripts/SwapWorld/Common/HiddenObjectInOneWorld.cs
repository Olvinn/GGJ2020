using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Common
{
    public class HiddenObjectInOneWorld : WorldSwitchObject
    {
        [SerializeField] private WorldState hiddenWorldState;
        [SerializeField] private MeshRenderer meshRenderer;

        private void Reset()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();

            if (meshRenderer == null) enabled = false;
        }

        protected override void OnWorldChanged(WorldState state)
        {
            if (state == hiddenWorldState) HideObject();
            else ShowObject();
        }

        protected virtual void HideObject()
        {
            meshRenderer.enabled = false;
        }

        protected virtual void ShowObject()
        {
            meshRenderer.enabled = true;
        }
    }
}