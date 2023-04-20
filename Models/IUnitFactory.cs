using MediaevalFightForFood.Models.Enums;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models;
public interface IUnitFactory
{
	Unit CreateRandomUnit(TeamType team, int x, int y);
}
