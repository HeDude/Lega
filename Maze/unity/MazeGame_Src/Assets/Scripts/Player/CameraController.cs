using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    //Properties
    [SerializeField] private float maxSensitivity;
    [SerializeField] private float sensitivity;
    [SerializeField] private float smoothing;

    private GameObject player;

    Vector2 smoothedVelocity;
    Vector2 currentLookingPosition;

    //Start is called before the first frame update
    private void Start()
    {
        //Locks the cursor to the center of the screen and hides it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Sets sensitivity and smoothing to 2
        maxSensitivity = 2f;
        sensitivity = maxSensitivity;
        smoothing = 2f;

        //Finds player in scene
        player = transform.parent.gameObject;
    }

    //Update is called once per frame
    private void Update()
    {
        //Handles the rotation of the camera
        RotateCamera();
    }

    //Handles the rotation of the camera
    private void RotateCamera()
    {
        //Gets mouse position
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        inputValues = Vector2.Scale(inputValues, new Vector2(sensitivity * smoothing, sensitivity * smoothing));

        //Lerp the smoothed velocity with smoothing
        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);

        //Sets current looking position
        currentLookingPosition += smoothedVelocity;

        //Clamps looking position
        currentLookingPosition.y = Mathf.Clamp(currentLookingPosition.y, -80f, 60f);

        //Updates player rotation
        transform.localRotation = Quaternion.AngleAxis(-currentLookingPosition.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(currentLookingPosition.x, player.transform.up);
    }

    //Disables sensitivity
    public void DisableSensitivity()
    {
        sensitivity = 0.0f;
    }

    //Enables sensitivity
    public void EnableSensitivity()
    {
        sensitivity = maxSensitivity;
    }
}
