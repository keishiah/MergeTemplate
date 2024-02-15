using System;
using UniRx;
using UnityEngine;

namespace CodeBase.Data
{
    [Serializable]
    public class Coins : ISerializationCallbackReceiver
    {
        public int CurrentCoinsCount => _coinsCount.Value;

        private ReactiveProperty<int> _coinsCount = new();
        private int _defaultValue = 0;
        [SerializeField] private int savedValue;

        public Coins()
        {
            _coinsCount.Subscribe((newValue => savedValue = newValue));
        }

        public bool SpendCoins(int count)
        {
            if (_coinsCount.Value <= count)
                return false;
            _coinsCount.Value -= count;
            return true;
        }

        public void AddCoins(int count) => _coinsCount.Value += count;

        public IDisposable SubscribeToCoinsCountChanges(Action<int> onCoinsCountChanged)
        {
            return _coinsCount.Subscribe(onCoinsCountChanged);
        }

        public void OnAfterDeserialize()
        {
            _coinsCount.Value = Mathf.Max(_defaultValue, savedValue);
        }

        public void OnBeforeSerialize()
        {
        }
    }
}