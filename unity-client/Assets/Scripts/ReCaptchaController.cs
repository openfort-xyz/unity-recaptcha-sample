using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine.Events;

// Define the Actions enum
public enum GameAction
{
    Auth,
    Mint
    // Add more actions as needed
    
}
public class ReCaptchaController : MonoBehaviour
{
    // Singleton instance
    public static ReCaptchaController Instance { get; private set; }

    private GameAction _currentAction;
    
    // Your site key here
    [SerializeField] private string siteKey;
    
    public event UnityAction<bool, GameAction> OnActionValidated;

    public TextMeshProUGUI statusText;
    
    // Import the JavaScript functions from the .jslib file
    [DllImport("__Internal")]
    private static extern void LoadReCaptcha(string siteKey);

    [DllImport("__Internal")]
    private static extern void ExecuteReCaptcha(string siteKey);

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadReCaptcha(siteKey);
    }
    
    // Call this method to execute ReCaptcha and pass the action
    public void ValidateAction(GameAction gameAction)
    {
        // You can use the action parameter to perform different logic based on the action
        statusText.text = $"ReCaptcha validating {gameAction} action...";
        Debug.Log($"ReCaptcha validating {gameAction} action...");
        
        _currentAction = gameAction;
        ExecuteReCaptcha(siteKey);
    }

    #region .JSLIB CALLBACKS
    // This method will be called from the .jslib when ReCaptcha is resolved
    void OnReCaptchaResolved(string token)
    {
        // Handle the token received from ReCaptcha
        statusText.text = "ReCaptcha token received.";
        Debug.Log("ReCaptcha token received: " + token);
        // You can now send this token to your server for verification
        // Verify the token using the VerifyReCaptcha Cloud Script
        VerifyReCaptcha(token, _currentAction);
    }
    
    void OnReCaptchaError(string errorMessage)
    {
        // Handle the error message received from ReCaptcha
        statusText.text = "ReCaptcha error: " + errorMessage;
        Debug.LogError("ReCaptcha error: " + errorMessage);
        
        OnActionValidated?.Invoke(false, _currentAction);
    }
    #endregion

    private async void VerifyReCaptcha(string token, GameAction currentAction)
    {
        statusText.text = "ReCaptcha verifying token...";
        var args = new Dictionary<string, object> { { "token", token } };

        try {
            var cloudCodeService = CloudCodeService.Instance;
            var score = await cloudCodeService.CallEndpointAsync<string>("VerifyReCaptcha", args);
            Debug.Log("ReCaptcha verification score: " + score);

            try
            {
                var scoreValue = float.Parse(score);

                if (scoreValue > 0.5)
                {
                    statusText.text = $"ReCaptcha verification score: {score} <br> SUCCESS!";
                    await UniTask.Delay(2);
                    statusText.text = string.Empty;
                    
                    OnActionValidated?.Invoke(true, currentAction);
                }
                else
                {
                    statusText.text = $"ReCaptcha verification score: {score} <br> FAILED!";
                    await UniTask.Delay(2);
                    statusText.text = string.Empty;
                    
                    OnActionValidated?.Invoke(false, currentAction);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        } catch (RequestFailedException e)
        {
            statusText.text = "Failed to verify ReCaptcha token: " + e.ErrorCode;
            Debug.LogError("Failed to verify ReCaptcha token: " + e.ErrorCode);
            
            OnActionValidated?.Invoke(false, currentAction);
        }
    }
}