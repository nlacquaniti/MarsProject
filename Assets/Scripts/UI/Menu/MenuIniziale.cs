using UnityEngine;
using UnityEngine.UI;

public class MenuIniziale : MonoBehaviour
{
    [SerializeField] private Text[] m_Texts;
    [SerializeField] private Color[] m_Colors;

    private int m_Index = 0;
    private bool m_StopInput = false;


    private void Start()
    {
        m_Texts[m_Index].color = m_Colors[1];
    }

    private void Update()
    {
        if(Input.GetButtonDown(InputContainer.SUBMIT))
        {

            switch (m_Index)
            {
                case 0:
                    SceneLoader.GoToScene(ScenesContainer.MENU_SELEZIONE);
                    break;

                case 1:
                    Application.Quit();
                    break;

            }

                
            /*
            if (m_Index == 2)
                SceneLoader.GoToScene(ScenesContainer.CREDITS);
            if (m_Index == 3)
                SceneLoader.GoToScene(ScenesContainer.OPZIONI);
            if (m_Index == 4)
                SceneLoader.GoToScene(ScenesContainer.MULTIPLAYER); 
             */

        }


        if (!m_StopInput)
        {
            if (Input.GetAxis(InputContainer.HORIZONTAL) > 0)
            {
                if (m_Index > 0)
                {
                    m_Texts[m_Index].color = m_Colors[0];
                    m_Index--;
                    m_Texts[m_Index].color = m_Colors[1];
                }
                else
                {
                    m_Texts[m_Index].color = m_Colors[0];
                    m_Index = m_Texts.Length - 1;
                    m_Texts[m_Index].color = m_Colors[1];
                }
            }

            if(Input.GetAxis(InputContainer.HORIZONTAL) < 0)
            {
                if (m_Index < m_Texts.Length - 1)
                {
                    m_Texts[m_Index].color = m_Colors[0];
                    m_Index++;
                    m_Texts[m_Index].color = m_Colors[1];
                }
                else
                {
                    m_Texts[m_Index].color = m_Colors[0];
                    m_Index = 0;
                    m_Texts[m_Index].color = m_Colors[1];
                }
            }




        }


        if (Input.GetAxis(InputContainer.VERTICAL) == 0 && Input.GetAxis(InputContainer.HORIZONTAL) == 0)
        {
            m_StopInput = false;
        }
        else
        {
            m_StopInput = true;
        }
    }
}