using MediaevalFightForFood.Models.Enums;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models;

public class UnitFactory : IUnitFactory
{
    private readonly Random _random = new();
    private readonly Dictionary<TeamType, List<Func<Unit>>> _availableFactories;

    public UnitFactory()
    {
        _availableFactories = new Dictionary<TeamType, List<Func<Unit>>>
        {
            { TeamType.Green, new List<Func<Unit>>(DefaultUnitFactories.GreenTeam) },
            { TeamType.Red, new List<Func<Unit>>(DefaultUnitFactories.RedTeam) },
        };
    }

    public Unit CreateRandomUnit(TeamType team, int x, int y)
    {
        List<Func<Unit>> factories = _availableFactories[team];
        int index = _random.Next(factories.Count);
        Func<Unit> selectedFactory = factories[index];
        factories.RemoveAt(index);

        var result = selectedFactory();
        result.TeamType = team;
        result.X = x; 
        result.Y = y;
        return result;
    }
}