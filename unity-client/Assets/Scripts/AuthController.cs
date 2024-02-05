using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AuthController : MonoBehaviour
{
    public UnityEvent<string> onAuthSuccess;
    public UnityEvent onAuthFailed;

    public Button signInButton;
    public TextMeshProUGUI statusText;
    
    internal async void Awake()
    {
        await UnityServices.InitializeAsync();
    }
    
    public async void SignIn()
    {
        statusText.text = "Signing in...";
        signInButton.interactable = false;
        await SignInAnonymously();
    }
    
    private async Task SignInAnonymously()
    {
        AuthenticationService.Instance.SignedIn += () =>
        {
            var playerId = AuthenticationService.Instance.PlayerId;

            statusText.text = "Signed in as: " + playerId;
            Debug.Log("Signed in as: " + playerId);
            
            signInButton.gameObject.SetActive(false);
            onAuthSuccess?.Invoke(playerId);
        };
        AuthenticationService.Instance.SignInFailed += s =>
        {
            Debug.Log(s);
            statusText.text = "Sign in failed. Try again.";
            
            signInButton.interactable = true;
            onAuthFailed?.Invoke();
        };

        // Doing this we sign in as a new AuthenticationService anonymous user everytime.
        // We do this because in this sample we create an Openfort player also every time we sign in with AuthenticationService.
        AuthenticationService.Instance.ClearSessionToken();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
