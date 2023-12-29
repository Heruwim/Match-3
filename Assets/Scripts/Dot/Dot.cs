using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private float _swipeAngle = 0f;
    [SerializeField] private int _column;
    [SerializeField] private int _row;
    [SerializeField] private int _targetX;
    [SerializeField] private int _targetY;

    private Vector2 _firstTouchPosition;
    private Vector2 _finalTouchPosition;
    private Vector2 _tempPosition;
    private GameObject _otherDot;
    private Board _board;

    private void Start()
    {
        _board = FindObjectOfType<Board>();
        _targetX = (int)transform.position.x;
        _targetY = (int)transform.position.y;
        _row = _targetY;
        _column = _targetX;
    }

    private void Update()
    {
        _targetX = _column;
        _targetY = _row;

        if(Mathf.Abs(_targetX - transform.position.x) < 0.1)
        {
            //Move towards the target
            _tempPosition = new Vector2(_targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, _tempPosition, 0.4f);
        }
        else
        {
            //Directly set the position
            _tempPosition = new Vector2(_targetX, transform.position.y);
            transform.position = _tempPosition;
            _board.AllDots[_column, _row] = this.gameObject;
        }
        if (Mathf.Abs(_targetY - transform.position.y) > 0.1)
        {
            //Move towards the target
            _tempPosition = new Vector2(transform.position.x, _targetY);
            transform.position = Vector2.Lerp(transform.position, _tempPosition, 0.4f);
        }
        else
        {
            //Directly set the position
            _tempPosition = new Vector2(transform.position.x, _targetY);
            transform.position = _tempPosition;
            _board.AllDots[_column, _row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        _firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(_firstTouchPosition);
    }

    private void OnMouseUp()
    {
        _finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    private void CalculateAngle()
    {
        _swipeAngle = Mathf.Atan2(_finalTouchPosition.y - _firstTouchPosition.y, _finalTouchPosition.x - _firstTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(_swipeAngle);
        MovePieces();
    }

    private void MovePieces()
    {
        if(_swipeAngle > -45 && _swipeAngle <= 45 && _column < _board.Width)
        {
            //Right swipe
            _otherDot = _board.AllDots[_column + 1, _row];
            _otherDot.GetComponent<Dot>()._column -= 1;
            _column += 1;
        }
        else if(_swipeAngle > 45 && _swipeAngle <= 135 && _row < _board.Height)
        {
            //Up swipe
            _otherDot = _board.AllDots[_column, _row + 1];
            _otherDot.GetComponent<Dot>()._row -= 1;
            _row += 1;
        }
        else if ((_swipeAngle > 135 || _swipeAngle <= -135) && _column > 0)
        {
            //Left swipe
            _otherDot = _board.AllDots[_column - 1, _row];
            _otherDot.GetComponent<Dot>()._column += 1;
            _column -= 1;
        }
        else if (_swipeAngle < -45 && _swipeAngle >= -135 && _row > 0)
        {
            //Down swipe
            _otherDot = _board.AllDots[_column, _row - 1];
            _otherDot.GetComponent<Dot>()._row += 1;
            _row -= 1;
        }
    }
}
