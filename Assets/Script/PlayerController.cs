using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform playerTrans;
    [SerializeField] private float speed;

    void Start()
    {
        playerTrans = this.transform;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector3 movement = Vector3.zero;

        if (keyboard.wKey.wasPressedThisFrame) movement.z += 1;
        if (keyboard.sKey.wasPressedThisFrame) movement.z -= 1;
        if (keyboard.aKey.wasPressedThisFrame) movement.x -= 1;
        if (keyboard.dKey.wasPressedThisFrame) movement.x += 1;
        playerTrans.position += movement;
    }
}
