'use strict';

app.test = kendo.observable({
    onShow: function () { },
    afterShow: function () { },

    id: 0,
    displayName: '',
    phoneNumbers: '',

    showUrl: function() {
    
        alert(url);
    
    },

    pick: function () {

        var self = this;

        navigator.contacts.pickContact(function (contact) {

            console.log('The following contact has been selected:' + JSON.stringify(contact));

            self.set('id', contact.id);
            self.set('displayName', contact.displayName);

            self.set('phoneNumbers', ',');

            contact.phoneNumbers.forEach(function (entry) {
                
                self.set('phoneNumbers', self.get('phoneNumbers') + ', ' + entry.value);

            });

            self.set('phoneNumbers', self.get('phoneNumbers').replace(',, ',''))


        }, function (err) {
            console.log('Error: ' + err);
        });
    },
    share: function () {

        window.plugins.socialsharing.share(this.get("displayName") + '\r\n' + this.get('phoneNumbers'), 'Contact ' + this.get('displayName'));

    },
    toast: function () {

        window.plugins.toast.show('Hello there!', 'short', 'bottom', function (a) { console.log('toast success: ' + a) }, function (b) { alert('toast error: ' + b) });
    },
   
    slideRight: function () {
        this.slide("right", "components/home/home.html");
    },
    slideLeft: function () {
        this.slide("left", "components/home/home.html");
    },

    slide: function (direction, page) {

        var highspeed = $("#slide-highspeed-switch").data("kendoMobileSwitch");
        var overlap = $("#slide-overlap-switch").data("kendoMobileSwitch");

        if (!this.checkSimulator()) {
            var options = {
                "direction": direction,
                "duration": highspeed.check() ? 400 : 800,
                "slowdownfactor": overlap.check() ? 4 : 1,
                "iosdelay": 0,
                "androiddelay": 0,
                "winphonedelay": 0,
                "href": null,
                "fixedPixelsTop": 0,
                "fixedPixelsBottom": 60
            };
            window.plugins.nativepagetransitions.slide(
              options,
              function (msg) { console.log("SUCCESS: " + JSON.stringify(msg)) },
              function (msg) { alert("ERROR: " + JSON.stringify(msg)) }
            );
        }
    },

    scan: function () {

        cordova.plugins.barcodeScanner.scan(
          function (result) {

              app.test.set('barcodeResult', result.text);
              app.test.set('barcodeType', result.format);

          },
          function (error) {
              alert("Scanning failed: " + error);
          },
          {
              "preferFrontCamera": false,
              "showFlipCameraButton": false,
              "prompt": "Place a barcode inside the scan area", // supported on Android only
              "formats": "EAN13,QR_CODE,PDF_417" // default: all but PDF_417 and RSS_EXPANDED
          });
    },

    checkSimulator: function () {
        if (window.navigator.simulator === true) {
            alert('This plugin is not available in the simulator.');
            return true;
        } else if (window.plugins.nativepagetransitions === undefined) {
            alert('Plugin not found. Maybe you are running in AppBuilder Companion app which currently does not support this plugin.');
            return true;
        } else {
            return false;
        }
    }
});


