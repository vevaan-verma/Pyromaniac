using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action")]
public class ActionProperties : ScriptableObject {

    [Header("Properties")]
    [SerializeField] private string actionName;
    [SerializeField] private Sprite icon;
    [SerializeField] private ActionType actionType;

    public string GetName() => actionName;

    public Sprite GetIcon() => icon;

    public ActionType GetActionType() => actionType;

}
