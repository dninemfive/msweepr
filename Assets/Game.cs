using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public const float CAMERA_SIZE_FACTOR = 1f;
    public const float BORDER_SIZE_FACTOR = 4f;
    public static (int x, int y) BoardSize = (80, 45);
    public static int NumMines = 800;
    public static Game Instance = null;
    public static Board Board;
    public static System.Random Random = new System.Random();
    public static GameObject TileObject => Instance._tileObject;
    [SerializeField]
    private GameObject _tileObject;
    public static Camera Camera => Instance._camera;
    [SerializeField]
    private Camera _camera;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Board = new Board(BoardSize, NumMines);
        CenterBoardInCamera();
        foreach (Tile t in Board.AllTiles) t.Reveal();
    }
    // Update is called once per frame
    Vector3 TileSize => TileObject.transform.localScale;
    public void CenterBoardInCamera()
    {
        (int xDimension, int yDimension) = BoardSize;
        Vector3 pos = Camera.transform.position;
        pos.x = xDimension / 2.0f - TileSize.x / 2.0f;
        pos.y = yDimension / 2.0f - TileSize.y / 2.0f;
        Camera.transform.position = pos;
        if (xDimension > yDimension)
        {
            Camera.orthographicSize = ((xDimension / 2.0f * CAMERA_SIZE_FACTOR) + (2.0f * BORDER_SIZE_FACTOR * TileSize.x)) / 2.0f;
        }
        else
        {
            Camera.orthographicSize = ((yDimension / 2.0f * CAMERA_SIZE_FACTOR) + (2.0f * BORDER_SIZE_FACTOR * TileSize.y)) / 2.0f;
        }
    }
    public static void End() { }
}
