using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models.Units;
public class Archer : Unit
{
	public Archer()
	{
		AttackRangeStraight = 2;
        IsRangeUnit = true;
    }
}
