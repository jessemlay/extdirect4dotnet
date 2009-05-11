Ext.onReady(function() {

    Ext.override(Ext.data.DirectProxy, {
        createCallback: function(action, reader, cb, scope, arg) {
            return {
                callback: (action == 'load') ? function(result, e) {
                    if (typeof result == 'string') {
                        result = Ext.decode(result);
                    }
                    if (!e.status) {
                        this.fireEvent(action + "exception", this, e, result);
                        cb.call(scope, null, arg, false);
                        return;
                    }
                    var records;
                    try {
                        records = reader.readRecords(result);
                    }
                    catch (ex) {
                        this.fireEvent(action + "exception", this, e, result, ex);
                        cb.call(scope, null, arg, false);
                        return;
                    }
                    this.fireEvent(action, this, e, arg);
                    cb.call(scope, records, arg, true);
                } : function(result, e) {
                    if (typeof result == 'string') {
                        result = Ext.decode(result);
                    }
                    if (!e.status) {
                        this.fireEvent(action + "exception", this, e);
                        cb.call(scope, null, e, false);
                        return;
                    }
                    this.fireEvent(action, this, result, e, arg);
                    cb.call(scope, result, e, true);
                },
                scope: this
            }
        }
    });
    
    Ext.Direct.addProvider(Ext.app.REMOTING_API); 
    
    var reader = new Ext.data.JsonReader({
        totalProperty: 'total',
        successProperty: 'success',
        idProperty: 'id',
        root: 'data'
    }, [
	    { name: 'id' },
	    { name: 'email', allowBlank: false },
	    { name: 'first', allowBlank: false },
	    { name: 'last', allowBlank: false }
    ]);
    var store = new Ext.data.DirectStore({
        paramsAsHash: false,
        api: {
            load: TestAction.getData
        },
        reader: reader,
        listeners: {
            load: function(result) {
                console.log('load', store)
            },
            loadexception: function() {

            },
            scope: this
        }
    });
    store.load();
    
    var userColumns = [
        { header: "ID", width: 40, sortable: true, dataIndex: 'id' },
        { header: "Email", width: 100, sortable: true, dataIndex: 'email', editor: new Ext.form.TextField({}) },
        { header: "First", width: 50, sortable: true, dataIndex: 'first', editor: new Ext.form.TextField({}) },
        { header: "Last", width: 50, sortable: true, dataIndex: 'last', editor: new Ext.form.TextField({}) }
    ];
    var grid = new Ext.grid.EditorGridPanel({
        store: store,
        columns: userColumns,
        viewConfig: {
            forceFit: true
        },
        sm: new Ext.grid.RowSelectionModel({
            singleSelect: true
        })
    });

    var p = new Ext.Panel({
        title: 'Remote Call Grid',
        width: 600,
        height: 300,
        layout: 'fit',
        items: [
			grid
		]
    }).render(Ext.getBody());

    setTimeout(function() {
        Ext.get('loading').remove();
        Ext.fly('loading-mask').fadeOut({
            remove: true
        });
        // console.log(store);
    }, 250);


});