using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{

    public event EventHandler OnDamagePopUp;

    [SerializeField] private Transform damagePopUp;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ShootAction shootAction;
    [SerializeField] private TextMeshProUGUI damagePopUpText;

    private Animator animator;

    private void Awake()
    {
        shootAction = GetComponentInParent<ShootAction>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {

        Unit.OnAnyActionPointChanged += Unit_OnAnyActionPointChanged;
        healthSystem.OnDamaged += HealthSystm_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
        Hide();
    }



    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void HealthSystm_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }


    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount =  healthSystem.GetHealthNormalized();
    }

    private void OnDestroy()
    {
        healthSystem.OnDamaged -= HealthSystm_OnDamaged;
    }

    public void ShowDamage(int damageAmount)
    {
        Show();
        //Sets the random damage inflicted to the damagePopUp text
        damagePopUpText.text = damageAmount.ToString();

        //event for the popUp animation
        OnDamagePopUp?.Invoke(this, EventArgs.Empty);
        
    }

    // functions to show and hide damagePopUp
    private void Show()
    {
        damagePopUp.gameObject.SetActive(true);
    }

    private void Hide()
    {
        damagePopUp.gameObject.SetActive(false);
    }

    public void DamagePopUpAnimationComplete()
    {
        Hide();
    }

}
