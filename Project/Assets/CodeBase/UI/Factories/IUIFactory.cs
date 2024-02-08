﻿using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CodeBase.UI.Factories
{
    public interface IUIFactory
    {
        void Cleanup();
        UniTask WarmUp();
        UniTask<GameObject> CreateJoyStick();
        UniTask CreateuiRoot();
    }
}