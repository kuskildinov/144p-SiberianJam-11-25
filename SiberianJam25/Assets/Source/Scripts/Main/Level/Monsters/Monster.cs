using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Mesh variants")]
    [SerializeField] private GameObject _goodMesh;
    [SerializeField] private GameObject _badMesh;

    protected LevelMonstersHandler _monstersHandler;

    public virtual void Initialize(LevelMonstersHandler monstersHandler)
    {
        _monstersHandler = monstersHandler;
    }

    #region >>> VISUAL

    public void ShowGoodMesh()
    {
        _goodMesh.gameObject.SetActive(true);
        _badMesh.gameObject.SetActive(false);
    }

    public void ShowBadMesh()
    {
        _goodMesh.gameObject.SetActive(false);
        _badMesh.gameObject.SetActive(true);
    }

    #endregion
}
