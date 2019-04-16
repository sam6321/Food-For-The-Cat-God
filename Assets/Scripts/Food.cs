using System;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public interface IFoodCheck
    {
        bool Check(Food food);
        string GetCheckString();
    }

    // Check a single tag
    public class TagCheck : IFoodCheck
    {
        private Tag tag;

        public TagCheck(Tag tag)
        {
            this.tag = tag;
        }

        public bool Check(Food food)
        {
            return Array.IndexOf(food.tags, tag) >= 0;
        }

        public string GetCheckString()
        {
            switch (tag)
            {
                case Tag.Meat:
                    return "something meaty";

                case Tag.Fruit:
                    return "fruit, I want fruit";

                case Tag.Vegetable:
                    return "vegetables. Yes I eat them too";

                case Tag.Snack:
                    return "a small snack";

                case Tag.Pastry:
                    return "a pastry";

                case Tag.Meal:
                    return "a full meal";

                case Tag.Dessert:
                    return "something nice for after dinner";

                case Tag.Drink:
                    return "something to drink";

                case Tag.ContainsSucrose:
                    return "something containing sucrose";

                case Tag.ContainsFructose:
                    return "something containing fructose";

                case Tag.ContainsPotassium:
                    return "something containing potassium, K?";

                default:
                    return "unknown tag";
            }
        }
    }

    // Check if the sugar level matches
    public class SugarCheck : IFoodCheck
    {
        private FoodLevel level;

        public SugarCheck(FoodLevel level)
        {
            this.level = level;
        }

        public bool Check(Food food)
        {
            return food.sugar == level;
        }

        public string GetCheckString()
        {
            switch(level)
            {
                case FoodLevel.High:
                    return "something really sweet";

                case FoodLevel.Low:
                    return "something that isn't sweet";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check if the energy level matches
    public class EnergyCheck : IFoodCheck
    {
        private FoodLevel level;

        public EnergyCheck(FoodLevel level)
        {
            this.level = level;
        }

        public bool Check(Food food)
        {
            return food.energy == level;
        }

        public string GetCheckString()
        {
            switch (level)
            {
                case FoodLevel.High:
                    return "something that'll give me energy";

                case FoodLevel.Low:
                    return "something low in energy";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check if the carbs level matches
    public class CarbsCheck : IFoodCheck
    {
        private FoodLevel level;

        public CarbsCheck(FoodLevel level)
        {
            this.level = level;
        }

        public bool Check(Food food)
        {
            return food.carbs == level;
        }

        public string GetCheckString()
        {
            switch (level)
            {
                case FoodLevel.High:
                    return "something with carbs";

                case FoodLevel.Low:
                    return "no carbs, I'm watching my weight";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check if the protein level matches
    public class ProteinCheck : IFoodCheck
    {
        private FoodLevel level;

        public ProteinCheck(FoodLevel level)
        {
            this.level = level;
        }

        public bool Check(Food food)
        {
            return food.protein == level;
        }

        public string GetCheckString()
        {
            switch (level)
            {
                case FoodLevel.High:
                    return "protein! Now!";

                case FoodLevel.Low:
                    return "anything without Protein";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check multiple properties
    public class CompoundCheck : IFoodCheck
    {
        private IFoodCheck[] checks;

        public CompoundCheck(params IFoodCheck[] checks)
        {
            this.checks = checks;
        }

        public bool Check(Food food)
        {
            return Array.TrueForAll(checks, check => check.Check(food));
        }

        public string GetCheckString()
        {
            return String.Join(", ", checks.Select(c => c.GetCheckString()));
        }
    }

    public enum Tag
    {
        Meat,
        Fruit,
        Vegetable,
        Snack,
        Pastry,
        Meal,
        Dessert,
        Drink,
        ContainsSucrose,
        ContainsFructose,
        ContainsPotassium
    }

    public enum ModifierTag
    {
        None,
        AlwaysSucceed,
        AlwaysFail
    }

    public enum FoodLevel
    {
        High,
        Low
    }

    [SerializeField]
    private string name;

    [SerializeField]
    private FoodLevel sugar;

    [SerializeField]
    private FoodLevel energy;

    [SerializeField]
    private FoodLevel carbs;

    [SerializeField]
    private FoodLevel protein;

    [SerializeField]
    private Tag[] tags;

    [SerializeField]
    private ModifierTag modifierTag;

    public bool ShouldGenerateFoodCheck()
    {
        // Only generate food checks for foods that do not have a special modifier
        return modifierTag == ModifierTag.None;
    }

    public IFoodCheck GetRandomFoodCheck()
    {
        int value = Random.Range(0, 3 + tags.Length);
        if(value <= 3)
        {
            // FoodLevel check
            switch(value)
            {
                default:
                case 0: return new SugarCheck(sugar);
                case 1: return new EnergyCheck(energy);
                case 2: return new CarbsCheck(carbs);
                case 3: return new ProteinCheck(protein);
            }
        } 
        else
        {
            // Tags check
            return new TagCheck(tags[value - 3]);
        }
    }
}
