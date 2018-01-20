using UnityEngine;

public class PickSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject energy, negative;
    [SerializeField]
    private GameObject Inner, Outer;
    public static float partRotationSpeed;
    [SerializeField]
    private static float maxSpeed, minSpeed;
    [SerializeField]
    private float spawnInterval;
    private float t = 0, s = 0;
    float Speed;

    private void Start()
    {
        partRotationSpeed = 200;
    }
    
    private void Update()
    {
        t += Time.deltaTime;
        s += Time.deltaTime;
        
        Inner.transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * partRotationSpeed);

        if (GameManager.currentLevel == 3)
        {
            Outer.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * (partRotationSpeed-100));
        }

        if (t >= spawnInterval)
        {
            SpawnPick(energy);
            t = 0;
        }

        if (s >= (Random.Range(3,6)))
        {
            SpawnPick(negative);
            s = 0;
        }
    }

    private void SpawnPick(GameObject pickType)
    {
        GameObject pick_clone = Instantiate(pickType, gameObject.transform);
        pick_clone.transform.parent = null;

        float xVect = Random.Range(-100, 100);
        float yVect = Random.Range(-100, 100);

        Vector3 RandomDirection = new Vector3(xVect, 0, yVect);
        Debug.Log("RandomVector: (" + xVect + ", 0, " + yVect + ")");

        do
        {
            Speed = Random.Range(-maxSpeed, maxSpeed);
        } while (Speed == 0 || (Speed < minSpeed && Speed > 0) || (Speed > -minSpeed && Speed < 0));

        pick_clone.GetComponent<Rigidbody>().velocity = RandomDirection.normalized * Speed;
    }

    public static void IncreasePickSpeedBy(float s)
    {
        minSpeed += s;
        maxSpeed += s;
        Debug.Log("Speed increased to " + minSpeed + " & " + maxSpeed);
    } 

    public static void ResetPickSpeed()
    {
        minSpeed = 2;
        maxSpeed = 4;
    } 
}
