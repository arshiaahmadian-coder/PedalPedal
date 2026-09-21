using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Image dialogImage;
    [SerializeField] private Animator animator;
    public GameObject touchArea;
    [SerializeField] private TMP_Text dialogText;
    [SerializeField] private AudioSource audioSource;

    [Header("dialog settings")]
    public DialogData dialogData;

    public int dialogIndex = 0;

    public void StartDialogCycle()
    {
        touchArea.SetActive(true);
        ChangeAnimationData();
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
        ChangeAnimationData();
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

    private void ChangeAnimationData()
    {
        // text
        dialogText.text = dialogData.dialogTextList[dialogIndex];

        // sound
        if (dialogData.multyVoiceLines)
            audioSource.PlayOneShot(dialogData.dialogVoiceList[dialogIndex]);
        else 
            audioSource.PlayOneShot(dialogData.dialogVoiceList[0]);

        // sprite
        if (dialogData.multySprites)
            dialogImage.sprite = dialogData.dialogCharacterList[dialogIndex];
        else 
            dialogImage.sprite = dialogData.dialogCharacterList[0];
    }
}
