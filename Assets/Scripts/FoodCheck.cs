public abstract class FoodCheck
{
    private Food food;
    public Food Food { get { return food; } }

    protected FoodCheck(Food food)
    {
        this.food = food;
    }

    public abstract bool Check(Food food);
    public abstract string GetCheckString();
}