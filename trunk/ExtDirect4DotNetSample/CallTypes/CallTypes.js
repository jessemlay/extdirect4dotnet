

Ext.Direct.addProvider(Ext.app.REMOTING_API);

Ext.onReady(function() {

    var echo = new Ext.Panel({

        title: 'Echo check',

        columnWidth: 0.2,

        height: 400,

        tools: [{

            id: 'refresh',

            handler: function()

            {

                CallTypes.Echo('Echo please...', function(e, result)

                {

                    echo.body.update(result.result);

                });

            }

        }]

    });



    var time = new Ext.Panel({

        title: 'Time check',

        columnWidth: 0.2,

        height: 400,

        tools: [{

            id: 'refresh',

            handler: function()

            {

                CallTypes.GetTime(function(e, result)

                {

                    time.body.update(result.result);

                });

            }

        }]

    });

    

    var upload = new Ext.form.FormPanel({

        title: 'UploadHttpRequestParam',

        columnWidth: 0.2,

        height: 410,

        bodyStyle: 'padding: 15px;',

        fileUpload: true,

        items: [{

            xtype: 'textfield',

            name: 'firstName',

            fieldLabel: 'First Name'

        },{

            fieldLabel: 'Upload an image!',

            xtype: 'textfield',

            inputType: 'file',
            name: 'file'

        }],

        buttons: [{

            text: 'Submit',

            handler: function(){

                var f = upload.getEl().child('form');

                CallTypes.UploadHttpRequestParam(f, function(e, data) {

                    Ext.Msg.alert('Result', Ext.encode(data.result));
                    upload2.setTitle('File uploaded!');

                    upload2.header.highlight();

                });

            }

        }]

    });

var upload2 = new Ext.form.FormPanel({

        title: 'UploadNamedParameter',

        columnWidth: 0.2,

        height: 410,

        bodyStyle: 'padding: 15px;',

        fileUpload: true,

        items: [{

            xtype: 'textfield',

            name: 'firstName',

            fieldLabel: 'First Name'

        },{

            fieldLabel: 'Upload an image!',

            xtype: 'textfield',

            inputType: 'file',
            name: 'file'

        }],

        buttons: [{

            text: 'Submit',

            handler: function(){

                var f = upload2.getEl().child('form');

                CallTypes.UploadNamedParameter(f, function(e, data) {

                    
                    Ext.Msg.alert('Result', Ext.encode(data.result));
                    upload.setTitle('File uploaded!');

                    upload.header.highlight();

                });

            }

        }]

    });
    

    var form = new Ext.form.FormPanel({

        title: 'Form Submit',

        columnWidth: 0.2,

        height: 410,

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

            xtype: 'numberfield',

            name: 'age',

            fieldLabel: 'Age'

        }],

        buttons: [{

            text: 'Submit',

            handler: function(){

                var f = form.getEl().child('form');

                CallTypes.SaveMethod_Form(f, function(e, data) {

                console.log(e, data);

                    var r = data.result;

                    var name = r.firstName + ' ' + r.lastName;

                    Ext.Msg.alert('Hello', 'Hi ' +name +', you are ' + r.age);

                });

            }

        }]

    });



    var dates = new Ext.Panel({

        title: 'Date check',

        columnWidth: 0.2,

        height: 400,

        tools: [{

            id: 'refresh',

            handler: function() {

                var d = new Date();

                CallTypes.DateSample(d, d.add(Date.DAY, -1), function(e, result) {

                    dates.body.update(result.result);

                });

            }

        }]

    });



    var ct = new Ext.Container({

        autoEl: {},

        layout: 'column',

        items: [echo, time, upload, form, upload2/*dates*/],

        renderTo: document.body

    });

});


setTimeout(function() {
    Ext.get('loading').remove();
    Ext.fly('loading-mask').fadeOut({
        remove: true
    });
    //store.load();

}, 250);