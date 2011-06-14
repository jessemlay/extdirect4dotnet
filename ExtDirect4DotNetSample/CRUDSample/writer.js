
Ext.Direct.addProvider(Ext.app.REMOTING_API); 

var Employee = Ext.data.Record.create([
    {name: 'firstname'},                  // map the Record's "firstname" field to the row object's key of the same name
    {name: 'job', mapping: 'occupation'}  // map the Record's "job" field to the row object's "occupation" key
]);

var proxy = new Ext.data.DirectProxy({
    api: {
        read: CRUDSampleMethods.read,
        create: CRUDSampleMethods.create,
        update: CRUDSampleMethods.update,
        destroy: CRUDSampleMethods.destroy
    }
});

var reader = new Ext.data.JsonReader({
    totalProperty: 'results',
    successProperty: 'success',
    idProperty: 'id',
    root: 'rows'
    },[
	    { name: 'id' },
	    { name: 'email', allowBlank: false },
	    { name: 'first', allowBlank: false },
	    { name: 'last', allowBlank: false }
    ]
);

var writer = new Ext.data.JsonWriter({
    encode: false,
    writeAllFields: false
});

var store = new Ext.data.DirectStore({	    
    id: 'person',	    
    proxy : proxy,
    reader: reader,
    writer: writer, 
    baseParams: {dummy:'blubb'},
    batch: false,
    remoteSort: true,
    listeners: {
        load: function(result) {	        
        },
        loadexception: function() {

        },
        scope: this
    }
});


var userColumns =  [
    {header: "ID", width: 40, sortable: true, dataIndex: 'id'},
    {header: "Email", width: 100, sortable: true, dataIndex: 'email', editor: new Ext.form.TextField({})},
    {header: "First", width: 50, sortable: true, dataIndex: 'first', editor: new Ext.form.TextField({})},
    {header: "Last", width: 50, sortable: true, dataIndex: 'last', editor: new Ext.form.TextField({})}
];


Ext.onReady(function() {
	Ext.QuickTips.init();


// load the store immeditately
store.load({params: {
    start: 0,          // specify params for the first page load if using paging
    limit: 10
}});
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
		bbar: new Ext.PagingToolbar({
            store: store,       // grid and PagingToolbar using same store
            displayInfo: true,
            pageSize: 10,
            prependButtons: true,
            items: [
                'text 1'
            ]
        }),
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
    
});