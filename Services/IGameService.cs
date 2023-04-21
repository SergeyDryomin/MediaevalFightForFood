using MediaevalFightForFood.Models;
using MediaevalFightForFood.Models.Units;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Services;

public interface IGameService
{
    GameBoard GameBoard { get; }
    Team CurrentTeam { get; }
    bool MoveUnit(Unit unit, int newX, int newY);
    bool Attack(Unit attacker, List<Unit> possibleTarget);
    bool Attack(Unit attacker, Unit target);
    void CollectFood();
    bool IsReviving();
    void ReviveUnit(Unit unit, int x, int y);
    Unit? TakeUnit( Cell cell );
    void StartTurn(ref bool isReviving);
    bool HasSameTeamTypeUnit(Unit selectedUnit, Cell cell);
    bool? IsCurrentTeamWon();
}