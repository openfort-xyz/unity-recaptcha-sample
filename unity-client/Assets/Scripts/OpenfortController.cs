using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Openfort.Model;
using TMPro;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class OpenfortController : MonoBehaviour
{
    [Serializable]
    public class CreateOfPlayerResponse
    {
        public string playerId;
        public string accountAddress;
    }
    
    private string _unityAuthPlayerName;
    private CreateOfPlayerResponse _createOfPlayerResponse;

    public Button mintButton;
    public Button addressButton;
    public TextMeshProUGUI addressButtonText;
    public TextMeshProUGUI statusText;

    private void OnEnable()
    {
        ReCaptchaController.Instance.OnActionValidated += ReCaptchaController_OnActionValidated_Handler;
    }

    private void OnDisable()
    {
        ReCaptchaController.Instance.OnActionValidated -= ReCaptchaController_OnActionValidated_Handler;
    }
    
    public void AuthController_OnAuthSuccess_Handler(string playerId)
    {
        // We'll use the AuthenticationService user name as the Openfort player name later.
        _unityAuthPlayerName = playerId;
        
        // Let's validate the auth action before creating the Openfort player.
        ReCaptchaController.Instance.ValidateAction(GameAction.Auth);
    }
    
    public void ValidateMintAction()
    {
        mintButton.interactable = false;
        ReCaptchaController.Instance.ValidateAction(GameAction.Mint);
    }
    
    private void ReCaptchaController_OnActionValidated_Handler(bool validated, GameAction gameAction)
    {
        if (!validated)
        {
            Debug.LogWarning($"ReCaptchaController did not validate the action: {gameAction}");

            if (gameAction == GameAction.Mint)
            {
                mintButton.interactable = true;
            }
            return;
        }

        switch (gameAction)
        {
            case GameAction.Auth:
                CreateOpenfortPlayer();
                break;
            case GameAction.Mint:
                MintNft();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(gameAction), gameAction, null);
        }
    }
    
    private async void CreateOpenfortPlayer()
    {
        if (string.IsNullOrEmpty(_unityAuthPlayerName))
        {
            Debug.LogError("playerName is null or empty.");    
            return;
        }
        
        var args = new Dictionary<string, object> { { "playerName", _unityAuthPlayerName } };
    
        try {
            statusText.text = "Creating Openfort player...";
            Debug.Log("Creating Openfort player...");
            
            var cloudCodeService = CloudCodeService.Instance;
            var response = await cloudCodeService.CallEndpointAsync<string>("CreateOpenfortPlayer", args);

            _createOfPlayerResponse = JsonConvert.DeserializeObject<CreateOfPlayerResponse>(response);
            
            statusText.text = "Created Openfort player: " + _createOfPlayerResponse.playerId;
            Debug.Log("Created Openfort player: " + _createOfPlayerResponse.playerId);
            Debug.Log("Openfort account address: " + _createOfPlayerResponse.accountAddress);
            
            mintButton.gameObject.SetActive(true);

        } catch (RequestFailedException e)
        {
            statusText.text = "Failed to create Openfort player: " + e.ErrorCode;
            Debug.LogError("Failed to create Openfort player: " + e.ErrorCode);
        }
    }
    
    private async void MintNft()
    {
        if (_createOfPlayerResponse is null)
        {
            Debug.LogError("CreateOfPlayerResponse is null.");
            return;
        }
        
        var args = new Dictionary<string, object> { { "playerId", _createOfPlayerResponse.playerId }, { "accountAddress", _createOfPlayerResponse.accountAddress } };
    
        try
        {
            statusText.text = "Minting NFT...";
            Debug.Log("Minting NFT...");
            
            var cloudCodeService = CloudCodeService.Instance;
            var txResponse = await cloudCodeService.CallEndpointAsync<TransactionIntentResponse>("MintNft", args);
            
            statusText.text = "Transaction ID: " + txResponse.Id;
            Debug.Log("Transaction ID: " + txResponse.Id);
            
        } catch (RequestFailedException e) {
            
            statusText.text = e.Message;
            Debug.Log(e.ErrorCode);
            
            // We can try to mint again
            mintButton.interactable = true;
        }
        
        statusText.text = "NFT minted!";
        Debug.Log("NFT minted!");
        
        mintButton.gameObject.SetActive(false);
        addressButton.gameObject.SetActive(true);
        addressButtonText.text = _createOfPlayerResponse.accountAddress;
    }

    public void OpenAccountExplorer()
    {
        Application.OpenURL($"https://mumbai.polygonscan.com/address/{_createOfPlayerResponse.accountAddress}#nfttransfers");
    }
}
