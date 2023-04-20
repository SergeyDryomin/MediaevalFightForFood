using MediaevalFightForFood.Models;
using MediaevalFightForFood.Models.Units;
using MediaevalFightForFood.Models.Units.Base;
using GameBoard = MediaevalFightForFood.Models.GameBoard;

namespace MediaevalFightForFood.Services;
public class GameService
{
    public GameBoard GameBoard { get; private set; }
    public Team CurrentTeam;
    public List<Team> Teams ;

    public void InitializeGame(IUnitFactory unitFactory)
    {
        Teams = new List<Team>();
        GameBoard = new GameBoard( unitFactory, Teams , 9, 5 );
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

    public void EndTurn()
    {
        var nextTeamPosition = Teams.IndexOf(CurrentTeam) + 1;
        if (nextTeamPosition == Teams.Count)
        {
            nextTeamPosition = 0;
        }

        CurrentTeam = Teams[nextTeamPosition];
    }

    public void StartTurn(ref bool isReviving)
    {
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
        if (Teams.All(u => u.TeamType == CurrentTeam.TeamType) ||
            CurrentTeam.Units.Count(u => u.Cell?.StartPosition != null && u.Cell?.StartPosition != CurrentTeam.TeamType) >= 2)
        {
            return true;
        }
        else if(!Teams.Any())
        {
            return null;
        }

        return false;
    }
}
