const axios = require("axios-0.21");

module.exports = async ({ params, context, logger }) => {
  try {
    const verifyUrl = `https://www.google.com/recaptcha/api/siteverify`;
    const secretKey = ""; // Replace with your secret key
    const responseToken = params["token"]; // The token received from ReCaptcha

    //TODO check if token param is valid (not empty or undefined)

    const response = await axios.post(verifyUrl, null, {
      params: {
        secret: secretKey,
        response: responseToken
      }
    });

    // Check if success is true
    if (!response.data.success) {
      // Throw an error if success is false
      logger.error(response.data);
      throw new Error('ReCaptcha verification failed');
    }

    // Return the score if the checks pass
    return response.data.score;

  } catch (err) {
    logger.error("Failed to verify ReCaptcha token", {"error.message": err.message});
    throw err;
  }
};

// Declare the required 'token' parameter
module.exports.params = { "token" : { "type": "String", "required": true } }