const axios = require("axios-0.21");

module.exports = async ({ params, context, logger }) => {
  try {
    const openfortApiKey = "" // Replace with your API key
    const openfortUrl = `https://api.openfort.xyz/v1/transaction_intents`;

    const chainId = 80001;
    const nftContractId = ""; // Replace
    const policyId = ""; // Replace

    const transactionIntentData = {
      chainId: chainId,
      player: params["playerId"],
      interactions: [
        {
          contract: nftContractId,
          functionName: "mint",
          functionArgs: [params["accountAddress"]]
        }
      ],
      policy: policyId 
    };
    const auth = {
      username: openfortApiKey,
      password: '' // Password is not required
    };
    txResponse = await axios.post(openfortUrl, transactionIntentData, { auth });

    return txResponse.data;
  } catch (err) {
    logger.error("Failed to call out to Openfort", {"error.message": err.message});
    throw err;
  }
};

// Declare the required parameters
module.exports.params = { 
  "playerId" : { "type": "String", "required": true },
  "accountAddress" : { "type": "String", "required": true },
}