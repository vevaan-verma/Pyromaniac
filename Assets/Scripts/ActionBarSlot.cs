using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarSlot : Slot {

    [Header("References")]
    [SerializeField] private Image icon;

    [Header("Slot")]
    private ActionProperties slotItem;

    public void SetSlotItem(ActionProperties item) {

        slotItem = item;
        icon.sprite = item.GetIcon();

    }

    public override void SetSelected(bool selected) {

        base.SetSelected(selected);

        switch (slotItem.GetActionType()) {

            case ActionType.WaterElement:

                playerController.CycleElement(ActionType.WaterElement);
                break;

            case ActionType.EarthElement:

                playerController.CycleElement(ActionType.EarthElement);
                break;

            case ActionType.FireElement:

                playerController.CycleElement(ActionType.FireElement);
                break;

            case ActionType.AirElement:

                playerController.CycleElement(ActionType.AirElement);
                break;

        }
    }

    public bool IsEmpty() => slotItem == null;

    public ActionProperties GetItem() => slotItem;

}
