using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.StaticData;

namespace CodeBase.Services.StaticDataService
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async void Initialize()
        {
        }
        
    }
}