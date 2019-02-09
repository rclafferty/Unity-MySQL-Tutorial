using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{
    GameObject mainMenu;

    [SerializeField]
    GameObject fieldEmailAddress;
    [SerializeField]
    Text textEmail;
    [SerializeField]
    GameObject fieldPassword;
    [SerializeField]
    Text textPassword;
    [SerializeField]
    GameObject fieldReenterPassword;
    [SerializeField]
    Text textReenterPassword;

    [SerializeField]
    GameObject buttonJoinGame;
    [SerializeField]
    GameObject buttonSwapRegistration;
    [SerializeField]
    GameObject buttonRegister;

    bool showRegistration;

    [SerializeField]
    Text textSwapButton;
    [SerializeField]
    Text textFeedback;

    WWWForm loginForm;
    WWWForm registerForm;

    ManagerReferences managerReferences;

    // Start is called before the first frame update
    void Start()
    {
        fieldEmailAddress = GameObject.Find("InputFieldEmail");
        textEmail = GameObject.Find("InputFieldEmailText").GetComponent<Text>();

        fieldPassword = GameObject.Find("InputFieldPassword");
        textPassword = GameObject.Find("InputFieldPasswordText").GetComponent<Text>();

        fieldReenterPassword = GameObject.Find("InputFieldPassword2");
        textReenterPassword = GameObject.Find("InputFieldPassword2Text").GetComponent<Text>();

        buttonJoinGame = GameObject.Find("ButtonJoinGame");

        buttonSwapRegistration = GameObject.Find("ButtonSwapRegistration");
        textSwapButton = buttonSwapRegistration.GetComponentInChildren<Text>();

        buttonRegister = GameObject.Find("ButtonRegister");

        showRegistration = false;
        textSwapButton.text = "Sign Up";

        textFeedback = GameObject.Find("TextFeedback").GetComponent<Text>();
        textFeedback.text = "";

        managerReferences = GameObject.Find("Manager").GetComponent<ManagerReferences>();

        DisplayLoginPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayLoginPanel()
    {
        buttonRegister.SetActive(false);
        fieldReenterPassword.SetActive(false);
    }

    public void SwapSignupSignin()
    {
        // Flip the following flags
        showRegistration = !showRegistration;
        buttonJoinGame.SetActive(!buttonJoinGame.activeInHierarchy);
        buttonRegister.SetActive(!buttonRegister.activeInHierarchy);
        fieldReenterPassword.SetActive(!fieldReenterPassword.activeInHierarchy);

        if (showRegistration)
        {
            textSwapButton.text = "Sign In";
        }
        else
        {
            textSwapButton.text = "Sign Up";
        }
    }

    public void RegisterButtonTapped()
    {
        // TODO
        textFeedback.text = "Processing registration...";
        StartCoroutine("RequestUserRegistration");
    }

    public void LoginButtonTapped()
    {
        textFeedback.text = "Logging in...";
        StartCoroutine("RequestLogin");
    }

    private string GetEmail()
    {
        return fieldEmailAddress.GetComponent<InputField>().text.Trim();
    }

    private string GetPassword()
    {
        return fieldPassword.GetComponent<InputField>().text.Trim();
    }

    private string GetConfirmPassword()
    {
        return fieldReenterPassword.GetComponent<InputField>().text.Trim();
    }

    // public bool DisplayFeedback(WWW form, string successMessage)
    public void DisplayFeedback(WWW form, string successMessage)
    {
        if (string.IsNullOrEmpty(form.error))
        {
            User thisUser = JsonUtility.FromJson<User>(form.text);
            managerReferences.user = thisUser;

            if (thisUser.Success == true)
            {
                if (!string.IsNullOrEmpty(thisUser.Error))
                {
                    textFeedback.text = thisUser.Error;
                    // return false;
                }
                else
                {
                    textFeedback.text = successMessage;
                    // Launch the game
                    GameObject.Find("PanelMainMenu").GetComponent<NetworkManagerMainMenu>().JoinGame();

                    // return true;
                }
            }
            else
            {
                textFeedback.text = thisUser.Error;
                // return false;
            }

            /*
            Using my custom JSON Decoder method below
            
            string[,] result = JSONDecoder(form.text);
            
            if (result[0, 1] == "false")
            {
                // Print error
                textFeedback.text = "ERROR: " + result[1, 1];
                return false;
            }
            else
            {
                // Success
                textFeedback.text = successMessage;
                return true;
            }
            */
        }
        else
        {
            // Error
            textFeedback.text = "An error occured.";
            // return false;
        }
    }

    public IEnumerator RequestUserRegistration()
    {
        string email = GetEmail();
        string password = GetPassword();
        string confirmPassword = GetConfirmPassword();

        if (password == confirmPassword)
        {
            if (password.Length >= 8)
            {
                // Register
                registerForm = new WWWForm();
                registerForm.AddField("email", email);
                registerForm.AddField("password", password);

                WWW webform = new WWW(managerReferences.urlRegister, registerForm);

                yield return webform;

                DisplayFeedback(webform, "Registration is successful");
            }
            else
            {
                textFeedback.text = "Password is too short";
            }
        }
        else
        {
            textFeedback.text = "Passwords do not match";
        }
    }

    public IEnumerator RequestLogin()
    {
        string email = GetEmail();
        string password = GetPassword();

        loginForm = new WWWForm();
        loginForm.AddField("email", email);
        loginForm.AddField("password", password);
        Debug.Log(email);
        Debug.Log(password);

        WWW webform = new WWW(managerReferences.urlLogin, loginForm);
        
        yield return webform;

        DisplayFeedback(webform, "Login is successful");
    }

    public string[,] JSONDecoder(string jsonString)
    {
        
        string[] parts = jsonString.Split('{', '}'); // 1st is empty, 2nd is everything in between {}
        parts = parts[1].Split(','); // 1st is success: true/false, 2nd is error: "..."

        string[] successParts = parts[0].Split(':');
        successParts[0] = successParts[0].Split('"')[1];
        string successString = successParts[0];
        string successResult = successParts[1];
        // Debug.Log("Success: " + successResult);

        string[] errorParts = parts[1].Split(':');
        errorParts[0] = errorParts[0].Split('"')[1];
        errorParts[1] = errorParts[1].Split('"')[1];
        string errorString = errorParts[0];
        string errorResult = errorParts[1];
        Debug.Log("Error: " + errorResult);

        string[,] result = { { successString, successResult }, { errorString, errorResult } };
        return result;
    }
}

public class User
{
    // These fields MUST be serializable or else the JSON decoder won't work.
    [SerializeField]
    private bool success;
    [SerializeField]
    private string error;
    [SerializeField]
    private string email;
    [SerializeField]
    private string userID;

    public bool Success
    {
        get
        {
            return success;
        }
    }

    public string Error
    {
        get
        {
            return error;
        }
    }

    public string Email
    {
        get
        {
            return email;
        }
    }

    public string UserID
    {
        get
        {
            return userID;
        }
    }
}

public class PlayerGear
{
    [SerializeField]
    private string pathHead;
    [SerializeField]
    private string pathShoulders;
    [SerializeField]
    private string pathRightHand;
    [SerializeField]
    private string pathLeftHand; // OffHand
}