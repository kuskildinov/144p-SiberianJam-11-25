using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject
{
    [SerializeField] private int _index;
    [SerializeField] private Collider _collider;

    private Rigidbody _rigidbody;

    public int Index => _index;
    public Rigidbody Rigidbody => _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();

        player.TakeItem(this);
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        _collider.enabled = false;
    }
}
