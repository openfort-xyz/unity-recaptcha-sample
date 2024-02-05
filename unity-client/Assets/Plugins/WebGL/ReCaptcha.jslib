mergeInto(LibraryManager.library, {
  LoadReCaptcha: function(siteKey) {
    var siteKeyString = UTF8ToString(siteKey);
    console.log('Loading ReCaptcha with site key:', siteKeyString);

    var script = document.createElement('script');
    script.src = `https://www.google.com/recaptcha/api.js?render=${siteKeyString}`;
    document.head.appendChild(script);

    // Error handling for script loading
    script.onerror = function() {
      console.error('Failed to load ReCaptcha script');
      SendMessage('ReCaptchaController', 'OnReCaptchaError', 'Failed to load ReCaptcha script');
    };
  },

  ExecuteReCaptcha: function(siteKey) {
    var siteKeyString = UTF8ToString(siteKey);
    console.log('Executing ReCaptcha with site key:', siteKeyString);

    if (typeof grecaptcha === 'undefined') {
      console.error('ReCaptcha is not loaded');
      SendMessage('ReCaptchaController', 'OnReCaptchaError', 'ReCaptcha is not loaded');
      return;
    }

    grecaptcha.ready(function() {
      grecaptcha.execute(siteKeyString, {action: 'submit'}).then(function(token) {
        // Call the C# callback with the token
        SendMessage('ReCaptchaController', 'OnReCaptchaResolved', token);
      }, function(error) {
        // Error handling for ReCaptcha execution
        console.error('Failed to execute ReCaptcha', error);
        SendMessage('ReCaptchaController', 'OnReCaptchaError', 'Failed to execute ReCaptcha: ' + error);
      });
    });
  }
});