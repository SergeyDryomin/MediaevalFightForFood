using MediaevalFightForFood.Models.Enums;
using MediaevalFightForFood.Models.Units;
using MediaevalFightForFood.Models.Units.Base;

namespace MediaevalFightForFood.Models;

public class GameBoard
{
    public int Width { get; }
    public int Height { get; }
    public Cell[,] Cells { get; }

    public GameBoard( IUnitFactory unitFactory, List<Team> teams, int width, int height )
    {
        Width = width;
        Height = height;
        Cells = new Cell[width, height];

        // Инициализация ячеек
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cells[x, y] = new Cell(x, y);
            }
        }

        string[] boardSetup =
        {
            "000pepppp",
            "00ppppepp",
            "pppeppppp",
            "ppppppp11",
            "ppppep111"
        };

        for (int y = 0; y < boardSetup.Length; y++)
        {
            for (int x = 0; x < boardSetup[y].Length; x++)
            {
                char cellContent = boardSetup[y][x];

                if (cellContent == 'e')
                {
                    Cells[x, y].HasFood = true;
                }
                else if (int.TryParse(cellContent.ToString(), out int team))
                {
                    Cells[x, y].StartPosition = (TeamType)team;
                    CreateUnit(unitFactory, teams, (TeamType)team, x, y);
                }
            }
        }
    }

    public void PlaceUnit( Unit unit )
    {
        Cells[unit.X, unit.Y].Units.Add(unit);
        unit.Cell = Cells[unit.X, unit.Y];
    }

    public void AddUnit( Unit unit )
    {
        Cells[unit.X, unit.Y].AddUnit(unit);
        unit.Cell = Cells[unit.X, unit.Y];
    }

    public void RemoveUnit( Unit unit )
    {
        Cells[unit.X, unit.Y].RemoveUnit(unit);
        unit.Cell = null;
    }

    public void MoveUnit( Unit unit, int newX, int newY )
    {
        RemoveUnit( unit );
        unit.X = newX;
        unit.Y = newY;
        AddUnit( unit );
    }

    public bool IsValidPositionToMove( Unit unit, int x, int y )
    {
        if ( IsOutOfBoard(x, y) )
        {
            return false;
        }

        if ( Cells[x, y].HasFood )
        {
            return unit.CanCollectFood && IsNeghbourCell(unit, x, y);
        }

        if (Cells[x, y].Units.Any())
        {
            return false;
        }

        return IsNeghbourCell(unit, x, y);
    }

    public bool IsValidPositionToRevive( Unit unit, int x, int y )
    {
        if ( IsOutOfBoard(x,y) )
        {
            return false;
        }

        if (Cells[x, y].Units.Any())
        {
            return false;
        }

        return Cells[x, y].StartPosition == unit.TeamType;
    }

    public bool IsValidToAttack( Unit attacker, Unit target )
    {
        if (attacker.TeamType == target.TeamType)
        {
            return false;
        }

        int dx = Math.Abs(attacker.X - target.X);
        int dy = Math.Abs(attacker.Y - target.Y);

        // Проверяем, находится ли цель в пределах диапазона атаки
        bool inRangeStraight = dx <= attacker.AttackRangeStraight &&
                               dy <= attacker.AttackRangeStraight && 
                               (dx == 0 || dy == 0);
        bool inRangeDiagonally = dx <= attacker.AttackRangeDiagonally && 
                                 dy <= attacker.AttackRangeDiagonally;

        if (!inRangeStraight && !inRangeDiagonally)
        {
            return false;
        }

        // Проверяем, есть ли препятствия между атакующим и целью 
        int x1 = Math.Min(attacker.X, target.X);
        int x2 = Math.Max(attacker.X, target.X);
        int y1 = Math.Min(attacker.Y, target.Y);
        int y2 = Math.Max(attacker.Y, target.Y);
        if ( inRangeStraight )
        {
            int start;
            int end;
            if (dy == 0)
            {
                start = x1 + 1;
                end = x2;
            }
            else
            {
                start = y1 + 1;
                end = y2;
            }

            for (; start < end; start++)
            {
                var cell = dy == 0 ? Cells[start, y1] : Cells[x1, start];
                if ( cell.HasFood || cell.Units.Count > 0 )
                {
                    return false;
                }
            }
        }
        else
        {
            for (int x = x1 + 1, y = y1 + 1; x < x2 && y < y2; x++, y++)
            {
                if (Cells[x, y].HasFood || Cells[x, y].Units.Count > 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsAnyPlaceToRevive( Team currentTeam )
    {
        foreach (var cell in Cells)
        {
            if ( cell.StartPosition == currentTeam.TeamType &&
                !cell.Units.Any() )
            {
                return true;
            }
        }

        return false;
    }

    private bool IsOutOfBoard( int x, int y )
    {
        return x < 0 || x >= Width || y < 0 || y >= Height;
    }

    private void CreateUnit( IUnitFactory unitFactory, List<Team> teams, TeamType teamType, int x, int y )
    {
        Unit unit = unitFactory.CreateRandomUnit( teamType, x, y);
        var team = teams.FirstOrDefault(t => t.TeamType == unit.TeamType);
        if (team is null )
        {
            team = new Team(teamType);
            teams.Add(team);
        }
        
        team.AddUnit(unit);
        unit.Team = team;

        PlaceUnit( unit );
    }

    private bool IsNeghbourCell( Unit unit, int x, int y )
    {
        // Проверяем, является ли клетка соседней по горизонтали или вертикали
        int dx = Math.Abs(unit.X - x);
        int dy = Math.Abs(unit.Y - y);

        return (dx == 1 && dy == 0) || 
               (dx == 0 && dy == 1) ||
               (dx == 1 && dy == 1);
    }
}