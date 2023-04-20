using MediaevalFightForFood.Models.Enums;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models;

public class Cell
{
    public int X { get; }
    public int Y { get; }
    public List<Unit> Units { get; }
    public bool HasFood { get; set; }
    public TeamType? StartPosition { get; set; }

    public Cell(int x, int y)
    {
        X = x;
        Y = y;
        Units = new List<Unit>();
        HasFood = false;
    }

    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
    }
}