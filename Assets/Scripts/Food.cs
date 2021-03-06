﻿using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

public class Food : MonoBehaviour
{
    public abstract class FoodCheck
    {
        private Food food;
        public Food Food { get { return food; } }

        protected FoodCheck(Food food)
        {
            this.food = food;
        }

        public bool Check(Food food)
        {
            switch(food.modifierTag)
            {
                case ModifierTag.AlwaysFail:
                    return false;

                case ModifierTag.AlwaysSucceed:
                    return true;

                default:
                case ModifierTag.None:
                    return CheckImpl(food);
            }
        }

        protected abstract bool CheckImpl(Food food);
        public abstract string GetCheckString();
    }

    // Check a single tag
    public class TagCheck : FoodCheck
    {
        private Tag tag;

        public TagCheck(Tag tag, Food food) : base(food)
        {
            this.tag = tag;
        }

        protected override bool CheckImpl(Food food)
        {
            return Array.IndexOf(food.tags, tag) >= 0;
        }

        public override string GetCheckString()
        {
            switch (tag)
            {
                case Tag.Meat:
                    return "Something meaty";

                case Tag.Fruit:
                    return "Fruit, I want fruit";

                case Tag.Vegetable:
                    return "Vegetables. Yes I eat them too";

                case Tag.Snack:
                    return "I'd like a small snack";

                case Tag.Pastry:
                    return "I want a pastry";

                case Tag.Meal:
                    return "A full meal, please";

                case Tag.Dessert:
                    return "Something nice for after dinner";

                case Tag.Drink:
                    return "Something to drink";

                case Tag.ContainsSucrose:
                    return "Something containing sucrose";

                case Tag.ContainsFructose:
                    return "Something containing fructose";

                case Tag.ContainsPotassium:
                    return "Something containing potassium, K?";

                default:
                    return "unknown tag";
            }
        }
    }

    // Check if the sugar level matches
    public class SugarCheck : FoodCheck
    {
        private FoodLevel level;

        public SugarCheck(FoodLevel level, Food food) : base(food)
        {
            this.level = level;
        }

        protected override bool CheckImpl(Food food)
        {
            return food.sugar == level;
        }

        public override string GetCheckString()
        {
            switch(level)
            {
                case FoodLevel.High:
                    return "Something really sweet";

                case FoodLevel.Low:
                    return "Something that isn't sweet";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check if the energy level matches
    public class EnergyCheck : FoodCheck
    {
        private FoodLevel level;

        public EnergyCheck(FoodLevel level, Food food) : base(food)
        {
            this.level = level;
        }

        protected override bool CheckImpl(Food food)
        {
            return food.energy == level;
        }

        public override string GetCheckString()
        {
            switch (level)
            {
                case FoodLevel.High:
                    return "Something that'll give me energy";

                case FoodLevel.Low:
                    return "Something low in energy";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check if the carbs level matches
    public class CarbsCheck : FoodCheck
    {
        private FoodLevel level;

        public CarbsCheck(FoodLevel level, Food food) : base(food)
        {
            this.level = level;
        }

        protected override bool CheckImpl(Food food)
        {
            return food.carbs == level;
        }

        public override string GetCheckString()
        {
            switch (level)
            {
                case FoodLevel.High:
                    return "Something with carbohydrates";

                case FoodLevel.Low:
                    return "No carbs, I'm watching my weight";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check if the protein level matches
    public class ProteinCheck : FoodCheck
    {
        private FoodLevel level;

        public ProteinCheck(FoodLevel level, Food food) : base(food)
        {
            this.level = level;
        }

        protected override bool CheckImpl(Food food)
        {
            return food.protein == level;
        }

        public override string GetCheckString()
        {
            switch (level)
            {
                case FoodLevel.High:
                    if(Random.Range(0, 10) == 0)
                    {
                        return "Protein, I'm getting SWOLE";
                    }
                    else
                    {
                        return "Protein! Now!";
                    }

                case FoodLevel.Low:
                    return "Anything without Protein";

                default:
                    return "unknown food level";
            }
        }
    }

    // Check multiple properties
    public class CompoundCheck : FoodCheck
    {
        private FoodCheck[] checks;

        public CompoundCheck(Food food, params FoodCheck[] checks) : base(food)
        {
            this.checks = checks;
        }

        protected override bool CheckImpl(Food food)
        {
            return Array.TrueForAll(checks, check => check.Check(food));
        }

        public override string GetCheckString()
        {
            return String.Join(", ", checks.Select(c => c.GetCheckString()).ToArray());
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

    public string Name { get { return name; } }

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
    
    [SerializeField]
    public List<AudioClip> allAudio;

    public bool ShouldGenerateFoodCheck()
    {
        // Only generate food checks for foods that do not have a special modifier
        return modifierTag == ModifierTag.None;
    }

    public FoodCheck GetRandomFoodCheck()
    {
        int value = Random.Range(0, 4 + tags.Length);
        if(value <= 3)
        {
            // FoodLevel check
            switch(value)
            {
                default:
                case 0: return new SugarCheck(sugar, this);
                case 1: return new EnergyCheck(energy, this);
                case 2: return new CarbsCheck(carbs, this);
                case 3: return new ProteinCheck(protein, this);
            }
        } 
        else
        {
            // Tags check
            return new TagCheck(tags[value - 4], this);
        }
    }
}
