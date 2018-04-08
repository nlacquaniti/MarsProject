using UnityEngine;

public class Restart : MonoBehaviour
{
    private bool _Trigger = true;
    private void Update()
    {
      
        if (Input.GetButtonDown("Menu"))
        {
            HUD.instance.PauseMenuInteraction();
        }
    }
}
