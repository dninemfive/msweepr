using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Board
{
    const float CAMERA_SIZE_FACTOR = 1.1f;
    private readonly List<List<Tile>> _board = new List<List<Tile>>();
    public IEnumerable<Tile> AllTiles
    {
        get
        {
            foreach (List<Tile> l in _board) foreach (Tile t in l) yield return t;
        }
    }
    public static IEnumerable<(int x, int y)> AllCoordinatesForDimension((int x, int y) dim)
    {
        (int x, int y) = dim;
        for (int i = 0; i < x; i++) for (int j = 0; j < y; j++) yield return (i, j);
    }
    private static IEnumerable<(int x, int y)> MineCoordinateEnumerable((int x, int y) dim, int numMines)
    {
        List<(int x, int y)> AllCoords = AllCoordinatesForDimension(dim).ToList();
        for(int i = 0; i < numMines; i++)
        {
            if (AllCoords.Any())
            {
                (int x, int y) item = AllCoords[Game.Random.Next(0, AllCoords.Count)];
                yield return item;
                AllCoords.Remove(item);
            }
            else break;
        }
    }
    public static HashSet<(int x, int y)> MineCoordinateSet((int x, int y) dim, int numMines) => new HashSet<(int x, int y)>(MineCoordinateEnumerable(dim, numMines));
    public int MinesRemaining => AllTiles.Where(x => x.Flag == Tile.FlagStatus.Flagged).Count();
    public Board((int x, int y) dimensions, int numMines)
    {
        HashSet<(int x, int y)> MineCoords = MineCoordinateSet(dimensions, numMines);
        (int x, int y) = dimensions;
        for(int i = 0; i < x; i++)
        {
            _board.Add(new List<Tile>());
            for(int j = 0; j < y; j++)
            {
                Tile newTile = GameObject.Instantiate(Game.TileObject, new Vector3(i, j), Quaternion.identity).GetComponent<Tile>();
                _board[i].Add(newTile);
                newTile.Init(i, j, MineCoords.Contains((i, j)));
            }
        }
        CenterBoardInCamera(x, y);
    }
    public Tile this[int x, int y]
    {
        get
        {
            if ((x >= 0 && x < _board.Count) && (y >= 0 && y < _board[x].Count)) return _board[x][y];
            return null;
        }
    }
    public void CenterBoardInCamera(int xDimension, int yDimension)
    {
        Vector3 pos = Game.Camera.transform.position;
        pos.x = xDimension / 2.0f - 0.5f;
        pos.y = yDimension / 2.0f - 0.5f;
        Game.Camera.transform.position = pos;
        if (xDimension > yDimension)
        {
            Game.Camera.orthographicSize = xDimension / 2.0f * CAMERA_SIZE_FACTOR;
        }
        else
        {
            Game.Camera.orthographicSize = yDimension / 2.0f * CAMERA_SIZE_FACTOR;
        }
    }
}
