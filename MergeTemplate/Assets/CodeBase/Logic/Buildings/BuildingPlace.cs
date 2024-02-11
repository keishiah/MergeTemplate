using CodeBase.Services.StaticDataService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.Logic.Buildings
{
    public enum BuildingStateEnum
    {
        Inactive,
        PlaceToBuild,
        BuildInProgress,
        BuildingFinished
    }

    public class BuildingPlace : MonoBehaviour
    {
        public Image buildingStateImage;
        public string buildingName;

        private IStaticDataService _staticDataService;

        [Inject]
        void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void SetBuildingState(BuildingStateEnum state)
        {
            switch (state)
            {
                case BuildingStateEnum.Inactive:
                    buildingStateImage.gameObject.SetActive(false);
                    break;
                case BuildingStateEnum.PlaceToBuild:
                    ShowBuildSprite(_staticDataService.PlaceToBuildSprite);
                    break;
                case BuildingStateEnum.BuildInProgress:
                    ShowBuildSprite(_staticDataService.BuildInProgressSprite);
                    break;
                case BuildingStateEnum.BuildingFinished:
                    ShowBuildSprite(_staticDataService.GetBuildingInfo(buildingName).buildingSprite);
                    break;
            }
        }

        private void ShowBuildSprite(Sprite spriteToShow)
        {
            buildingStateImage.gameObject.SetActive(true);
            buildingStateImage.sprite = spriteToShow;
        }
    }
}