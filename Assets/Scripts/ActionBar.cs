using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {

    [Header("Slots")]
    [SerializeField] private ActionBarSlot[] slots;
    private int selectedSlotIndex;

    private void Awake() {

        selectedSlotIndex = -1;

    }

    public void AddItem(ActionProperties itemProperties) {

        for (int i = 0; i < slots.Length; i++) {

            if (slots[i].IsEmpty()) {

                slots[i].SetSlotItem(itemProperties);
                break;

            }
        }
    }

    public void SetItem(ActionProperties itemProperties, int slotIndex) {

        if (slotIndex < 0 || slotIndex >= slots.Length) return;

        slots[slotIndex].SetSlotItem(itemProperties);

    }

    public void SelectSlot(int slotIndex) { // slot index is the number key that was clicked minus one

        if (slotIndex < 0 || slotIndex >= slots.Length) return; // make sure slot index is within bounds

        if (slots[slotIndex].IsEmpty()) return; // make sure slot is not empty (no item in slot)

        // if the same slot is clicked, return
        if (selectedSlotIndex == slotIndex)
            return;

        // if a slot is already selected, deselect it
        if (selectedSlotIndex != -1)
            slots[selectedSlotIndex].SetSelected(false);

        // select new slot
        selectedSlotIndex = slotIndex;
        slots[selectedSlotIndex].SetSelected(true);

    }
}
