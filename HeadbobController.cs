using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbobController : MonoBehaviour
{
    [Header("Headbob Parameters")]
    [SerializeField] private bool headbobEnable = true;

    [SerializeField][Range(0.0f, 0.005f)] private float headbobAmplitude = 0.0015f;
    [SerializeField][Range(0.0f, 30.0f)] private float headbobFrequency = 10.0f;

    [SerializeField] private Transform mainCamera = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 3.0f;
    Vector3 startPos;
    private CharacterController characterController;

    void Start()
    {
        startPos = mainCamera.localPosition;
        characterController = gameObject.GetComponentInParent<CharacterController>();
    }

    void Update()
    {
        if (!headbobEnable) return;

        CheckMotion();
        mainCamera.LookAt(FocusTarget());
    }

    private void CheckMotion()
    {
        float speed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;

        ResetPosition();

        if (speed < toggleSpeed) return;
        if (!characterController.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * headbobFrequency) * headbobAmplitude * 4;
        pos.x += Mathf.Sin(Time.time * headbobFrequency / 2) * headbobAmplitude * 2;
        return pos;
    }

    private void PlayMotion(Vector3 motion)
    {
        mainCamera.localPosition += motion;
    }

    private void ResetPosition()
    {
        if (mainCamera.localPosition == startPos) return;
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, startPos, 1 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + mainCamera.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
}
