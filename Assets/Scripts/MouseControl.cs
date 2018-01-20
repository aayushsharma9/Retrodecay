using System.Collections;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField]
    private AnimationClip destroyAnim;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            if (Physics.Raycast (ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Energy")
                {
                    StartCoroutine(DestroyThis(hit.transform.gameObject));
                    GameManager.AddEnergy(2);
                    GameManager.Score++;

                    if (GameManager.Score > GameManager.HighScore)
                    {
                        GameManager.HighScore = GameManager.Score;
                        PlayerPrefs.SetInt("HighScore", GameManager.HighScore);
                        Debug.Log("High Score changed to " + GameManager.HighScore);
                    }
                }

                else if (hit.transform.gameObject.tag == "Negative")
                {
                    StartCoroutine(DestroyThis(hit.transform.gameObject));
                    GameManager.AddEnergy(-2);
                    TurnEverythingNegative();
                }
            }
        }
    }
    
    private void TurnEverythingNegative()
    {
        GameObject[] pick;
        pick = GameObject.FindGameObjectsWithTag("Energy");

        foreach (GameObject pickInstance in pick)
        {
            pickInstance.GetComponent<Animator>().Play("EnergyInvert");
            pickInstance.tag = "Negative";
            pickInstance.GetComponent<BoxCollider>().size = new Vector3(0.4f, 0.4f, 0.1f);
        }
    }


    private IEnumerator DestroyThis(GameObject _energy)
    {
        _energy.GetComponent<BoxCollider>().enabled = false;
        _energy.GetComponent<Animator>().Play("EnergyDestroy");
        yield return new WaitForSeconds(destroyAnim.length);
        Destroy(_energy);
    }
}
