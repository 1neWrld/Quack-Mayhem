using UnityEngine;
using TMPro;
using UnityEngine.UI;




public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;



    public void SetBaseAction(BaseAction baseAction)
    {
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        // Anonymous Function/ Lambda Expression
        button.onClick.AddListener(() =>
        {

            UnitActionSystem.Instance.SetSelectedAction(baseAction);

        });

    }




}
