using UnityEngine;
using TMPro;

public class ReloadProgressBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI progressBarText;
    private float currentProgress = 0f;
    private float targetProgress = 1f; // 1 significa 100% concluído

    public GunScript gun;

    private void Update()
    {
        
    }

    public void UpdateProgressBarText()
    {
        // Atualize a barra de progresso com base no tempo restante de recarga
        currentProgress += (gun.currentReloadTime);
        currentProgress = Mathf.Clamp01(currentProgress);
        progressBarText.text = $"Reload: {Mathf.Round(currentProgress * 100)}%";
    }

    // Esta função pode ser chamada quando a recarga estiver concluída para redefinir a barra de progresso
    public void ResetProgress()
    {
        currentProgress = 0f;
        UpdateProgressBarText();
    }
}
