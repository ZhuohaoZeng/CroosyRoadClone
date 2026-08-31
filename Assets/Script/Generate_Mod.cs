using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    public float interval;
    public float timer;

    private bool shouldOverrideDirection;
    private Vector3 instanceDirection;

    public void SetSpawnDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude <= 0f)
        {
            Debug.LogWarning("Spawn direction cannot be zero.", this);
            return;
        }

        instanceDirection = direction.normalized;
        shouldOverrideDirection = true;
    }

    void Start()
    {
        timer = interval;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!blockPrefab)
        {
            Debug.LogError("Hi, you forgot to set blockPrefab.");
            return;
        }
        if(timer <= 0)
        {
            generate();
            timer = interval;
        }
        timer -= Time.deltaTime;
    }

    void generate()
    {
        GameObject instance = Instantiate(
            blockPrefab,
            transform.position,
            Quaternion.identity
        );

        if (!shouldOverrideDirection) return;

        MoveModule moveModule = instance.GetComponent<MoveModule>();

        if (moveModule != null)
        {
            moveModule.SetDirection(instanceDirection);
        }
        else
        {
            Debug.LogWarning(
                $"{blockPrefab.name} does not have a MoveModule component.",
                instance
            );
        }
    }
}
