using UnityEngine;

public class CrashCounter : MonoBehaviour
{
    public int crashAmount;

    public void Crashed() {
        crashAmount += 1;
    }
}
