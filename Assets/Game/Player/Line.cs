using System;
using UnityEngine;

namespace Game.Player
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private float _startSpeed;
        [SerializeField] private float _padding;
        [SerializeField] private float _respawnHeight;
        [SerializeField] private AnimationCurve _speedPerTime;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _startAudio;
        [SerializeField] private AudioClip _stopAudio;
        public Item[] _items { get; private set; }

        private int _itemsCount;
        private bool _isRotate;
        private int _rollDistance;
        private Pool<Transform> _itemViews;
        private Transform _lastItem;
        private float _pastDistance;
        private Action _onEndRotate;
        public void Init(GameObject itemPrefab, Sprite[] sprites)
        {
            _respawnHeight = itemPrefab.transform.lossyScale.y * 2 + _padding * 2;
            Transform[] objects = new Transform[sprites.Length];
            _itemsCount = sprites.Length;
            _items = new Item[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                GameObject obj = Instantiate(itemPrefab, transform);
                Item item = new Item(obj.transform, int.Parse(sprites[i].name));
                objects[i] = obj.transform;
                _items[i] = item;
                PlaceItem(i);
                obj.GetComponentInChildren<SpriteRenderer>().sprite = sprites[i];
            }
            _lastItem = objects[_itemsCount - 1];
            _itemViews = new Pool<Transform>(objects);
        }
        private void PlaceItem(int index)
        {
            Item item = _items[index];
            Vector3 position = transform.position + Vector3.up * (item.transform.lossyScale.x + _padding) * (index-1);
            item.transform.position = position;
        }
        public void StartRotate(int distance, Action OnEndRotate)
        {
            _audioSource.PlayOneShot(_startAudio);
            _onEndRotate = OnEndRotate;
            _isRotate = true;
            _rollDistance = distance;
        }
        private void Update()
        {
            if (_isRotate)
            {
                UpdateLine();
            }
        }
        private void UpdateLine()
        {
            float speed = _speedPerTime.Evaluate(_pastDistance/_rollDistance);
            float delta = _startSpeed * speed * Time.deltaTime;
            if(_pastDistance >= _rollDistance)
            {
                _isRotate=false;
                _pastDistance = 0;
                SetItemsPos();
                _onEndRotate.Invoke();
                _audioSource.PlayOneShot(_stopAudio);
            }
            for (int i = 0; i < _items.Length; i++)
            {
                Item item = _items[i];
                item.transform.position += Vector3.down * delta;
            }
            CheckLastItem();
        }
        private void CheckLastItem()
        {
            Transform obj = _itemViews.GetNotDelete();
            float height = transform.position.y - obj.position.y;
            if (height >= _respawnHeight)
            {
                _itemViews.Get();
                float heightDelta = height - _respawnHeight;
                obj.transform.position = _lastItem.position + new Vector3(0, obj.localScale.y + _padding - heightDelta, 0);
                _lastItem = obj;
                _pastDistance += 1;
                CheckLastItem();
            }
        }
        private void SetItemsPos()
        {
            int offset = _rollDistance - (_rollDistance / _itemsCount) * _itemsCount;
            Debug.Log($"rolldist:{_rollDistance}, itemCount:{_itemsCount}, divide:{_rollDistance / _itemsCount}, result:{offset}");
            Item[] items = new Item[_itemsCount];
            for (int i = 0; i < _itemsCount; i++)
            {
                Item item = _items[i];
                int index = i - offset;
                if(index < 0)
                {
                    index =  _itemsCount + index;
                }
                items[index] = item;
            }
            _items = items;
            for (int i = 0; i < _itemsCount; i++)
            {
                PlaceItem(i);
            }
            _items = items;
        }
    }
}

