using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour
{

    [SerializeField]
    private AnimationClip destroyAnim;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
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
                    }
                }
            }
        }
    }

    private IEnumerator DestroyThis(GameObject energy)
    {
        energy.GetComponent<BoxCollider>().enabled = false;
        energy.GetComponent<Animator>().Play("EnergyDestroy");
        yield return new WaitForSeconds(destroyAnim.length);
        Destroy(energy);
    }
}
