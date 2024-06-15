using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandSlot : Slot {

    public override void SetSelected(bool selected) {

        base.SetSelected(selected);
        playerController.ToggleWand();

    }
}
