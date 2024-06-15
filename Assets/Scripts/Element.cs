using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Element")]
public class Element : ScriptableObject {

    [Header("Color")]
    [SerializeField] private Color playerColor;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Color slotColor;

    public Color GetColor() => playerColor;

    public Gradient GetGradient() => gradient;

    public Color GetSlotColor() => slotColor;

}
