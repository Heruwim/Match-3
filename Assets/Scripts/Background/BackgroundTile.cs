using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    [SerializeField] private GameObject[] _dots;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int dotToUse = Random.Range(0, _dots.Length);
        GameObject dot = Instantiate(_dots[dotToUse], transform.position, Quaternion.identity);
        dot.transform.parent = this.transform;
        dot.name = this.gameObject.name;
    }
}
