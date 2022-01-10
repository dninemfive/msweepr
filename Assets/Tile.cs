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
                    Tile item = Board[X + i, Y + j];
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
        None = 0,
        Flagged = 1,
        Question = 2
    }
    public void Init(int x, int y, bool isMine)
    {
        X = x;
        Y = y;
        IsMine = isMine;
    }
    // https://answers.unity.com/questions/1350065/onmousedown-for-right-mouse.html
    private void OnMouseOver()
    {
        if (revealed) return;
        if(Input.GetMouseButtonDown(0))
        {
            Reveal();
        }
        if(Input.GetMouseButtonDown(1))
        {
            RotateFlag();
        }
    }
    public void Reveal(bool IsClick = true)
    {
        if (revealed) return;
        Text.text = "";
        revealed = true;
        transform.Rotate(new Vector3(0, 0, 180));
        // rotate the text to keep it right-side-up
        Text.transform.Rotate(new Vector3(0, 0, 180));
        if (IsMine)
        {
            Text.text = "*";
            if (IsClick) Text.color = Color.red;
            if (!Game.Over)
            {
                Game.End();
            }
            return;
        }
        if(NeighborMineCount > 0)
        {
            Text.text = "" + NeighborMineCount;
            Text.color = TextColor(NeighborMineCount);
            return;
        }
        foreach (Tile t in Neighbors)
        {
            t.Reveal();
        }
    }
    public static Color TextColor(int numAdjacents)
    {
        // TODO: this set of colors works well with most "abnormal trichromacy" forms of colorblindness (except blue-weak), but largely fails at mono/dichromacy forms.
        //       not high priority since the numbers are still visible (though the orange is a bit too close to the gray to work super well) and distinct, but color
        //       settings with colorblind presets would be a good idea.
        switch (numAdjacents)
        {
            case 1: return new Color(127 / 255f, 0f, 127 / 255f); // purple
            case 2: return Color.blue;
            case 3: return new Color(24 / 255f, 89/255f, 21 / 255f); // slightly darker green
            case 4: return new Color(255 / 255f, 127 / 255f, 0f); // orange
            // tiles with more than four adjacents are rare so i'm too lazy to put together a proper color set for them
            default: return Color.red;
        }
    }
    public void RotateFlag()
    {
        Flag = (FlagStatus)(((int)Flag + 1) % 3);
        switch (Flag)
        {
            case FlagStatus.None:
                Text.text = "";
                return;
            case FlagStatus.Flagged:
                Text.text = "!";
                return;
            case FlagStatus.Question:
                Text.text = "?";
                return;
        }
    }
}
