using UnityEngine;
using TMPro;
using UnityEngine.UI;




public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        // Anonymous Function/ Lambda Expression
        button.onClick.AddListener(() =>
        {

            UnitActionSystem.Instance.SetSelectedAction(baseAction);

        });

    }

    // Updates selectedAction visual based on current active action
    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }


}
