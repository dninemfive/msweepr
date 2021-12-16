using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Board
{
    const float CAMERA_SIZE_FACTOR = 10f;
    private List<List<Tile>> _board = new List<List<Tile>>();
    public IEnumerable<Tile> AllTiles
    {
        get
        {
            foreach (List<Tile> l in _board) foreach (Tile t in l) yield return t;
        }
    }
    public int MinesRemaining => AllTiles.Where(x => x.Flag == Tile.FlagStatus.Flagged).Count();
    public Board(int xDimension, int yDimension)
    {
        for(int i = 0; i < xDimension; i++)
        {
            _board.Add(new List<Tile>());
            for(int j = 0; j < yDimension; j++)
            {
                Tile newTile = GameObject.Instantiate(Game.TileObject, new Vector3(i, j), Quaternion.identity).GetComponent<Tile>();
                _board[i].Add(newTile);
                newTile.Init(i, j, Mathf.RoundToInt(UnityEngine.Random.value) == 1);
            }
        }
        CenterBoardInCamera(xDimension, yDimension);
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
        // https://answers.unity.com/questions/174002/what-is-the-relationship-between-camera-size-units.html
        // http://gamedesigntheory.blogspot.com/2010/09/controlling-aspect-ratio-in-unity.html
        Rect rect = Game.Camera.rect;
        float aspectRatio = Game.Camera.aspect;
        if (xDimension > yDimension)
        {
            rect.width = xDimension * CAMERA_SIZE_FACTOR;
            rect.height = rect.width / aspectRatio;
        }
        else
        {
            rect.height = yDimension * CAMERA_SIZE_FACTOR;
            rect.width = rect.height * aspectRatio;
        }
        Game.Camera.rect = rect;
    }
}
