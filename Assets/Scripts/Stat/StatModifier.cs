using System;
using System.Globalization;

namespace CatAndHuman.Stat
{
    [Serializable]
    public class StatModifier
    {
        public StatType stat;
        public ModifierType modifier;
        public float value;

        public static StatModifier Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentException("String is null or empty.");
            }

            var args = s.Split('_');
            if (args.Length != 3)
            {
                throw new ArgumentException($"{s} doesn't have 3 args.");
            }

            try
            {
                return new StatModifier
                {
                    stat = (StatType)Enum.Parse(typeof(StatType), args[0], true),
                    modifier = (ModifierType)Enum.Parse(typeof(ModifierType), args[1], true),
                    value = float.Parse(args[2], CultureInfo.InvariantCulture)
                };
            }
            catch (Exception e)
            {
                throw new ArgumentException($"{s} could not be parsed.");
            }
        }
    }
}