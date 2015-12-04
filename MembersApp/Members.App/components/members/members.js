'use strict';

app.members = kendo.observable({
    onShow: function () {

    },
    afterShow: function () {

        console.log(app.members.dataSourceChanged);

        if (app.members.dataSourceChanged == true) {
            app.members.reloadListView();
        }
    },

    dataSource: {},
  
    dataSourceChanged: false,

    reloadListView: function() {

        var listView = $("#listViewMembers").data("kendoMobileListView");

        if (listView == null)
        {
            console.log('no list view.....')
            return;
        }


        app.members.set("dataSource", app.members.loadDatasource());

        listView.setDataSource(app.members.dataSource);
               

        app.members.dataSourceChanged = false;
    },


    initListView: function() {
          
        
        console.log("initListView..........");

        app.members.set("dataSource", app.members.loadDatasource());


        $("#listViewMembers").kendoMobileListView({
            template: $("#contactsTemplate").html(),
            dataSource: app.members.get("dataSource"),
            filterable: {
                field: "FirstName",
                operator: "startswith",
                placeholder: "Search..."

            },
            endlessScroll: false,
            loadMore: false,
            pullToRefresh: true
            
                
        });

    },

    loadDatasource: function () {

        console.log('Loading...');

        app.mobileApp.pane.loader.show();

        var dataSource = new kendo.data.DataSource({
            type: "odata-v4",
            pageSize: 50,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "FirstName", dir: "asc" },
            transport: {

                read: url + "/odata/Members",

                create: {
                    url: url + "/odata/Members",
                    contentType: "application/json; charset=utf-8",
                    type: "POST"
                },

                update: {
                    url: function (data) {
                        return url + "/odata/Members(" + data.MemberId + ")";
                    },
                    contentType: "application/json; charset=utf-8",
                    type: "PUT"
                },
                destroy: {
                    url: function (data) {
                        return url + "/odata/Members(" + data.MemberId + ")";
                    }
                },
                dataType: "json"
                //parameterMap: function (data, type) {
                //    if (type != "read") {
                //        return JSON.stringify(data);
                //    }
                //}

            },
            schema: {

                model: {
                    id: "MemberId",
                    fields: {
                        MemberId: { type: "number" },
                        FirstName: { type: "string" },
                        LastName: { type: "string" }

                    }
                }
            },
            requestEnd: function (e) {
                var response = e.response;
                var type = e.type;
                
                app.mobileApp.pane.loader.hide();
            }

        });

        dataSource.bind("error", function (e) {

            alert('Datasource ' + e.status);

        });

        dataSource.read();

        console.log("Loaded...");
        console.log(dataSource);

        return dataSource;

    },


    goToDetails: function (e) {
  
        if (e.dataItem == undefined)
            return;

        console.log(e);

        var nav = function () { app.mobileApp.navigate("#components/details/details.html?uid=" + e.dataItem.uid) };
  
        var item = app.members.get("dataSource").getByUid(e.dataItem.uid);
  
        console.log(item);

        app.details.loadData(item, nav);

       
    },
    save: function () {

        console.log("save");

        var dataSource = app.members.get("dataSource");

        dataSource.sync();
        

        //var listView = $("#listViewMembers").data("kendoMobileListView");

        //listView.setDataSource(dataSource);

        //var item = listView.dataSource.data()[0];

        //item.FirstName = "asd";

        //console.log(item);

    },
    add: function (e) {

        var dataSource = app.members.get("dataSource");

        var member = dataSource.add({ FirstName: '', LastName: '', Street: '', City: '', PB: '' , MemberId: 0, Locked: false, Visible: true, Version: 1, Gender: 'm' });

        var nav = function () { app.mobileApp.navigate("#components/details/details.html?uid=" + member.uid) };

        app.details.loadData(member, nav);
        
    }
  
    
});