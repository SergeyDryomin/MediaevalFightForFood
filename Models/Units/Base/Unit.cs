using MediaevalFightForFood.Models.Enums;

namespace MediaevalFightForFood.Models.Units.Base;
public abstract class Unit
{
    public int X { get; set; }
    public int Y { get; set; }
    public TeamType TeamType { get; set; }
    public Team Team { get; set; }
    public Cell? Cell { get; set; }
    public int Health { get; set; } = 1;
    public int MaxHealth { get; set; } = 1;
    public int AttackRangeStraight { get; protected set; } = 1;
    public int AttackRangeDiagonally { get; protected set; } = 1;
    public bool IsFirstMove = true;
	public bool CanCollectFood { get; protected set; } = false;
    public bool IsRangeUnit { get; protected set; } = false;

    public virtual void Attack(Unit target)
	{
		--target.Health;
	}

    public bool IsFullHealth()
    {
        return Health == MaxHealth;
    }
}
