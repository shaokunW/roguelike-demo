using System;
using CatAndHuman.Uitilities.Mock;
using UnityEngine;

namespace CatAndHuman.UI.select
{
    public class AppBootstrap : MonoBehaviour
    {
        public PoolList lists;
        public TopPanel top;

        private void Start()
        {
            lists.characters = MockData.Characters();
            UIStateStore.I.Mutate(_ => UIState.Default);
        }
    }
}