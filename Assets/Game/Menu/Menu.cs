using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private Loading _loading;
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private WalletPresenter _coinsWallet;

    private void Start()
    {
        _coinsWallet.Init(ServiceLocator.Locator.CoinsWallet);
        _bestScore.text = "Best score: " + Saver.GetInt("BestScore", 0).ToString();
    }
    public void StartGame()
    {
        _loading.LoadGame();
    }
}
