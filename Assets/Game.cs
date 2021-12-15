using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance = null;
    public static Board Board;
    public static GameObject TileObject => Instance._tileObject;
    [SerializeField]
    private GameObject _tileObject;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        Board = new Board(10, 10);
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
