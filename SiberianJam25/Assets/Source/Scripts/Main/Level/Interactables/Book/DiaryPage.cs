using UnityEngine;

public class DiaryPage : InteractableObject
{
    [SerializeField] private SheetData _data;

    public override void TryInteract(Player player = null)
    {
        base.TryInteract(player);

        player.OnDiaryPageTaked(_data);
        this.gameObject.SetActive(false);
    }
}
