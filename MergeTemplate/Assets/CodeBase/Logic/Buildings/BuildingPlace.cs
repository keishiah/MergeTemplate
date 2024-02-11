using CodeBase.Services;
using CodeBase.Services.StaticDataService;
using CodeBase.UI;
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
        // public string buildingName;

        private string _buildingToCreateName;

        private IStaticDataService _staticDataService;
        private UiPresenter _uiPresenter;
        private BuildingCreator _buildingCreator;

        [Inject]
        void Construct(IStaticDataService staticDataService, UiPresenter uiPresenter, BuildingCreator buildingCreator)
        {
            _staticDataService = staticDataService;
            _uiPresenter = uiPresenter;
            _buildingCreator = buildingCreator;
        }

        public void SetBuildingState(BuildingStateEnum state)
        {
            switch (state)
            {
                case BuildingStateEnum.Inactive:
                    buildingStateImage.raycastTarget = false;
                    buildingStateImage.gameObject.SetActive(false);
                    break;
                case BuildingStateEnum.PlaceToBuild:
                    buildingStateImage.raycastTarget = true;
                    SubscribeToOpenCreateBuildingPopup();
                    ShowBuildSprite(_staticDataService.PlaceToBuildSprite);
                    break;
                case BuildingStateEnum.BuildInProgress:
                    buildingStateImage.raycastTarget = false;
                    ShowBuildSprite(_staticDataService.BuildInProgressSprite);
                    break;
                case BuildingStateEnum.BuildingFinished:
                    buildingStateImage.raycastTarget = false;
                    ShowBuildSprite(_staticDataService.GetBuildingInfo(_buildingToCreateName).buildingSprite);
                    break;
            }
        }

        public void StartCreatingBuilding(string buildingToCreateName)
        {
            _buildingToCreateName = buildingToCreateName;
            SetBuildingState(BuildingStateEnum.BuildInProgress);
            _buildingCreator.CreateProductInTimeAsync(this).Forget();
        }

        private void SubscribeToOpenCreateBuildingPopup()
        {
            buildingStateImage.GetComponent<Button>().onClick
                .AddListener(() => _uiPresenter.OpenCreateBuildingPopup(this));
        }

        private void ShowBuildSprite(Sprite spriteToShow)
        {
            buildingStateImage.gameObject.SetActive(true);
            buildingStateImage.sprite = spriteToShow;
        }
    }
}