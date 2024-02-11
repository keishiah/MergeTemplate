using System;
using System.Threading;
using CodeBase.Logic.Buildings;
using CodeBase.Services.StaticDataService;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CodeBase.Services
{
    public class BuildingCreator : IDisposable
    {
        private IStaticDataService _staticDataService;
        private CancellationTokenSource _activityToken;

        [Inject]
        void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _activityToken = new CancellationTokenSource();
        }

        public async UniTaskVoid CreateProductInTimeAsync(BuildingPlace buildingPlace, string buildingName)
        {
            var timeToCreate = _staticDataService.GetBuildingData(buildingName).timeToCreate;
            buildingPlace.UpdateTimerText(timeToCreate);

            while (timeToCreate > 0)
            {
                var delayTimeSpan = TimeSpan.FromSeconds(1f);

                await UniTask.Delay(delayTimeSpan, cancellationToken: _activityToken.Token);
                timeToCreate--;
                buildingPlace.UpdateTimerText(timeToCreate);
            }

            CreateBuilding(buildingPlace);
        }

        private void CreateBuilding(BuildingPlace buildingPlace)
        {
            buildingPlace.SetBuildingState(BuildingStateEnum.BuildingFinished);
        }

        public void Dispose()
        {
            _activityToken?.Cancel();
            _activityToken?.Dispose();
            _activityToken = null;
        }
    }
}