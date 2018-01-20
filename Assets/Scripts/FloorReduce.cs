using UnityEngine;

public class FloorReduce : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Energy")
        {   
            //Debug.Log("GAME OVER");
            Destroy(other.gameObject);
            GameManager.EnergyLevel--;
        }
    }
}
