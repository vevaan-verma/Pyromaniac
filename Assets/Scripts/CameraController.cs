using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("References")]
    [SerializeField] private Transform target;
    private new Camera camera;

    [Header("Follow")]
    [SerializeField] private float followSmoothing;
    private float zOffset;

    [Header("Bounds")]
    [SerializeField] private BoxCollider2D mapBounds;
    private float xMin, yMin, xMax, yMax;
    private float camSize;
    private float camRatio;

    private void Start() {

        target = FindObjectOfType<PlayerController>().transform;
        camera = GetComponent<Camera>();

        zOffset = transform.position.z - target.position.z;

        xMin = mapBounds.bounds.min.x;
        yMin = mapBounds.bounds.min.y;
        xMax = mapBounds.bounds.max.x;
        yMax = mapBounds.bounds.max.y;

        camSize = camera.orthographicSize;
        camRatio = ((float) Screen.width / Screen.height) * camSize;

    }

    private void LateUpdate() {

        transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(target.position.x, xMin + camRatio, xMax - camRatio), Mathf.Clamp(target.position.y, yMin + camSize, yMax - camSize), zOffset), followSmoothing * Time.deltaTime); // z value of vector3 should be zero because offset is being added after

    }
}
