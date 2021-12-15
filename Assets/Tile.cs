using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsMine { get; private set; } = false;
    public IEnumerable<Tile> Neighbors;
    public int NeighborMineCount => Neighbors.Where(x => x.IsMine).Count();
    public FlagStatus Flag;
    public enum FlagStatus
    {
        None,
        Flagged,
        Question
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
