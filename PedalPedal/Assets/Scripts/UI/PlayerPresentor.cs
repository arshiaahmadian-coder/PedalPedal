using TMPro;
using UnityEngine;

public class PlayerPresentor : MonoBehaviour
{
    [SerializeField] ColideDetection colideDetection;
    [SerializeField] TMP_Text deathText;
    private int colideAmount = 0;

    private void OnEnable()
    {
        colideDetection.OnCollide += UpdateUI;
    }

    private void OnDisable()
    {
        colideDetection.OnCollide -= UpdateUI;
    }

    private void UpdateUI()
    {
        colideAmount += 1;
        deathText.text = colideAmount.ToString();
    }
}
