using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagment;
using CodeBase.Services.SceneContextProvider;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.UI.Factories
{
    public class UIFactory : IUIFactory
    {
        public Transform _worldCanvasTransform { get; private set; }

        private readonly IAssetProvider _assetProvider;
        private readonly UiPresenter _uiPresenter;
        private readonly SceneContextProvider _sceneContextProvider;
        
        private GameObject uiRoot;

        public UIFactory(IAssetProvider assetProvider, UiPresenter uiPresenter,
            SceneContextProvider sceneContextProvider)
        {
            _assetProvider = assetProvider;
            _uiPresenter = uiPresenter;
            _sceneContextProvider = sceneContextProvider;
        }


        public async UniTask WarmUp()
        {
            await _assetProvider.Load<GameObject>(AssetPath.UIRoot);
        }

        public async UniTask CreateuiRoot()
        {
            var prefab = await _assetProvider.Load<GameObject>(AssetPath.UIRoot);
            var element = _sceneContextProvider.GetCurrentSceneContextInstantiator()
                .InstantiatePrefab(prefab);

            uiRoot = element;
        }

        public async UniTask<GameObject> CreateJoyStick()
        {
            GameObject joyPrefab =
                await _assetProvider.Load<GameObject>(AssetPath.JoystickCanvas);
            GameObject joy =
                _sceneContextProvider.GetCurrentSceneContextInstantiator().InstantiatePrefab(joyPrefab);
            joy.GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("JoystickCamera").GetComponent<Camera>();

            return joy;
        }
        

        public void Cleanup()
        {
            // _uiPresenter.Unsubscribe();
        }
    }
}