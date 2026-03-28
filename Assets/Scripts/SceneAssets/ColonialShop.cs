using Assets.Player;
using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Zenject;

public class ColonialShop : MonoBehaviour
{
    public event UnityAction<Consumables> OnConsumablePurchase;

    [SerializeField]
    private UIDocument _document;
    private VisualElement _shop;

    private Button _buyNanorepairKitButton;
    private Button _buyFuelButton;

    [Inject]
    private IPlayerSettings _playerSettings;
    void Start()
    {
        _shop = _document.rootVisualElement.Q<VisualElement>("ColonialShopWindow");
        _buyNanorepairKitButton = _shop.Q<VisualElement>("NanorepairKit").Q<Button>("BuyButton");
        _buyFuelButton = _shop.Q<VisualElement>("Fuel").Q<Button>("BuyButton");

        _buyNanorepairKitButton.clicked += BuyNanorepairKit;
        _buyFuelButton.clicked += BuyFuel;
    }

    private void BuyNanorepairKit()
    {
        if (_playerSettings.Money >= 1000)
            ConsumablePurchase(Consumables.NanoRepairKit, 1000);
    }

    private void BuyFuel()
    {
        ConsumablePurchase(Consumables.Fuel, 0);
    }

    private void ConsumablePurchase(Consumables consumable, int price)
    {
        _playerSettings.AddConsumable(consumable);
        _playerSettings.Money -= price;
        _playerSettings.SaveSettings();
        OnConsumablePurchase?.Invoke(consumable);
    }

    private void OnDestroy()
    {
        _buyNanorepairKitButton.clicked -= BuyNanorepairKit;
        _buyFuelButton.clicked -= BuyFuel;
    }

}
