using UnityEngine;

public class TrashScript : MonoBehaviour {

    public DoughManager doughManager;

    public void OnDrop()
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            doughManager.DeleteBowl();
        }
    }

  
}
