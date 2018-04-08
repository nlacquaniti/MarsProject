using UnityEngine;

public class GoToSelezionaMappa : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButtonDown(InputContainer.MENU))
        {
            SceneLoader.GoToScene(ScenesContainer.MENU_INIZIALE);
        }
    }

}
