'use strict';

app.settings = kendo.observable({
    onShow: function () {

        app.settings.loadSettings();

    },
    afterShow: function () {
    },

    url: '',

    loadSettings: function () {

        this.set('url', localStorage.getItem('url'));

        if (this.get('url') == null || this.get('url') == '')
            this.set('url', 'http://wcf.programski-kod.com');


        url = this.get('url');

        //loadDatasource();

    },
    saveSettings: function () {

        localStorage.setItem('url', this.get('url'));

        url = this.get('url');

        app.members.dataSourceChanged = true;

    },


    registerForPush: function () {

        app.PushNotification = window.plugins.pushNotification;
        if (device.platform == 'android' || device.platform == 'Android') {

            app.mobileApp.pane.loader.show();

            app.PushNotification.register(
                app.settings.pushSuccessHandler,
                app.settings.pushErrorHandler, {
                    "senderID": googleApiProjectNumber,
                    "ecb": "onNotificationGCM"
                });
        }
        else {
            alert("Push not supported");

        }
    },

    pushErrorHandler: function (error) {

        app.mobileApp.pane.loader.hide();

        alert('Error: ' + error);
    },
    pushSuccessHandler: function (result) {

        var obj = { DeviceUUID: device.uuid, PushToken: result };

        $.ajax({
            type: "POST",
            dataType: "json",
            contentType: "application/json",
            url: url + "/odata/Device",
            data: JSON.stringify(obj),
            success: function () {

                app.mobileApp.pane.loader.hide();

                showToast('PushToken registerd');

                Vibrate(100);

            },
            error: function (xhr, ajaxOptions, thrownError) {

                app.mobileApp.pane.loader.hide();

                alert(xhr.status);
                alert(thrownError);
            }
        });

    }


});

