using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatces : MonoBehaviour
{
    private Board _board;
    
    public List<GameObject> CurrentMatches = new List<GameObject>();

    private void Start()
    {
        _board = FindAnyObjectByType<Board>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCorutine());
    }

    private List<GameObject> IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.IsRowBomb)
        {
            CurrentMatches.Union(GetRowPieces(dot1.Row));
        }

        if (dot2.IsRowBomb)
        {
            CurrentMatches.Union(GetRowPieces(dot2.Row));
        }

        if (dot3.IsRowBomb)
        {
            CurrentMatches.Union(GetRowPieces(dot3.Row));
        }

        return currentDots;
    }
    
    private List<GameObject> IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.IsColumnBomb)
        {
            CurrentMatches.Union(GetColumnPieces(dot1.Column));
        }

        if (dot2.IsColumnBomb)
        {
            CurrentMatches.Union(GetColumnPieces(dot2.Column));
        }

        if (dot3.IsColumnBomb)
        {
            CurrentMatches.Union(GetColumnPieces(dot3.Column));
        }

        return currentDots;
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!CurrentMatches.Contains(dot))
        {
            CurrentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().IsMatched = true;
    }

    private void GetNearByPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCorutine()
    {
        yield return new WaitForSeconds(0.2f);
        for(int i = 0; i < _board.Width; i++) 
        {
            for (int j = 0; j < _board.Height; j++)
            {
                GameObject currentDot = _board.AllDots[i, j];
                if (currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();
                    if(i > 0 && i < _board.Width - 1)
                    {
                        GameObject leftDot = _board.AllDots[i - 1, j];
                        GameObject rightDot = _board.AllDots[i + 1, j];
                        if(leftDot != null && rightDot != null)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                CurrentMatches.Union(IsRowBomb(leftDotDot, currentDotDot, rightDotDot));
                                CurrentMatches.Union(IsColumnBomb(leftDotDot, currentDotDot,  rightDotDot));
                                GetNearByPieces(leftDot, currentDot, rightDot);                  
                            }
                        }
                    }

                    if (j > 0 && j < _board.Height - 1)
                    {
                        GameObject upDot = _board.AllDots[i, j + 1];
                        GameObject downDot = _board.AllDots[i, j - 1];
                        if (upDot != null && downDot != null)
                        {
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                CurrentMatches.Union(IsColumnBomb(upDotDot, currentDotDot,  downDotDot));
                                CurrentMatches.Union(IsRowBomb(upDotDot, currentDotDot, downDotDot));
                                GetNearByPieces(upDot, currentDot, downDot);
                               
                            }
                        }
                    }
                }
            }
        }
    }

    private List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < _board.Height; i++)
        {
            if (_board.AllDots[column, i] != null)
            {
                dots.Add(_board.AllDots[column, i]);
                _board.AllDots[column, i].GetComponent<Dot>().IsMatched = true;
            }
        }

        return dots;
    }

    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < _board.Width; i++)
        {
            for (int j = 0; j < _board.Height; j++)
            {
                //Check is the piece exists
                if(_board.AllDots[i, j] != null)
                {
                    // Check the tag on that dot
                    if (_board.AllDots[i, j].tag == color)
                    {
                        // Set the dot to be matched
                        _board.AllDots[i, j ].GetComponent<Dot>().IsMatched = true;
                    }
                }
            }
        }
    }

    private List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < _board.Width; i++)
        {
            if (_board.AllDots[i, row] != null)
            {
                dots.Add(_board.AllDots[i, row]);
                _board.AllDots[i, row].GetComponent<Dot>().IsMatched = true;
            }
        }

        return dots;
    }

    public void CheckBombs()
    {
        // Did the player move something?
        if(_board.CurrentDot != null)
        {
            // Is the piece they moved matched?
            if (_board.CurrentDot.IsMatched)
            {
                // Make it unmatched
                _board.CurrentDot.IsMatched = false;
                // Decide what kind of bomb to make
                /*
                int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 50)
                {
                    // Make a row bomb
                    _board.CurrentDot.MakeRowBomb();
                }
                else if (typeOfBomb >= 50)
                {
                    // Make a column bomb
                    _board.CurrentDot.MakeColumnBomb();
                }
                */
                if((_board.CurrentDot.SwipeAngle > -45 && _board.CurrentDot.SwipeAngle <= 45)
                    || (_board.CurrentDot.SwipeAngle < -135 && _board.CurrentDot.SwipeAngle >= 135))
                {
                    _board.CurrentDot.MakeRowBomb();
                }
                else
                {
                    _board.CurrentDot.MakeColumnBomb();
                }
            }
            // Is the other piece matched?
            else if(_board.CurrentDot != null)
            {
                Dot otherDot = _board.CurrentDot.OtherDot.GetComponent<Dot>();
                // Is the other dot is matched?
                if(otherDot.IsMatched)
                {
                    // Make it unmatched
                    otherDot.IsMatched = false;
                    /*
                    // Deside what kind of bomb to make
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        // Make a row bomb
                        otherDot.OtherDot.GetComponent<Dot>().MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        // Make a column bomb
                        otherDot.OtherDot.GetComponent<Dot>().MakeColumnBomb();
                    }
                    */
                    if ((_board.CurrentDot.SwipeAngle > -45 && _board.CurrentDot.SwipeAngle <= 45)
                    || (_board.CurrentDot.SwipeAngle < -135 && _board.CurrentDot.SwipeAngle >= 135))
                    {
                        otherDot.GetComponent<Dot>().MakeRowBomb();
                    }
                    else
                    {
                        otherDot.GetComponent<Dot>().MakeColumnBomb();
                    }
                }
            }
        }
    }
}
