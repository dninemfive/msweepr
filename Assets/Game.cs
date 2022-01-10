using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public const float CAMERA_SIZE_FACTOR = 1f;
    public const float BORDER_SIZE_FACTOR = 1f;
    public static (int x, int y) BoardSize = (80, 45);
    public static int NumMines = 60;
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
    }
    // Update is called once per frame
    void Update()
    {

    }
    public static void SetUp()
    {

    }
    public static void End() { }
}
