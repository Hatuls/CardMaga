using Keywords;

public class Dexterity : StatAbst
{
    public Dexterity(int amount) : base(amount)
    {

    }

    public override KeywordTypeEnum Keyword => KeywordTypeEnum.Dexterity;

    public override void Add(int amount)
    {
        base.Add(amount);
    }
    public override void Reduce(int amount)
    {
        base.Reduce(amount);

        if (Amount < 0)
            Amount = 0;
    }

}

public class Strength : StatAbst
{
    public Strength(int amount) : base(amount)
    {
    }

    public override KeywordTypeEnum Keyword => KeywordTypeEnum.Strength;

    public override void Reduce(int amount)
    {
        base.Reduce(amount);

        if (Amount < 0)
            Amount = 0;
    }

}

public class Defense : StatAbst
{
    public Defense(int amount) : base(amount)
    {
    }

    public override KeywordTypeEnum Keyword => KeywordTypeEnum.Defense;



    public override void Reduce(int amount)
    {
        base.Reduce(amount);

        if (Amount < 0)
            Amount = 0;

        // transfer damage to Health


    }
}