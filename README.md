# Openfort Unity reCAPTCHA V3 Sample

## [Try it live!](https://openfort-xyz.github.io/unity-recaptcha-sample/build)
  
## Overview
This is a sample project to showcase the Openfort integration with [Google reCAPTCHA V3](https://developers.google.com/recaptcha/docs/v3), a system to help you protect your sites from fraudulent activities, spam, and abuse. In this sample, we will activate reCAPTCHA verification every time the user tries to perform a key action like authenticating or minting an NFT. 

The sample includes:
  - [**`ugs-backend`**](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/tree/main/ugs-backend)
    
    Some [Cloud Code JS scripts](https://docs.unity.com/ugs/en-us/manual/cloud-code/manual/scripts) to interact with the Openfort API from UGS BaaS.

  - [**`unity-client`**](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/tree/main/unity-client)

    A Unity sample game that connects to ``ugs-backend`` through [Cloud Code Client SDK](https://docs.unity.com/ugs/manual/cloud-code/manual). It uses [Openfort Unity SDK](https://github.com/openfort-xyz/openfort-csharp-unity) to have full compatibility with ``ugs-backend`` responses.

## Workflow diagram

<div align="center">
    <img
      width="100%"
      height="100%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample_workflow_9d3da01894.png?updated_at=2024-02-06T08:35:17.086Z"
      alt='Openfort Unity reCAPTCHA V3 Sample Workflow'
    />
    </div>

## Prerequisites
+ **Get started with Openfort**
  + [Sign in](https://dashboard.openfort.xyz/login) or [sign up](https://dashboard.openfort.xyz/register) and create a new dashboard project

+ **Get started with UGS**
  + [Complete basic prerequisites](https://docs.unity.com/ugs/manual/overview/manual/getting-started#Prerequisites)
  + [Create a project](https://docs.unity.com/ugs/manual/overview/manual/getting-started#CreateProject)

+ **Get started with GitHub Pages**
  + [Create a repository for your site](https://docs.github.com/en/pages/getting-started-with-github-pages/creating-a-github-pages-site#creating-a-repository-for-your-site)
  + [Create your site](https://docs.github.com/en/pages/getting-started-with-github-pages/creating-a-github-pages-site#creating-your-site)

## Setup Openfort dashboard
  + [Add a Contract](https://dashboard.openfort.xyz/assets/new)
    
    This sample requires an NFT contract to run. We use [0x38090d1636069c0ff1Af6bc1737Fb996B7f63AC0](https://mumbai.polygonscan.com/address/0x38090d1636069c0ff1Af6bc1737Fb996B7f63AC0) (NFT contract deployed in 80001 Mumbai). You can use it for this tutorial:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_4_9397f3633b.png?updated_at=2023-12-14T15:59:33.808Z"
      alt='Contract Info'
    />
    </div>

  + [Add a Policy](https://dashboard.openfort.xyz/policies/new)
    
    We aim to cover gas fees for users. Set a new gas policy:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_5_ab3d8ad48d.png?updated_at=2023-12-14T15:59:33.985Z"
      alt='Gas Policy'
    />
    </div>

    Now, add a rule so our contract uses this policy:

    <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_6_6727e69146.png?updated_at=2023-12-14T15:59:33.683Z"
      alt='Policy Rule'
    />
    </div>

## Set up reCAPTCHA V3

Go to [reCAPTCHA v3 Admin Console](https://www.google.com/recaptcha/admin) to register a new site. Choose ***Switch to create a classic key*** if the option is available:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample1_011de519f7.png?updated_at=2024-02-02T14:36:18.691Z"
      alt='Set up reCAPTCHA V3: Switch to create a classic key'
    />
    </div>

Enter a ***Label*** name, select the ***reCAPTCHA type*** and enter your GitHub Pages URL as a ***Domain***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample2_536c95791f.png?updated_at=2024-02-02T14:36:19.493Z"
      alt='Set up reCAPTCHA V3: Fill options'
    />
    </div>

Accept the terms of service and choose ***Submit***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample3_12771b8ee4.png?updated_at=2024-02-02T14:36:19.586Z"
      alt='Set up reCAPTCHA V3: Submit'
    />
    </div>

Copy the ***Site Key*** and the ***Secret Key*** and save them somewhere safe:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample4_01a863dd6e.png?updated_at=2024-02-02T14:36:21.176Z"
      alt='Set up reCAPTCHA V3: Copy keys'
    />
    </div>

## Set up [`ugs-backend`](https://github.com/openfort-xyz/ugs-unity-game-services-sample/tree/main/ugs-backend)

- ### Fill in environment variables

  + [Retrieve the **API Secret key**](https://dashboard.openfort.xyz/apikeys) and fill in the [``openfortApiKey``](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/55f91101ff4eae301b6d8c98458023c6b3b34d6e/ugs-backend/CreateOpenfortPlayer.js#L5) variable.
  + [Use the same **API Secret key**](https://dashboard.openfort.xyz/apikeys) to fill the [``openfortApiKey``](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/55f91101ff4eae301b6d8c98458023c6b3b34d6e/ugs-backend/MintNft.js#L5) variable.
  + [Retrieve the **NFT Contract API ID**](https://dashboard.openfort.xyz/assets) and fill in the [``nftContractId``](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/55f91101ff4eae301b6d8c98458023c6b3b34d6e/ugs-backend/MintNft.js#L9C11-L9C25) variable.
  + [Retrieve the **Policy API ID**](https://dashboard.openfort.xyz/policies) and fill in the [``policyId``](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/55f91101ff4eae301b6d8c98458023c6b3b34d6e/ugs-backend/MintNft.js#L10C11-L10C19) variable.
  + Fill in the [``secretKey``](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/55f91101ff4eae301b6d8c98458023c6b3b34d6e/ugs-backend/VerifyReCaptcha.js#L6) with your **reCAPTCHA secret key**.

- ### Deploy to UGS
  Follow [the official documentation steps](https://docs.unity.com/ugs/en-us/manual/cloud-code/manual/scripts/getting-started#Deploy_a_Cloud_Code_script) to deploy each and all of the Cloud Code scripts.

## Set up [``unity-client``](https://github.com/openfort-xyz/ugs-unity-game-services-sample/tree/main/unity-client)

- ### Link to UGS project
  Follow the [official documentation steps](https://docs.unity.com/ugs/manual/authentication/manual/get-started#Link_your_project) to link the ``unity-client`` to your UGS Project.

- ### Fill in needed variables
  On the [Main scene](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/tree/main/unity-client/Assets/Scenes), select the *ReCaptchaController* game object and fill in your **reCAPTCHA site key**:

  <div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample5_ad65b1ce5b.png?updated_at=2024-02-02T14:36:19.490Z"
      alt='Set up Unity client: Fill in reCAPTCHA site key'
    />
    </div>

## Build to WebGL

> **IMPORTANT:** You need to build this sample to the root folder of the GitHub Pages repository.

In Unity go to *File --> Build Settings*, select *Web* platform and choose ***Build***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample6_d388084266.png?updated_at=2024-02-02T14:36:16.787Z"
      alt='Build to WebGL'
    />
    </div>

Find your GitHub Pages repository root folder and choose ***Select Folder***:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample7_735cd4abc0.png?updated_at=2024-02-02T14:36:15.587Z"
      alt='Build to WebGL: Select folder'
    />
    </div>

After the build is completed, go to your GitHub Pages repository URL and you should see the *Sign in* panel:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample8_c283fc6617.png?updated_at=2024-02-02T14:36:19.482Z"
      alt='Build to WebGL: Sign in panel'
    />
    </div>

Open the browser console to check that reCAPTCHA V3 is loaded and ready:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample9_f55bb4e5a2.png?updated_at=2024-02-02T14:36:21.283Z"
      alt='Build to WebGL: reCAPTCHA ready'
    />
    </div>

Click the ***Sign in*** button and after some authentication-related logs, the *Mint* panel should appear. In the console logs, we should see a positive reCAPTCHA verification score. Because we passed the reCAPTCHA verification, an Openfort player should have been created:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample10_97c88ae6ee.png?updated_at=2024-02-02T14:36:21.617Z"
      alt='Build to WebGL: positive score'
    />
</div>

If you click the ***Mint*** button, the game will go through another reCAPTCHA verification. After a brief period, you should see the *NFT minted* panel with the Openfort player account address in a button:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/unity_recaptcha_sample11_86ae061a30.png?updated_at=2024-02-05T11:21:12.597Z"
      alt='Build to WebGL: NFT minted'
    />
</div>

Click on the ***Address*** button to open Etherscan and see that the transaction is confirmed:

<div align="center">
    <img
      width="50%"
      height="50%"
      src="https://blog-cms.openfort.xyz/uploads/ugs_integration_8_6b345bd148.png?updated_at=2023-12-14T16:05:00.991Z"
      alt='Etherscan'
    />
</div>

## Code walkthrough

All this is thanks to the [**``ReCaptcha.jslib plugin``**](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/main/unity-client/Assets/Plugins/WebGL/ReCaptcha.jslib) located in the Unity client, which allows the execution of reCAPTCHA V3 from the client. The ``.jslib`` methods are called from the [**``ReCaptchaController.cs``**](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/main/unity-client/Assets/Scripts/ReCaptchaController.cs).

Then, when reCAPTCHA V3 gets the response token from the execution, we validate it in the backend through [**``VerifyReCaptcha.js``**](https://github.com/dpradell-dev/openfort-unity-recaptcha-sample/blob/main/ugs-backend/VerifyReCaptcha.js)

## Conclusion

Upon completing the above steps, your Unity game will be fully integrated with Openfort and Google reCAPTCHA V3. Always remember to test every feature before deploying to guarantee a flawless player experience.

## Get support
If you found a bug or want to suggest a new [feature/use case/sample], please [file an issue](../../issues).

If you have questions, or comments, or need help with code, we're here to help:
- on Twitter at https://twitter.com/openfortxyz
- on Discord: https://discord.com/invite/t7x7hwkJF4
- by email: support+youtube@openfort.xyz
