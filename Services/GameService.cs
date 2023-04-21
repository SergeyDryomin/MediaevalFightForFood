using MediaevalFightForFood.Models;
using MediaevalFightForFood.Models.Units;
using MediaevalFightForFood.Models.Units.Base;
using GameBoard = MediaevalFightForFood.Models.GameBoard;

namespace MediaevalFightForFood.Services;
public class GameService : IGameService
{
    public GameBoard GameBoard { get; set; }
    public Team CurrentTeam { get; set; }
    private readonly List<Team> Teams ;
    public GameService(IUnitFactory unitFactory)
    {
        Teams = new List<Team>();
        GameBoard = new GameBoard(unitFactory, Teams, 9, 5);
        CurrentTeam = Teams.First();
    }

    public bool MoveUnit(Unit unit, int newX, int newY)
    {
        if (!GameBoard.IsValidPositionToMove(unit, newX, newY))
        {
            return false;
        }

        GameBoard.MoveUnit(unit, newX, newY);
        unit.IsFirstMove = false;
        return true;
    }

    public bool Attack(Unit attacker, List<Unit> possibleTarget)
    {
        var target = possibleTarget.FirstOrDefault(t => t.TeamType != CurrentTeam.TeamType);
        return target is not null && Attack( attacker, target);
    }

    public bool Attack(Unit attacker, Unit target)
    {
        if (!GameBoard.IsValidToAttack(attacker, target))
        {
            return false;
        }

        // Вызов метода Attack базового класса Unit
        attacker.Attack(target);

        // Удалить цель, если ее здоровье стало меньше или равно нулю
        if (target.Health <= 0)
        {
            GameBoard.RemoveUnit(target);
            target.Team.RemoveUnit(target);
        }

        if (attacker.Health <= 0)
        {
            GameBoard.RemoveUnit(attacker);
            CurrentTeam.RemoveUnit(attacker);
        }
        else if (!attacker.IsRangeUnit)
        {
            MoveUnit(attacker, target.X, target.Y);
        }

        attacker.IsFirstMove = false;
        target.IsFirstMove = false;
        return true;
    }

    public void CollectFood()
    {
        if (!CurrentTeam.HasFood)
        {
            if (CurrentTeam.Units.Any(u => u.CanCollectFood && u.Cell.HasFood))
            {
                CurrentTeam.CollectFood();
            }
        }
    }

    public bool IsReviving()
    {
        return CurrentTeam.HasFood &&
               CurrentTeam.DeadUnits.Any() &&
               GameBoard.IsAnyPlaceToRevive(CurrentTeam);
    }

    public void ReviveUnit(Unit unit, int x, int y)
    {
        if (!GameBoard.IsValidPositionToRevive(unit, x, y))
        {
            return;
        }

        unit.X = x;
        unit.Y = y;
        unit.Health = unit.MaxHealth;
        GameBoard.AddUnit(unit);
        CurrentTeam.ReviveUnit(unit);
    }

    public Unit? TakeUnit( Cell cell )
    {
        return cell.Units.FirstOrDefault( u => u.TeamType == CurrentTeam.TeamType);
    }

    public void StartTurn(ref bool isReviving)
    {
        var nextTeamPosition = Teams.IndexOf(CurrentTeam) + 1;
        if (nextTeamPosition == Teams.Count)
        {
            nextTeamPosition = 0;
        }

        CurrentTeam = Teams[nextTeamPosition];

        CollectFood();

        if ( IsReviving() )
        {
            isReviving = true;
        }
    }

    public bool HasSameTeamTypeUnit(Unit selectedUnit, Cell cell)
    {
        return cell.Units.Any(u => u.TeamType == selectedUnit.TeamType);
    }

    public bool? IsCurrentTeamWon()
    {
        // Check are all other dead or 2 enemy points captured(!!! - fix for more players)
        if (Teams.Where( t => t != CurrentTeam).All(u => u.Units.Count == 0) ||
            CurrentTeam.Units.Count(u => u.Cell?.StartPosition != null && u.Cell?.StartPosition != CurrentTeam.TeamType) >= 2)
        {
            // Return null if all units dead for draw game or false
            return CurrentTeam.Units.Count == 0 ? null : true;
        }

        return  false;
    }
}
