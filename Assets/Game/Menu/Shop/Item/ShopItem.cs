using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {

        [SerializeField] private string _name;
        [SerializeField] private TextMeshProUGUI _scoreOutput;
        [SerializeField] private int _price;

        private Wallet _wallet;
        public int Price => _price;
        public string Type { get; private set; }
        public bool IsPurchased { get; private set; }

        public event Action<string> OnSelected;

        public void Init(Wallet wallet, string type)
        {
            _wallet = wallet;
            Type = type;
            IsPurchased = Saver.GetBool(Type + _name, false);
            if (IsPurchased)
            {
                _scoreOutput.text = "Select";
            }
            else
            {
                _scoreOutput.text = _price.ToString();
            }
        }
        public void Buy()
        {
            if (IsPurchased)
            {
                Select();
            }
            else
            {
                bool isPurchased = _wallet.TryRemove(_price);
                if (isPurchased)
                {
                    IsPurchased = true;
                    Saver.SaveBool(true,Type + _name);
                    Select();
                    _scoreOutput.text = "Select";
                }
            }

        }
        public void Select()
        {
            Saver.SaveString(_name, Type);
            OnSelected?.Invoke(_name);
        }
    }
}