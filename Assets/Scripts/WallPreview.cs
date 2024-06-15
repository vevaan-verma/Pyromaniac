using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPreview : MonoBehaviour {

    [Header("References")]
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Points")]
    // points 1 and 2 must be evenly spaced from the center of the wall preview
    [SerializeField] private Transform point1;
    [SerializeField] private Transform point2;
    [SerializeField] private float wallCheckLeniency;

    [Header("Placement")]
    [SerializeField] private Color canPlaceColor;
    [SerializeField] private Color cannotPlaceColor;
    private bool canPlace;

    private void Update() {

        RaycastHit2D hit1 = Physics2D.Raycast(point1.position, Vector2.down, wallCheckLeniency, wallMask);
        RaycastHit2D hit2 = Physics2D.Raycast(point2.position, Vector2.down, wallCheckLeniency, wallMask);

        if (hit1 && hit2) { // can place wall

            canPlace = true;
            spriteRenderer.color = canPlaceColor;
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, hit1.transform.position.y + (hit1.transform.localScale.y / 2f), transform.position.z); // place wall preview on the surface of the hit object

        } else { // cannot place wall

            canPlace = false;
            spriteRenderer.color = cannotPlaceColor;
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z); // place wall preview on mouse position

        }

        // keep wall preview with mouse
        // transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, FindObjectOfType<PlayerController>().transform.position.y - 0.5f, transform.position.z); (moves with player on y axis)

    }

    public bool CanPlaceWall() => canPlace;

}
