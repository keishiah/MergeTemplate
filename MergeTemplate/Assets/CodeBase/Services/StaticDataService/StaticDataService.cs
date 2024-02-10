using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Logic.Buildings;
using CodeBase.StaticData;

namespace CodeBase.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private const string BuildingsDataPath = "BuildingsStaticData";

        private Dictionary<BuildingType, BuildingStaticData> _buildingStaticData;

        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async void Initialize()
        {
            var buildingStaticDataTemp =
                await _assetProvider.LoadStaticDataByLabel<BuildingStaticData>(BuildingsDataPath);
            _buildingStaticData = buildingStaticDataTemp.ToDictionary(x => x.buildingType, x => x);
        }
        
        public BuildingStaticData GetBuildingData(BuildingType buildingType) =>
            _buildingStaticData.TryGetValue(buildingType, out BuildingStaticData buildingData)
                ? buildingData
                : throw new Exception($"_buildingStaticData dictionary doesnt have {buildingType}");
    }
}