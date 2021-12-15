using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsMine { get; private set; } = false;
    public IEnumerable<Tile> Neighbors => _neighbors;
    private List<Tile> _neighbors = new List<Tile>();
    public int NeighborMineCount => Neighbors.Where(x => x.IsMine).Count();
    public Board Board => Game.Board;
    public int X, Y;
    public FlagStatus Flag;
    public enum FlagStatus
    {
        None,
        Flagged,
        Question
    }
    public void Init(int x, int y, bool isMine)
    {
        IsMine = isMine;
    }
    public void FindNeighbors()
    {
        _neighbors.Clear();
        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                Tile item = Board[i, j];
                if (item != null)
                {
                    _neighbors.Add(item);
                }
            }
        }
    }
    public void OnClicked()
    {
        Reveal();
    }
    public void Reveal()
    {
        if (IsMine)
        {
            Game.End();
            return;
        }
        if(NeighborMineCount > 0)
        {
            // change texture, include number
            return;
        }
        foreach (Tile t in Neighbors)
        {
            t.Reveal();
        }
    }
}
