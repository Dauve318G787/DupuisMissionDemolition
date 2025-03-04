using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    // Add these for the rubber band
    private LineRenderer rubberBandLine; // To visualize the rubber band
    private Vector3 rubberBandAnchorPos; // The anchor position of the rubber band (slingshot point)

    void Awake() {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;

        // Initialize the rubber band visual (LineRenderer)
        rubberBandLine = gameObject.AddComponent<LineRenderer>();
        rubberBandLine.material = new Material(Shader.Find("Sprites/Default"));
        rubberBandLine.startWidth = 0.05f; // Adjust thickness
        rubberBandLine.endWidth = 0.05f;
        rubberBandLine.startColor = Color.black; // Adjust color
        rubberBandLine.endColor = Color.black;
        rubberBandLine.positionCount = 2;
        rubberBandLine.enabled = false; // Disable it initially
    }

    void OnMouseEnter() {
        print("Slingshot:OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit() {
        print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    void OnMouseDown() {
        aimingMode = true;

        // Instantiate the projectile
        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPoint.transform.position;

        // Disable physics on the projectile while aiming
        projectile.GetComponent<Rigidbody>().isKinematic = true;

        // Set up the rubber band (LineRenderer)
        rubberBandLine.enabled = true; // Enable the rubber band line
        rubberBandAnchorPos = launchPoint.transform.position;
    }

    void Update() {
        if (!aimingMode) return;

        // Get the mouse position in 3D space
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Calculate the delta position from the slingshot
        Vector3 mouseDelta = mousePos3D - launchPos;
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Update the projectile position based on mouse delta
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        // Update the rubber band line positions
        rubberBandLine.SetPosition(0, rubberBandAnchorPos); // Starting point (slingshot)
        rubberBandLine.SetPosition(1, projectile.transform.position); // Ending point (projectile)

        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;

            // Release the projectile and enable physics
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

            // Set the projectile's velocity
            projRB.velocity = -mouseDelta * velocityMult;

            // Switch camera view
            FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            FollowCam.POI = projectile;

            // Instantiate the projectile line (visual effect)
            Instantiate<GameObject>(projLinePrefab, projectile.transform);

            // Clear rubber band visual
            rubberBandLine.enabled = false;

            projectile = null;

            MissionDemolition.SHOT_FIRED();
        }
    }
}
