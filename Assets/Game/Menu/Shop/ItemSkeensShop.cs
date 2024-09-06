using System;
using UnityEngine;

namespace Shop
{
    public class ItemSkeensShop : MonoBehaviour
    {
        [SerializeField] private string _itemName;
        [SerializeField] private Transform _context;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _selectAudio;
        public string ItemName => _itemName;
        public event Action<string, string> OnItemSelected;

        private ShopItem[] _skeens;
        private bool _isInicialized;

        public void Start()
        {
            if (!_isInicialized)
            {
                Open();
                Init();
            }
            if (_context.gameObject.activeInHierarchy)
            {
                Close();
            }

        }
        public void Init()
        {

            _skeens = GetComponentsInChildren<ShopItem>();
            Wallet wallet = ServiceLocator.Locator.CoinsWallet;
            Debug.Log(_skeens.Length);
            for (int i = 0; i < _skeens.Length; i++)
            {

                _skeens[i].Init(wallet, _itemName);
                _skeens[i].OnSelected += (name) =>
                {
                    _source.PlayOneShot(_selectAudio);
                    OnItemSelected?.Invoke(_itemName, name);
                };
            }
        }
        public void Open()
        {
            _context.gameObject.SetActive(true);
        }
        public void Close()
        {
            _context.gameObject.SetActive(false);
        }
    }
}