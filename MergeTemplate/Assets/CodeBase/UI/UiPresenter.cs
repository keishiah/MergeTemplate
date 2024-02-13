using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.PlayerProgressService;
using CodeBase.UI.Elements;
using Zenject;

namespace CodeBase.UI
{
    public class UiPresenter
    {
        private IPlayerProgressService _playerProgressService;

        private List<UiViewBase> _uiElements = new();

        [Inject]
        void Construct(IPlayerProgressService playerProgressService)
        {
            _playerProgressService = playerProgressService;
        }

        public void SubscribeUIElementToPresenter(UiViewBase uiElement)
        {
            uiElement.InitUiElement(this);
        }
        
        public void AddUiElementToElementsList(UiViewBase element)
        {
            _uiElements.Add(element);
        }

        public UiViewBase GetUiElementFromElementsList<T>() where T : UiViewBase
        {
            return _uiElements.FirstOrDefault(element => element.GetType() == typeof(T));
        }
    }
}