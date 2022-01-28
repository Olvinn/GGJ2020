using System.Collections.Generic;
using System.Linq;
using SwapWorld.Base;
using SwapWorld.Types;
using UnityEngine;

namespace SwapWorld.Common
{
    public class MaterialChangeObject : WorldSwitchObject
    {
        [System.Serializable]
        private class MaterialByWorldState
        {
            [SerializeField] private WorldState state;
            [SerializeField] private List<MaterialForRenderer> materialsForState;
            public WorldState State => state;
            public List<MaterialForRenderer> MaterialsWithRenderer => materialsForState;
        }

        [System.Serializable]
        private class MaterialForRenderer
        {
            [Tooltip("Изменяемый mesh")] [SerializeField]
            private MeshRenderer meshRenderer;

            [Tooltip("Материал для mesh'a")] [SerializeField]
            private Material material;

            [Tooltip("Если true - создаёт копию материала и использует её в дальнейшем.")] [SerializeField]
            private bool useMaterialCopy;

            private Material materialCopy;

            private Material Material
            {
                get
                {
                    if (!useMaterialCopy) return material;

                    if (materialCopy == null) materialCopy = new Material(material);
                    return material;
                }
            }

            /// <summary>
            /// Натягивает материал (или его копию).
            /// </summary>
            public void ApplyMaterialToRenderer()
            {
                meshRenderer.material = Material;
            }
        }

        [SerializeField] private List<MaterialByWorldState> materials;

        protected override void OnWorldChanged(WorldState state)
        {
            var materialByWorld = materials.FirstOrDefault(c => c.State == state);
            if (materialByWorld == null) return;

            materialByWorld.MaterialsWithRenderer.ForEach(m => m.ApplyMaterialToRenderer());
        }
    }
}