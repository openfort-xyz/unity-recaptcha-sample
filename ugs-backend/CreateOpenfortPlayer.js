const axios = require("axios-0.21");

module.exports = async ({ params, context, logger }) => {
  try {
    const openfortApiKey = "" // Replace with your API key

    // Set Openfort API auth
    const auth = {
      username: openfortApiKey,
      password: '' // Password is not required
    };

    // Create Openfort player
    const playersApiUrl = `https://api.openfort.xyz/v1/players`;
    const playerData = {
      name: params["playerName"]
    };
    playerResponse = await axios.post(playersApiUrl, playerData, { auth });

    // Create player account
    const chainId = 80001;
    const accountsApiUrl = `https://api.openfort.xyz/v1/accounts`;
    const accountData = {
      chainId: chainId,
      player: playerResponse.data.id
    };
    accountResponse = await axios.post(accountsApiUrl, accountData, { auth });
  
    return JSON.stringify({
      playerId: playerResponse.data.id,
      accountAddress: accountResponse.data.address
    });
  } catch (err) {
    logger.error("Failed to call out to Openfort", {"error.message": err.message});
    throw err;
  }
};

// Declare the required 'token' parameter
module.exports.params = { "playerName" : { "type": "String", "required": true } }