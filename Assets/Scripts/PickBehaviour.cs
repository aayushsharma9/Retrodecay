using UnityEngine;

public class PickBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed, acceleration;

    private void Start()
    {
        
    }

    public void SetSpeedAndAcceleration(int _speed, int _acc)
    {
        speed = _speed;
        acceleration = _acc;
    }
}
