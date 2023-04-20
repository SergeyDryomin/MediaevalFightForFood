using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models.Units;

public class Knight : Unit, ICounterAttackUnit
{
    public override void Attack(Unit target)
	{
		base.Attack(target);
        if (target is ICounterAttackUnit counterAttackUnit)
        {
            counterAttackUnit.CounterAttack( this );
        }
    }

	public void CounterAttack(Unit attacker, int healthBeforeAttack = 1)
	{
		--attacker.Health;
	}
}