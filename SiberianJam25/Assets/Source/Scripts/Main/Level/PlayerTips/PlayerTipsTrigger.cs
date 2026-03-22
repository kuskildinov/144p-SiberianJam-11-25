using UnityEngine;

public class PlayerTipsTrigger : MonoBehaviour
{
    [SerializeField] private PlayerTipsType _type;

    public PlayerTipsType Type => _type;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, new Vector3(5f,2f,2f));
    }
}
