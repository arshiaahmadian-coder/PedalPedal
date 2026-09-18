using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public GameObject touchArea;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private AudioSource audioSource;

    [Header("dialog settings")]
    
    [SerializeField] private float startDelay;
    public DialogData dialogData;

    public int dialogIndex = 0;

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
        LevelManager.instance.EndOfDialogCycle();
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
