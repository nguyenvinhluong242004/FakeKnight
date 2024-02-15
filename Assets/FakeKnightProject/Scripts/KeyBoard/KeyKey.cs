using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyKey : MonoBehaviour
{
    [SerializeField] private string keyString;
    [SerializeField] private GetKeyDownOnBoard getKeyDownOnBoard;

    void Start()
    {
        getKeyDownOnBoard = FindObjectOfType<GetKeyDownOnBoard>();
    }
    public void getKey()
    {
        if (keyString == "OK")
        {
            if (getKeyDownOnBoard.loginOrPlay)
                getKeyDownOnBoard.keyBoard.SetActive(false);
            else
                getKeyDownOnBoard.keyBoardPlay.SetActive(false);
            if (getKeyDownOnBoard.status == 1)
            {
                getKeyDownOnBoard.login.position = new Vector3(0, 0, 90);
            }
            else if (getKeyDownOnBoard.status == 2)
            {
                getKeyDownOnBoard.fogot.position = new Vector3(0, 0, 90);
            }
            else if(getKeyDownOnBoard.status == 3)
            {
                getKeyDownOnBoard.register.position = new Vector3(0, 0, 90);
            }
        }
        else if (keyString == "UP")
        {
            if (!getKeyDownOnBoard.isUP)
            {
                gameObject.GetComponent<Image>().color = new Color(1f, 0, 0, 1f);
            }
            else
            {
                gameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            getKeyDownOnBoard.isUP = !getKeyDownOnBoard.isUP;
        }
        else if (keyString == "SPACE")
        {
            getKeyDownOnBoard.getKeyOnBoard(" ");
        } 
        else
        {
            if (getKeyDownOnBoard.isUP || keyString == "DEL")
                getKeyDownOnBoard.getKeyOnBoard(keyString.ToUpper());
            else
                getKeyDownOnBoard.getKeyOnBoard(keyString.ToLower());
        }
    }    

}
