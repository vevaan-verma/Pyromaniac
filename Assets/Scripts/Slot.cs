using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Image fillImage;
    protected PlayerController playerController;

    [Header("Selection")]
    [SerializeField] private Color selectedColor;
    [SerializeField] private float selectFadeDuration;
    private Tweener selectTweener;
    private Color startColor;
    private bool isSelected;

    private void Start() {

        playerController = FindObjectOfType<PlayerController>();
        startColor = fillImage.color;

    }

    public virtual void SetSelected(bool selected) {

        isSelected = selected;

        if (selectTweener != null && selectTweener.IsActive())
            selectTweener.Kill();

        if (isSelected)
            selectTweener = fillImage.DOColor(selectedColor, selectFadeDuration);
        else
            selectTweener = fillImage.DOColor(startColor, selectFadeDuration);

    }
}
