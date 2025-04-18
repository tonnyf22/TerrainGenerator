using System.Collections.Generic;
using UnityEngine;

namespace TerrainGenerator.Generation.Structure
{
    public class ObjectsScattering
    {
        public static ObjectsScattering CreateScattering(bool isApplyScatteringSparseLevel, int scatteringSparceLevel, GameObject parentGameObject)
        {
            return new ObjectsScattering(
                isApplyScatteringSparseLevel,
                scatteringSparceLevel,
                parentGameObject);
        }

        public readonly bool isApplyScatteringSparseLevel;
        public readonly int scatteringSparseLevel;
        public readonly GameObject objectsScatteringGameObject;

        public ObjectsScattering(bool isApplyScatteringSparseLevel, int scatteringSparseLevel, GameObject parentGameObject)
        {
            this.isApplyScatteringSparseLevel = isApplyScatteringSparseLevel;
            this.scatteringSparseLevel = scatteringSparseLevel;
            objectsScatteringGameObject = new GameObject("Scattering");
            objectsScatteringGameObject.transform.parent = parentGameObject.transform;
            SetObjectsScatteringGameObjectCoordinates();
        }

        private void SetObjectsScatteringGameObjectCoordinates()
        {
            objectsScatteringGameObject.transform.localPosition = Vector3.zero;
        }

        public void ApplyScatteringGameObjects(List<GameObject> scatteringGameObjects)
        {
            foreach (var item in scatteringGameObjects)
            {
                item.transform.parent = objectsScatteringGameObject.transform;
            }
        }
    }
}