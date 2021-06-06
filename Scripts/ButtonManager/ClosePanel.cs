using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel; // drop the panel in the editor
    public Button yourButton;	

    void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
    void TaskOnClick()
    {
        Debug.Log("close");
        panel.SetActive(false); // make it active/inactive with one click
    }
}
