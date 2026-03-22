using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePanelButton : InteractableObject
{
    [SerializeField] private int _index;
    private CodePanel _codePanel;

   public void Initialize(CodePanel codePanel)
    {
        _codePanel = codePanel;
    }

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();
        Debug.Log($"ВВод числа {_index}");
        _codePanel.EnterNewNumber(_index.ToString());
    }
}
