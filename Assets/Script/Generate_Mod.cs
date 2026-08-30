using UnityEngine;

public class BlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    public float interval;
    public float timer;

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
        Instantiate(blockPrefab, this.transform.position, Quaternion.identity, this.transform);
    }
}
