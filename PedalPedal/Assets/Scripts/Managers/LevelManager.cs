using UnityEditor.PackageManager;
using UnityEngine;

public enum Characters
{
    Zaal,
    Roodabeh,
    Player
}

public class LevelManager : MonoBehaviour
{
    public GameObject Controllers;

    [Header("Letter Settings")]
    [SerializeField]  Characters letterSenderCharacter;
    [SerializeField]  Transform ZaalLetterPos;
    [SerializeField]  Transform RoodabehLetterPos;
    [SerializeField]  Transform PlayerLetterPos;
    [SerializeField]  LetterThrower letterThrower;
    [SerializeField]  Characters letterHolder;
    [SerializeField]  GameObject ZaalLatterObject;
    [SerializeField]  GameObject RoodabehLatterObject;

    [Header("Dialog settings")]
    [SerializeField] DialogManager dialogManager;
    [SerializeField] bool hasStartDialog = false;
    [SerializeField] private DialogData StarterDialogData;
    [SerializeField] private float startDialogDelay = 1f;
    [SerializeField] bool hasEndDialog = false;
    [SerializeField] private DialogData endingDialogData;
    [SerializeField] private float endDialogDelay = 0f;

    private int gameStage = 0;
    public static LevelManager instance;
    private void Awake() { instance = this; }

    private void Start()
    {
        letterHolder = letterSenderCharacter;
        if (letterHolder == Characters.Zaal)
        {
            letterThrower.spriteRenderer = ZaalLatterObject.GetComponent<SpriteRenderer>();
        } else
        {
            letterThrower.spriteRenderer = RoodabehLatterObject.GetComponent<SpriteRenderer>();
        }

        if (hasStartDialog)
        {
            Controllers.SetActive(false);
            dialogManager.touchArea.SetActive(false);
            dialogManager.dialogData = StarterDialogData;
            Invoke(nameof(StartDialog), startDialogDelay);
        }   
    }

    public void PlayerReachedEndPoint()
    {
        Controllers.SetActive(false);
        // throw letter to reciver
        Invoke(nameof(ThrowLetter), 0.8f);
    }

    private void LetterRecived()
    {
        if (hasEndDialog)
        {
            // play ending dialog
        } else
        {
            print("Game Over, you win !");
        }
    }

    public void EndOfDialogCycle()
    {
        gameStage += 1;

        if (gameStage == 1) // after first dialog
        {
            ThrowLetter();
        } else if (gameStage == 2) // after second dialog
        {
            // end game
        }
    }

    public void ThrowLetter()
    {
        // set end pos
        Vector2 endPos = PlayerLetterPos.position;
        if (letterHolder == Characters.Player)
            endPos = (letterSenderCharacter == Characters.Zaal) 
            ? RoodabehLetterPos.position 
            : ZaalLetterPos.position;

        // set start pos
        Vector2 startPos;
        if (letterHolder == Characters.Roodabeh) startPos = RoodabehLetterPos.position;
        else if (letterHolder == Characters.Zaal) startPos = ZaalLetterPos.position;
        else startPos = PlayerLetterPos.position;

        // set values
        letterThrower.ThrowLetter(
            (letterSenderCharacter == Characters.Zaal) 
                ? ZaalLatterObject.transform 
                : RoodabehLatterObject.transform,
            startPos, endPos,
            LetterOnMidPoint, LetterOnComplete
        );

        // reset "letterHolder"
        if (endPos == new Vector2(PlayerLetterPos.position.x, PlayerLetterPos.position.y))
            letterHolder = Characters.Player;
        else if (endPos == new Vector2(ZaalLetterPos.position.x, ZaalLetterPos.position.y))
            letterHolder = Characters.Zaal;
        else if (endPos == new Vector2(RoodabehLetterPos.position.x, RoodabehLetterPos.position.y))
            letterHolder = Characters.Roodabeh;
        else print("ERR 1254");
    }

    private void LetterOnMidPoint() {
        letterThrower.spriteRenderer.sortingOrder = (letterHolder == Characters.Player) ? 20 : 10;
    }

    private void LetterOnComplete() {
        if (letterHolder == Characters.Player)
            Controllers.SetActive(true);
        else
        {
            // letter recived to Roodabeh/Zaal
            LetterRecived();
        }

        // TODO: particle effects
    }

    private void StartDialog()
    {
        dialogManager.StartDialogCycle();
    }
}
