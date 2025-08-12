﻿using UnityEngine;

namespace CatAndHuman
{
    [CreateAssetMenu(fileName = "DropRule_", menuName = "Vampire/Effects/Drop/Rule")]
    public class DropRule: ScriptableObject
    {
        public float minRadius;
        public float maxRadius;
        public GameObject collectablePrefab;

    }
}