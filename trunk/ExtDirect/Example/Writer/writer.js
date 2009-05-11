

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
Ext.override(Ext.data.DataReader, {
    update: function(rs, data) {
        if (Ext.isArray(rs)) {
            for (var i = rs.length - 1; i >= 0; i--) {
                if (Ext.isArray(data)) {
                    this.update(rs.splice(i, 1).shift(), data.splice(i, 1).shift());
                }
                else {
                    // weird...rs is an array but data isn't??  recurse but just send in the whole data object.
                    // the else clause below will detect !this.isData and throw exception.
                    this.update(rs.splice(i, 1).shift(), data);
                }
            }
        }
        else {
            if (!this.isData(data)) {
                // TODO: create custom Exception class to return record in thrown exception.  Allow exception-handler the choice
                // to commit or not rather than blindly rs.commit() here.
                rs.commit();
                throw new Error("DataReader#update received invalid data from server.  Please see docs for DataReader#update");
            }
            rs.data = this.extractValues(data, rs.fields.items, rs.fields.items.length);
            rs.commit();
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
	var writer = new Ext.data.JsonWriter({
	returnJson: true,
	    writeAllFields: true
	});

	var store = new Ext.data.DirectStore({	    	    
	    api: {
	        load: TestAction.getData,
	        create: TestAction.createData,
	        save: TestAction.saveData,
	        destroy: TestAction.deleteData
	    },
	    remoteSort: true,
	    reader: reader,
	    writer: writer, 	// <-- plug a DataWriter into the store just as you would a Reader
	    paramsAsHash: true,
	    batchSave: false,
	    prettyUrls: false,
	    listeners: {
	        load: function(result) {	        
	        },
	        loadexception: function() {

	        },
	        scope: this
	    }
	});
store.load();


var userColumns =  [
    {header: "ID", width: 40, sortable: true, dataIndex: 'id'},
    {header: "Email", width: 100, sortable: true, dataIndex: 'email', editor: new Ext.form.TextField({})},
    {header: "First", width: 50, sortable: true, dataIndex: 'first', editor: new Ext.form.TextField({})},
    {header: "Last", width: 50, sortable: true, dataIndex: 'last', editor: new Ext.form.TextField({})}
];
Ext.onReady(function() {
	Ext.QuickTips.init();

	var userForm = new App.user.Form({
		renderTo: 'user-form',
		listeners: {
			create : function(fpanel, data) {	// <-- custom "create" event defined in App.user.Form class
				var rec = new userGrid.store.recordType(data);
				userGrid.store.insert(0, rec);
			}
		}
	});

	// create user.Grid instance (@see UserGrid.js)
	var userGrid = new App.user.Grid({
		renderTo: 'user-grid',
		store: store,
		columns : userColumns,
		listeners: {
			rowclick: function(g, index, ev) {
				var rec = g.store.getAt(index);
				userForm.loadRecord(rec);
			},
			destroy : function() {
				userForm.getForm().reset();
			}
		}
});
setTimeout(function() {
    Ext.get('loading').remove();
    Ext.fly('loading-mask').fadeOut({
        remove: true
    });
}, 250);
});