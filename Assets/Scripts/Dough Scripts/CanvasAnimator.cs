using UnityEngine;

public class CanvasAnimator : MonoBehaviour
{
    public DoughManager man;

    public void CallDeleteFunction()
    {
        man.SwallawIfCompatibilityMatchs();
    }
}
