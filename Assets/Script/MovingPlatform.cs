using UnityEngine;

[DisallowMultipleComponent]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private MoveModule moveModule;

    public Vector3 Velocity => moveModule != null
        ? moveModule.Velocity
        : Vector3.zero;

    private void Awake()
    {
        if (moveModule == null)
        {
            moveModule = GetComponent<MoveModule>();
        }

        if (moveModule == null)
        {
            Debug.LogError("MovingPlatform requires a MoveModule.", this);
        }
    }
}
