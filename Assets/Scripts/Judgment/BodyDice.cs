using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyDice : MonoBehaviour
{
    [SerializeField] Sprite[] diceSides;
    private Image originImage;
    bool isRolling = false;

    private void Start()
    {
        originImage = GetComponent<Image>();
    }

    public void RollDice()
    {
        if (!isRolling) StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        isRolling = true;
        int randomDiceSide = 0;
        int finalSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            originImage.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.06f);
        }
        finalSide = randomDiceSide + 1;
        isRolling = false;
    }
}
