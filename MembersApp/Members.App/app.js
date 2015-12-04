'use strict';

//var url = "http://localhost:22863";
var url = "http://wcf.programski-kod.com";


// This is your Telerik Backend Services API key.
var bsApiKey = 'FgOQ9nmn5OdCyJKH';

// This is the scheme (http or https) to use for accessing the Telerik Backend Services REST API.
var bsScheme = 'http';

// This is your Google Cloud Console API project number. It is required by Google in order to enable push notifications for your Android. You do not need it for iOS.
var googleApiProjectNumber = '584231691568';



(function() {
    var app = {
        data: {}
    };

    var bootstrap = function() {
        $(function() {
            app.mobileApp = new kendo.mobile.Application(document.body, {
                transition: "slide",
                skin: 'ios7',
                initial: 'components/home/home.html'
            });
        });
    };

    if (window.cordova) {
        document.addEventListener('deviceready', function() {

            app.settings.loadSettings();

            if (!bsApiKey || bsApiKey === 'BACKEND_SERVICES_API_KEY') {
                $("#messageParagraph").html("Missing API key!<br /><br />It appears that you have not filled in your Backend Services API key.<br/><br/>Please go to scripts/app/main.js and enter your Everlive API key at the beginning of the file.");
                $("#registerButton").hide();
            }
            else if ((!googleApiProjectNumber || googleApiProjectNumber === 'GOOGLE_API_PROJECT_NUMBER') && device.platform.toLowerCase() == "android") {
                $("#messageParagraph").html("Missing Google API Project Number!<br /><br />It appears that you have not filled in your Google API project number. It is required for push notifications on Android.<br/><br/>Please go to scripts/app/main.js and enter your Google API project number at the beginning of the file.");
                $("#registerButton").hide();
            }


            if (navigator && navigator.splashscreen) {
                navigator.splashscreen.hide();
            }

            var element = document.getElementById('appDrawer');
            if (typeof(element) != 'undefined' && element !== null) {
                if (window.navigator.msPointerEnabled) {
                    $('#navigation-container').on('MSPointerDown', 'a', function(event) {
                        app.keepActiveState($(this));
                    });
                } else {
                    $('#navigation-container').on('touchstart', 'a', function(event) {
                        app.keepActiveState($(this));
                    });
                }
            }

            bootstrap();
        }, false);
    } else {
        bootstrap();
    }

    app.keepActiveState = function _keepActiveState(item) {
        var currentItem = item;
        $('#navigation-container li a.active').removeClass('active');
        currentItem.addClass('active');
    };

    window.app = app;


    window.onerror = function myErrorHandler(errorMsg, url, lineNumber) {
        alert(url + ":" + lineNumber + ": " + errorMsg);
        app.mobileApp.pane.loader.hide();
        return false;
    }


    app.isOnline = function() {
        if (!navigator || !navigator.connection) {
            return true;
        } else {
            return navigator.connection.type !== 'none';
        }
    };
}());



function showToast(txt) {
    try
    {
        window.plugins.toast.show(txt, 'short', 'bottom', function (a) { console.log('toast success: ' + a) }, function (b) { alert('toast error: ' + b) });
    }
    catch (ex)
    {
    	
    }
    
}

function Vibrate(time) {
    navigator.vibrate(time)
}
