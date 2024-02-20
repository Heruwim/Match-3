using System.Collections;
using UnityEngine;

public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _offSet;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject[] _dots;
    
    public GameObject[,] AllDots;
    public GameObject DestroyEffect;
    public GameState CurreState = GameState.move;
    public Dot CurrentDot;

    private BackgroundTile[,] _allTiles;
    private FindMatces _findMatces;

    public int Width
    {
        get { return _width; }
        set { _width = value; }
    }
    public int Height
    {
        get { return _height; }
        set { _height = value; }
    }

    private void Start()
    {
        _findMatces = FindObjectOfType<FindMatces>();
        _allTiles = new BackgroundTile[_width, _height];
        AllDots = new GameObject[_width, _height];

        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j + _offSet);
                GameObject backgroundTile = Instantiate(_tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform;
                backgroundTile.name = "(" + i + ", " + j + ")";
                int dotToUse = Random.Range(0, _dots.Length);
                int maxIterations = 0;
                while (MatchesAt(i, j, _dots[dotToUse ]) && maxIterations < 100)
                {
                    dotToUse = Random.Range(0, _dots.Length);
                    maxIterations++;
                }
                maxIterations = 0;

                GameObject dot = Instantiate(_dots[dotToUse], tempPosition, Quaternion.identity);
                dot.GetComponent<Dot>().Row = j;
                dot.GetComponent<Dot>().Column = i;
                dot.transform.parent = this.transform;
                dot.name = "(" + i + ", " + j + ")";
                AllDots[i, j] = dot;
            }
        }
        
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (AllDots[column - 1, row].tag == piece.tag && AllDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }

            if (AllDots[column, row - 1].tag == piece.tag && AllDots[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if(row > 1)
            {
                if (AllDots[column, row - 1].tag == piece.tag && AllDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (AllDots[column - 1, row].tag == piece.tag && AllDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (AllDots[column, row].GetComponent<Dot>().IsMatched)
        {
            //How many elements are in the matched pieces list from findmatches?
            if(_findMatces.CurrentMatches.Count == 4 || _findMatces.CurrentMatches.Count == 7)
            {
                _findMatces.CheckBombs();
            }
           
            GameObject particle = Instantiate(DestroyEffect, AllDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, 0.5f );
            Destroy(AllDots[column, row]);
            AllDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (AllDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        _findMatces.CurrentMatches.Clear();
        StartCoroutine(DecreaseRowCorutine());
    }

    private IEnumerator DecreaseRowCorutine()
    {
        int nullCount = 0;
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (AllDots[i, j] == null)
                {
                    nullCount++;
                }
                else if(nullCount > 0)
                {
                    AllDots[i, j].GetComponent<Dot>().Row -= nullCount;
                    AllDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(FillBoardCorutine());
    }

    private void RefilldBoard()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (AllDots[i, j] == null)
                {
                    Vector2 tempPosition = new Vector2(i, j + _offSet);
                    int dotToUse = Random.Range(0, _dots.Length);
                    GameObject piece = Instantiate(_dots[dotToUse], tempPosition, Quaternion.identity);
                    AllDots[i, j] = piece;
                    piece.GetComponent<Dot>().Row = j;
                    piece.GetComponent<Dot>().Column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (AllDots[i, j] != null)
                {
                    if (AllDots[i, j].GetComponent<Dot>().IsMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCorutine()
    {
        RefilldBoard();
        yield return new WaitForSeconds(0.5f);
        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(0.5f);
            DestroyMatches();
        }
        _findMatces.CurrentMatches.Clear();
        CurrentDot = null;
        yield return new WaitForSeconds(0.5f);
        CurreState = GameState.move;
    } 
}
