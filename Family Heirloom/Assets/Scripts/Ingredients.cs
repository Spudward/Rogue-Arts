using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ingredients : MonoBehaviour
{
    public static int currentIngredients;
    int totalIngredients;
    bool collected = false;
    public Text infoText;

    void Start()
    {
        totalIngredients = GameObject.FindGameObjectsWithTag("Ingredient").Length;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!collected)
            {
                collected = true;
                Ingredients.currentIngredients++;
                if (currentIngredients != totalIngredients)
                {
                    infoText.text = "You have Collected: " + currentIngredients + "/" + totalIngredients + " Ingredients";
                }
                else
                {
                    infoText.text = "You've collected all the ingredients, make your way to a work bench to make the antidote";
                }
                StartCoroutine(ClearText());
            }
            
        }
    }

    IEnumerator ClearText()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(3);
        infoText.text = " ";
        Destroy(this.gameObject);
    }
}
