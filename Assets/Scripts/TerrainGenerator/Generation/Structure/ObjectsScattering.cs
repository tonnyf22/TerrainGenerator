using System.Collections.Generic;
using UnityEngine;

namespace TerrainGenerator.Generation.Structure
{
    public class ObjectsScattering
    {
        public static ObjectsScattering CreateScattering(/* bool isApplyScattering, bool isApplyScatteringSparceLevel, */ int scatteringSparceLevel, GameObject parentGameObject)
        {
            return new ObjectsScattering(
                // isApplyScattering,
                // isApplyScatteringSparceLevel,
                scatteringSparceLevel,
                parentGameObject);
        }

        // public readonly bool isApplyScattering;
        // public readonly bool isApplyScatteringSparceLevel;
        public readonly int scatteringSparseLevel;
        public readonly GameObject scatteringGameObject;

        public ObjectsScattering(/* bool isApplyScattering, bool isApplyScatteringSparceLevel, */ int scatteringSparceLevel, GameObject parentGameObject)
        {
            // this.isApplyScattering = isApplyScattering;
            // this.isApplyScatteringSparceLevel = isApplyScatteringSparceLevel;
            this.scatteringSparseLevel = scatteringSparceLevel;
            scatteringGameObject = new GameObject("Scattering");
            scatteringGameObject.transform.parent = parentGameObject.transform;
        }

        public void ApplyScatteringGameObjects(List<GameObject> scatteringGameObjects)
        {
            foreach (var item in scatteringGameObjects)
            {
                item.transform.parent = scatteringGameObject.transform;
            }
        }
    }
}