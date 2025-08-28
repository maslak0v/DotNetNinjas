window.initializeRecaptcha = (dotNetHelper) => {
    const metaTag = document.querySelector('meta[name="recaptcha-sitekey"]');
    const siteKey = metaTag?.getAttribute('content');
    
    try {
        grecaptcha.render('recaptcha', {
            'sitekey': siteKey,
            'callback': (response) => {
                dotNetHelper.invokeMethodAsync('SetRecaptchaResponse', response);
            }
        });
    } catch (error) {
        console.error("reCAPTCHA error:", error);
    }
};