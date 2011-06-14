

Ext.Direct.addProvider(Ext.app.REMOTING_API);

var proxy = new Ext.data.DirectProxy({
    api: {
        read : Guestbook.read,
        destroy : Guestbook.destroy
    }
});

var reader = new Ext.data.JsonReader({
    totalProperty: 'results',
    successProperty: 'success',
    idProperty: 'id',
    root: 'rows'
    },[
	    { name: 'firstName' },
	    { name: 'lastName', allowBlank: false },
	    { name: 'message', allowBlank: false },
	    { name: 'pictureName', allowBlank: false }
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
    writer : writer,
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


var entrieColumns =  [
    {header: "First Name", width: 40, sortable: true, dataIndex: 'firstName'},
    {header: "Last Name", width: 100, sortable: true, dataIndex: 'lastName', editor: new Ext.form.TextField({})},
    {header: "Message", width: 50, sortable: true, dataIndex: 'message', editor: new Ext.form.TextField({})},
    {header: "pictureName", width: 50, sortable: true, dataIndex: 'pictureName', editor: new Ext.form.TextField({})}
];


Ext.onReady(function() {
    Ext.QuickTips.init();
    store.load();
    var guestbookgrid = new Ext.grid.GridPanel({
        store : store,
        columns : entrieColumns,
        height: 300,
        title: 'Framed with Row Selection and Horizontal Scrolling'
    });
    
    var guestbookform = new Ext.form.FormPanel({
        api: {
            submit: Guestbook.addEntry
        },
        
        fileUpload: true,
        border : false,
        height: 250,
        bodyStyle: 'padding: 15px;',
        items: [{
            xtype: 'textfield',
            name: 'firstName',
            fieldLabel: 'First Name'
        },{
            xtype: 'textfield',
            name: 'lastName',
            fieldLabel: 'Last Name'
        },{ 
            xtype: 'fileuploadfield',
            name: 'picture',
            fieldLabel: 'Your Picture'
        },{
            xtype: 'textarea',
            name: 'message',
            width: 300,
            fieldLabel: 'Message'
        }],
        buttons: [{
            text: 'Submit',
            handler: function(){
                guestbookform.getForm().submit();
                guestbookform.getForm().on('actioncomplete', function(){store.reload();}, this, {single:true})
            }
        }]
    });

    var tpl = new Ext.XTemplate(
        '<tpl for=".">',
            '<div class="itemwrap" style="margin:5px; backround color"><div ><img src="delete.gif" class="item-delete"></div>',
            '<div ><div style="float:left;"><img src="./uploadedFiles/{pictureName}" height="60" style="padding:5px;" title="{firstName}"></div><div style="padding-top:5px; padding-bottom:5px;"><b>{lastName} {firstName}</b></div>{message}<div style="clear: both;"></div></div>',
            '</div>',
        '</tpl>',
        '<div class="x-clear"></div>'
    );
    viewpanel = new Ext.Panel({
        id:'guestbook',
        frame:true,
        autoHeight:true,
        layout:'fit',
        border : false,
        style : 'backround-color:#ffffff;',

        items: new Ext.DataView({
            id : 'view',
            style : 'backround-color:#ffffff;',

            store: store,
            tpl: tpl,
            autoHeight:true,
            multiSelect: true,
            overClass:'item-over',
            itemSelector:'div.itemwrap',
            emptyText: 'No images to display'
           
        })
    });

    handleException = function(){
        Ext.MessageBox.prompt('Password Required...', 'To Delete a entrie not written by you, you need to log in.', function(btn, val) {
            if(btn == "cancel")
                return;
            if(val != "") {
                Guestbook.login(val, function(loggedin) {if(!loggedin) {handleException()}});
            }
        });
    }

    Ext.getCmp('view').on('click', function(a, index, c, d){
        if(Ext.fly(d.target).hasClass('item-delete')) {
            store.on('exception', handleException);
            store.remove(store.getAt(index));
            console.log('destroy...');
            
            //store.save();
        }
            
    })
    
    var panel = new Ext.Panel({
        title : 'Leave a Message',
        items :[
            guestbookform, 
            viewpanel
        ]
    });
    
    panel.render(document.body);

});

