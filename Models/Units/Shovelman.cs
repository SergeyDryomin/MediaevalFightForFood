using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models.Units;
public class Shovelman : Unit, ICounterAttackUnit
{
	public Shovelman()
	{
		Health = 2;
        MaxHealth = 2;

    }

	public override void Attack(Unit target)
	{
		int healthBeforeAttack = target.Health;
		target.Health -= Health;
		if (target is ICounterAttackUnit counterAttackUnit)
		{
			counterAttackUnit.CounterAttack(this, healthBeforeAttack);
		}
	}

	public void CounterAttack(Unit attacker, int healthBeforeAttack)
	{
		attacker.Health -= healthBeforeAttack;
	}
}
