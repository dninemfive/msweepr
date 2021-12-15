using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Board
{
    private List<List<Tile>> _board = new List<List<Tile>>();
    public IEnumerable<Tile> AllTiles
    {
        get
        {
            foreach (List<Tile> l in _board) foreach (Tile t in l) yield return t;
        }
    }
    public Board(int xDimension, int yDimension)
    {
        for(int i = 0; i < xDimension; i++)
        {
            _board.Add(new List<Tile>());
            for(int j = 0; j < yDimension; j++)
            {
                Tile newTile = GameObject.Instantiate(Game.TileObject, new Vector3(i, j), Quaternion.identity).GetComponent<Tile>();
                _board[i].Add(newTile);
                newTile.Init(Mathf.RoundToInt(UnityEngine.Random.value) == 1 ? true : false);
            }
        }
        foreach(Tile t in AllTiles)
        {
            t.FindNeighbors();
        }
    }
    public Tile this[int x, int y]
    {
        get
        {
            if ((x >= 0 && x < _board.Count) && (y >= 0 && y < _board[x].Count)) return _board[x][y];
            return null;
        }
    }
}
