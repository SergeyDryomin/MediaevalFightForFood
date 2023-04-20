using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models.Units;
public class Collector : Unit
{
	public Collector()
	{
		CanCollectFood = true;
        AttackRangeDiagonally = 0;
		AttackRangeStraight = 0;
    }
}
