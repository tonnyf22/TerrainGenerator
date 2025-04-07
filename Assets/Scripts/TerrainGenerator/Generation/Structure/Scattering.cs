using UnityEngine;

namespace TerrainGenerator.Generation.Structure
{
    public class Scattering
    {
        public static Scattering CreateScattering(/* bool isApplyScattering, bool isApplyScatteringSparceLevel, */ int scatteringSparceLevel, GameObject parentGameObject)
        {
            return new Scattering(
                // isApplyScattering,
                // isApplyScatteringSparceLevel,
                scatteringSparceLevel,
                parentGameObject);
        }

        public readonly bool isApplyScattering;
        public readonly bool isApplyScatteringSparceLevel;
        public readonly int scatteringSparceLevel;
        public readonly GameObject scatteringGameObject;

        public Scattering(/* bool isApplyScattering, bool isApplyScatteringSparceLevel, */ int scatteringSparceLevel, GameObject parentGameObject)
        {
            // this.isApplyScattering = isApplyScattering;
            // this.isApplyScatteringSparceLevel = isApplyScatteringSparceLevel;
            this.scatteringSparceLevel = scatteringSparceLevel;
            scatteringGameObject = new GameObject("Scattering");
            scatteringGameObject.transform.parent = parentGameObject.transform;
        }

        public void ApplyScatteringGameObjects(GameObject[] scatteringGameObjects)
        {
            foreach (var item in scatteringGameObjects)
            {
                item.transform.parent = scatteringGameObject.transform;
            }
        }
    }
}