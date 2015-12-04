'use strict';

app.details = kendo.observable({
    onShow: function () { },
    afterShow: function () { },

    Member: {},

    loadData: function (item, callback) {

        //$.getJSON(url, function (data) {
        //    app.details.set("oObj", data);
        //    callback();
        //    app.mobileApp.pane.loader.hide();
        //});


        this.set("Member", item);


        //app.details.Member = item;

        callback();

    },
    saveData: function () {

        app.members.save();

        app.mobileApp.navigate("#:back");
    },
    deleteData: function () {

        var dataSource = app.members.get("dataSource");

        dataSource.remove(app.details.Member);

        dataSource.sync();

        app.mobileApp.navigate("#:back");
    }
  
});

