using MediaevalFightForFood.Models.Units.Base;

public class Cannon : Unit
{
	public Cannon()
	{
		AttackRangeStraight = 3;
		AttackRangeDiagonally = 0;
        IsRangeUnit = true;
    }
}
