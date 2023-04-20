using MediaevalFightForFood.Models.Units.Base;

public interface ICounterAttackUnit
{
	void CounterAttack(Unit attacker, int healthBeforeAttack = 1);
}
