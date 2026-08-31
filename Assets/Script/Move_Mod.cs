using UnityEngine;

public class MoveModule : MonoBehaviour
{
    public float speed = 3;
    [SerializeField, HideInInspector] private Vector3 direction = Vector3.right;
    [SerializeField] private float lifeCycle = 7;

    public Vector3 Velocity => direction * speed;

    public void SetDirection(Vector3 newDirection)
    {
        if (newDirection.sqrMagnitude <= 0f)
        {
            Debug.LogWarning("Move direction cannot be zero.", this);
            return;
        }

        direction = newDirection.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lifeCycle <= 0) Destroy(this.gameObject);
        lifeCycle -= Time.fixedDeltaTime;
        Move();
    }

    void Move()
    {
        transform.position += Velocity * Time.fixedDeltaTime;
    }
}
