using CodeBase.Services;
using CodeBase.Services.StaticDataService;
using CodeBase.UI;
using TMPro;
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
        public TextMeshProUGUI timerText;

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
                    timerText.gameObject.SetActive(false);
                    buildingStateImage.gameObject.SetActive(false);
                    break;
                case BuildingStateEnum.PlaceToBuild:
                    buildingStateImage.raycastTarget = true;
                    SubscribeToOpenCreateBuildingPopup();
                    ShowBuildSprite(_staticDataService.PlaceToBuildSprite);
                    break;
                case BuildingStateEnum.BuildInProgress:
                    buildingStateImage.raycastTarget = false;
                    timerText.gameObject.SetActive(true);
                    ShowBuildSprite(_staticDataService.BuildInProgressSprite);
                    break;
                case BuildingStateEnum.BuildingFinished:
                    buildingStateImage.raycastTarget = false;
                    timerText.gameObject.SetActive(false);
                    ShowBuildSprite(_staticDataService.GetBuildingData(_buildingToCreateName).buildingSprite);
                    break;
            }
        }

        public void UpdateTimerText(int totalSeconds)
        {
            int hours = totalSeconds / 3600;
            int minutes = (totalSeconds % 3600) / 60;
            int seconds = totalSeconds % 60;

            string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
            timerText.text = formattedTime;
        }

        public void StartCreatingBuilding(string buildingToCreateName)
        {
            _buildingToCreateName = buildingToCreateName;
            SetBuildingState(BuildingStateEnum.BuildInProgress);
            _buildingCreator.CreateProductInTimeAsync(this, buildingToCreateName).Forget();
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