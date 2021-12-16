using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public bool IsMine { get; private set; } = false;
    private bool revealed = false;
    public IEnumerable<Tile> Neighbors
    {
        get
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    Tile item = Board[X + i, X + j];
                    if (item != null) yield return item;
                }
            }
        }
    }
    public int NeighborMineCount => Neighbors.Where(x => x.IsMine).Count();
    public Board Board => Game.Board;
    public int X { get; private set; }
    public int Y { get; private set; }
    public FlagStatus Flag;
    public Text Text;
    public enum FlagStatus
    {
        None,
        Flagged,
        Question
    }
    public void Init(int x, int y, bool isMine)
    {
        X = x;
        Y = y;
        IsMine = isMine;
    }
    void OnMouseDown()
    {
        Reveal();
    }
    public void Reveal()
    {
        if (revealed) return;
        revealed = true;
        transform.Rotate(new Vector3(0, 0, 180));
        // rotate the text to keep it right-side-up
        Text.transform.Rotate(new Vector3(0, 0, 180));
        if (IsMine)
        {
            Text.text = "*";
            Game.End();
            return;
        }
        if(NeighborMineCount > 0)
        {
            Text.text = "" + NeighborMineCount;
            return;
        }
        foreach (Tile t in Neighbors)
        {
            t.Reveal();
        }
    }
}
