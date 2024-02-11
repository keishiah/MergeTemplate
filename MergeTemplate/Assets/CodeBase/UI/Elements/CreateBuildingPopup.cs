using CodeBase.Logic.Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class CreateBuildingPopup : UiViewBase
    {
        public Button buildButton;

        public override void InitUiElement(UiPresenter uiPresenter)
        {
            uiPresenter.AddUiElementToElementsList(this);
            gameObject.SetActive(false);
        }

        public void SubscribeToCreateBuilding(BuildingPlace buildingPlace)
        {
            buildButton.onClick.AddListener(() =>
            {
                buildingPlace.StartCreatingBuilding("Name");
                gameObject.SetActive(false);
            });
        }
    }
}