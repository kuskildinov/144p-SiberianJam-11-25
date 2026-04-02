using UnityEngine;

public class FriendOnRoom : MonoBehaviour
{
    [SerializeField] private DialogComponent _phrases;

    public DialogComponent Phrases => _phrases;
}
