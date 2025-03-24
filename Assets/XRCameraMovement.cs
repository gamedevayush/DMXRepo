using UnityEngine;
using UnityEngine.XR;

public class XRCameraMovement : MonoBehaviour
{
    public XRNode inputSourceLeft = XRNode.LeftHand;
    public XRNode inputSourceRight = XRNode.RightHand;
    public float movementSpeed = 2.0f;
    public float rotationSpeed = 45.0f;

    private Vector2 inputAxis;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();
    }

    void MovePlayer()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceLeft);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        Vector3 direction = transform.right * inputAxis.x + transform.forward * inputAxis.y;
        direction.y = 0;

        characterController.Move(direction * movementSpeed * Time.deltaTime);
    }

    void RotatePlayer()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSourceRight);
        Vector2 rotationInput;
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out rotationInput);

        if (Mathf.Abs(rotationInput.x) > 0.2f)
        {
            transform.Rotate(Vector3.up, rotationInput.x * rotationSpeed * Time.deltaTime);
        }
    }
}
