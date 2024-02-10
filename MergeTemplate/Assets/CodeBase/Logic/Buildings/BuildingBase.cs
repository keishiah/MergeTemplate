using UnityEngine;

namespace CodeBase.Logic.Buildings
{
    public enum BuildingType
    {
        CubeBuilding
    }

    public abstract class BuildingBase : MonoBehaviour
    {
        public BuildingType buildingType;
    }
}