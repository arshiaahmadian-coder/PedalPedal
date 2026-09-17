using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Controllers;
    [SerializeField] private GameObject touchArea;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private AudioSource audioSource;

    [Header("dialog settings")]
    
    [SerializeField] private float startDelay;
    [SerializeField] private DialogData dialogData;

    private int dialogIndex = 0;

    private void Start()
    {
        Controllers.SetActive(false);
        touchArea.SetActive(false);
        Invoke(nameof(StartDialogCycle), startDelay);
    }

    public void StartDialogCycle()
    {
        touchArea.SetActive(true);
        ChangeTextAndSound();
        animator.SetTrigger("Up");
    }

    public void StopDialogCycle()
    {
        touchArea.SetActive(false);
        animator.SetTrigger("Down");
    }

    public void NextDialog()
    {
        animator.SetTrigger("Next");
        dialogIndex += 1;
    }

    // call in animator
    public void ChangeDialogText()
    {
        ChangeTextAndSound();
    }

    public void EnableControllers()
    {
        Controllers.SetActive(true);
    }

    public void OnNextClick()
    {
        if (dialogIndex < dialogData.dialogTextList.Length - 1)
            NextDialog();
        else
            StopDialogCycle();
    }

    private void ChangeTextAndSound()
    {
        dialogText.text = dialogData.dialogTextList[dialogIndex];
        if (dialogData.multyVoiceLines)
        
            audioSource.PlayOneShot(dialogData.dialogVoiceList[dialogIndex]);
        else 
            audioSource.PlayOneShot(dialogData.dialogVoiceList[0]);
    }
}
