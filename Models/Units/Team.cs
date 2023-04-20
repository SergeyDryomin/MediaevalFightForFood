using MediaevalFightForFood.Models.Enums;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models.Units;
public class Team
{
    public TeamType TeamType { get; }
    public List<Unit> Units { get; }
    public List<Unit> DeadUnits { get; }
    public bool HasFood { get; set; }

    public Team(TeamType type)
    {
        TeamType = type;
        Units = new();
        DeadUnits = new();
        HasFood = false;
    }

    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
        DeadUnits.Add(unit);
    }

    public void ReviveUnit(Unit unit)
    {
        DeadUnits.Remove(unit);
        Units.Add(unit);
        HasFood = false;
    }

    public void CollectFood()
    {
        HasFood = true;
    }
}