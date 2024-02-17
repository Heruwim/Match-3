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
                    if(i > 0 && i < _board.Width - 1)
                    {
                        GameObject leftDot = _board.AllDots[i - 1, j];
                        GameObject rightDot = _board.AllDots[i + 1, j];
                        if(leftDot != null && rightDot != null)
                        {
                            if(leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                            {
                                if(currentDot.GetComponent<Dot>().IsRowBomb
                                    || leftDot.GetComponent<Dot>().IsRowBomb
                                    || rightDot.GetComponent<Dot>().IsRowBomb)
                                {
                                    CurrentMatches.Union(GetRowPieces(j));
                                }

                                if (currentDot.GetComponent<Dot>().IsColumnBomb)
                                {
                                    CurrentMatches.Union(GetColumnPieces(i));
                                }

                                if (leftDot.GetComponent<Dot>().IsColumnBomb)
                                {
                                    CurrentMatches.Union(GetColumnPieces(i - 1));
                                }

                                if (rightDot.GetComponent<Dot>().IsColumnBomb)
                                {
                                    CurrentMatches.Union(GetColumnPieces(i + 1));
                                }

                                if (!CurrentMatches.Contains(leftDot))
                                {
                                    CurrentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().IsMatched = true;

                                if (!CurrentMatches.Contains(rightDot))
                                {
                                    CurrentMatches.Add(rightDot);
                                }
                                rightDot.GetComponent<Dot>().IsMatched = true;
                                if (!CurrentMatches.Contains(currentDot))
                                {
                                    CurrentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().IsMatched = true;
                            }
                        }
                    }

                    if (j > 0 && j < _board.Height - 1)
                    {
                        GameObject upDot = _board.AllDots[i, j + 1];
                        GameObject downDot = _board.AllDots[i, j - 1];
                        if (upDot != null && downDot != null)
                        {
                            if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                            {
                                if (currentDot.GetComponent<Dot>().IsColumnBomb
                                    || upDot.GetComponent<Dot>().IsColumnBomb
                                    || downDot.GetComponent<Dot>().IsColumnBomb)
                                {
                                    CurrentMatches.Union(GetColumnPieces(i));
                                }

                                if (currentDot.GetComponent<Dot>().IsRowBomb)
                                {
                                    CurrentMatches.Union(GetRowPieces(j));
                                }

                                if (upDot.GetComponent<Dot>().IsRowBomb)
                                {
                                    CurrentMatches.Union(GetRowPieces(j + 1));
                                }

                                if (downDot.GetComponent<Dot>().IsRowBomb)
                                {
                                    CurrentMatches.Union(GetRowPieces(j - 1));
                                }

                                if (!CurrentMatches.Contains(upDot))
                                {
                                    CurrentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().IsMatched = true;
                                if (!CurrentMatches.Contains(downDot))
                                {
                                    CurrentMatches.Add(downDot);
                                }
                                downDot.GetComponent<Dot>().IsMatched = true;
                                if (!CurrentMatches.Contains(currentDot))
                                {
                                    CurrentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().IsMatched = true;
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
}
