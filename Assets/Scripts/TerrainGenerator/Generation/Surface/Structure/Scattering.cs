using UnityEngine;

namespace TerrainGenerator.Generation.Surface.Structure
{
    public class Scattering
    {
        public static Scattering CreateScattering(int scatteringSparceLevel, GameObject parentGameObject)
        {
            return new Scattering(
                scatteringSparceLevel,
                parentGameObject);
        }

        public readonly int scatteringSparceLevel;
        public readonly GameObject scatteringGameObject;

        public Scattering(int scatteringSparceLevel, GameObject parentGameObject)
        {
            this.scatteringSparceLevel = scatteringSparceLevel;
            scatteringGameObject = new GameObject("Scattering");
            scatteringGameObject.transform.parent = parentGameObject.transform;
        }

        public void ApplyScattering(GameObject[] scatteringGameObjects)
        {
            //
        }
    }
}