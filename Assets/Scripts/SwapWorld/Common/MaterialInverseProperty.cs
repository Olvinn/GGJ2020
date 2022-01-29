using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Common
{
    public class MaterialInverseProperty : WorldSwitchObject
    {
        [SerializeField] private Material material;
        private static readonly int Inverse = Shader.PropertyToID("Inverse");

        protected override void OnWorldChanged(WorldState state)
        {
            var inverse = state == WorldState.Black;
            material.SetFloat(Inverse, inverse ? 1f : 0f);
        }
    }
}