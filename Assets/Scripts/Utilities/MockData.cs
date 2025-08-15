using System.Collections.Generic;
using UnityEngine;

namespace CatAndHuman.Uitilities.Mock
{
    [System.Serializable]
    public class Character
    {
        public int id;
        public string name;
        public string role;
        public Sprite sprite;
        public List<Weapon> weapons;
    }

    [System.Serializable]
    public class Weapon
    {
        public int id;
        public string name;
        public string tag;
        public Sprite sprite;
    }

    public static class MockData
    {
        public static List<Character> Characters()
        {
            return new List<Character>
            {
                new()
                {
                    id = 0, name = "Astra", role = "tank",
                    sprite = MockSprites.Solid(512, 512, new Color(0.3f, 0.5f, 0.9f)), weapons = Weapons()
                },
                new()
                {
                    id = 1, name = "Test2", role = "testRole",
                    sprite = MockSprites.Solid(512, 512, new Color(0.1f, 0.5f, 0.3f)), weapons = Weapons()
                },
            };
        }

        public static List<Weapon> Weapons()
        {
            return new List<Weapon>
            {
                new()
                {
                    id = 0, name = "WAstra", tag = "range",
                    sprite = MockSprites.Solid(512, 512, new Color(0.8f, 0.5f, 0.9f))
                },
                new()
                {
                    id = 1, name = "WTest2", tag = "melee",
                    sprite = MockSprites.Solid(512, 512, new Color(0.8f, 0.5f, 0.3f))
                }
            };
        }
    }
}