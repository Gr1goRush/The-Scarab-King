using UnityEngine;

namespace Game.Player
{
    public class LinesController : MonoBehaviour
    {
        [SerializeField] private WalletPresenter _scoreWalletPresenter;
        [SerializeField] private Line _linePrefab;
        [SerializeField] private float _padding;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Vector2Int _distanceRange;
        [SerializeField] private Sprite[] _firstSpritesKit;
        [SerializeField] private Sprite[] _secondSpritesKit;
        [SerializeField] private Sprite[] _thirdSpritesKit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _spinAudio;
        [SerializeField] private AudioClip _stopAudio;
        [SerializeField] private AudioClip[] _winAudios;

        private Line[] _lines;
        private PlayerInput _input;
        private Wallet _coinsWallet;
        private Wallet _scoreWallet;
        private int _realizedLines;
        private bool _isRolling;
        private void Start()
        {
            Init();
            _input = PlayerInput.Get();
            _input.AddListener(EventKey.PointerDown, StartRoll);
        }
        private void Init()
        {
            _coinsWallet = ServiceLocator.Locator.CoinsWallet;
            _scoreWallet = ServiceLocator.Locator.ScoreWallet;
            _scoreWalletPresenter.Init(_scoreWallet);
            _lines = new Line[3];
            CreateLine(-1, _firstSpritesKit);
            CreateLine(0, _secondSpritesKit);
            CreateLine(1, _thirdSpritesKit);
        }
        private void CreateLine(int index, Sprite[] sprites)
        {
            float xOffset = _linePrefab.transform.lossyScale.x + _padding;
            float xPos = index * xOffset;
            Vector3 position = transform.position + new Vector3(xPos, 0, -1);
            Line line = Instantiate(_linePrefab, position, Quaternion.identity, transform);
            _lines[index + 1] = line;
            line.Init(_itemPrefab, sprites);
        }
        private void StartRoll()
        {
            if (_isRolling) return;
            _isRolling = true;

            _audioSource.clip = _spinAudio;
            _audioSource.Play();
            for (int i = 0; i < 3; i++)
            {
                int distance = Random.Range(_distanceRange.x, _distanceRange.y);
                _lines[i].StartRotate(distance, RealizeLine);
            }
        }
        private void RealizeLine()
        {
            _realizedLines++;
            if(_realizedLines >= 3)
            {
                _audioSource.Stop();
                _audioSource.PlayOneShot(_stopAudio);
                _isRolling = false;
                _realizedLines = 0;
                CalculateScore();
            }
        }
        private void CalculateScore()
        {
            int coins = 0;
            int score = 0;
            for (int i = 0; i < 3; i++)
            {
                int itemInLine = 0;
                for (int j = 0; j < 2; j++)
                {
                    if (_lines[j]._items[i].ID == _lines[j + 1]._items[i].ID)
                    {
                        itemInLine++;
                    }
                }
                coins += itemInLine * 5;

                if (itemInLine == 1) score += 5;
                else if (itemInLine == 2) score += 10;
            }
            if(coins > 0)
            {
                _scoreWallet.Add(score);
                _audioSource.PlayOneShot(_winAudios[Random.Range(0, _winAudios.Length)]);
                _coinsWallet.Add(coins);
            } 
        }
    }
}