using CodeBase.Logic.Buildings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "BuildingStaticData", menuName = "StaticData/BuildingsStaticData", order = 0)]
    public class BuildingStaticData : ScriptableObject
    {
        public BuildingType buildingType;
        public AssetReference buildingPrefab;
    }
}