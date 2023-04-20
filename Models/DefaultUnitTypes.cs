using MediaevalFightForFood.Models.Units;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models;

public static class DefaultUnitFactories
{
    public static readonly List<Func<Unit>> GreenTeam = new()
    {
        () => new Collector(), 
        () => new Collector(), 
        () => new Archer(), 
        () => new Archer(), 
        () => new Shovelman()
    };

    public static readonly List<Func<Unit>> RedTeam = new()
    {
        () => new Collector(), 
        () => new Knight(), 
        () => new Knight(), 
        () => new Shovelman(),
        () => new Cannon()
    };
}
