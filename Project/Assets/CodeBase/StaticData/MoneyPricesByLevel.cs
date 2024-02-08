using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "MoneyPricesByLevel", menuName = "Configs/MoneyPricesByLevel", order = 0)]
    public class MoneyPricesByLevel : ScriptableObject
    {
        public List<string> sceneNames;
        public List<int> levelPrices;
    }
}