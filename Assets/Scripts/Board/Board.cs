    using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private GameObject _tilePrefab;

    private BackgroundTile[,] _allTiles;

    private void Start()
    {
        _allTiles = new BackgroundTile[_width, _height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(_tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + ", " + j + ")";
            }
        }
    }
}
