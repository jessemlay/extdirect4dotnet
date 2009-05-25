

Ext.Direct.addProvider(Ext.app.REMOTING_API);
Ext.form.Action.DirectSubmit = function(form, options){
    Ext.form.Action.DirectSubmit.superclass.constructor.call(this, form, options);
};

Ext.extend(Ext.form.Action.DirectSubmit, Ext.form.Action.Submit, {
    /**
    * @cfg {Ext.data.DataReader} errorReader <b>Optional. JSON is interpreted with no need for an errorReader.</b>
    * <p>A Reader which reads a single record from the returned data. The DataReader's <b>success</b> property specifies
    * how submission success is determined. The Record's data provides the error messages to apply to any invalid form Fields.</p>.
    */
    /**
    * @cfg {boolean} clientValidation Determines whether a Form's fields are validated
    * in a final call to {@link Ext.form.BasicForm#isValid isValid} prior to submission.
    * Pass <tt>false</tt> in the Form's submit options to prevent this. If not defined, pre-submission field validation
    * is performed.
    */
    type : 'directsubmit',

    // private
    run : function(){
        var o = this.options;/*
        var method = this.getMethod();
        var isGet = method == 'GET';*/
        if(o.clientValidation === false || this.form.isValid()){
            this.form.api.submit(this.form, this.success, this);
        }else if (o.clientValidation !== false){ // client validation failed
            this.failureType = Ext.form.Action.CLIENT_INVALID;
            this.form.afterAction(this, false);
        }
    },
    
    processResponse : function(response){
        this.response = response;
        /*if(!response.responseText && !response.responseXML){
            return true;
        }*/
        this.result = this.handleResponse(response);
        return this.result;
    },
    
    handleResponse : function(response){
        if(this.form.errorReader){
            var rs = this.form.errorReader.read(response);
            var errors = [];
            if(rs.records){
                for(var i = 0, len = rs.records.length; i < len; i++) {
                    var r = rs.records[i];
                    errors[i] = r.data;
                }
            }
            if(errors.length < 1){
                errors = null;
            }
            return {
                success : rs.success,
                errors : errors
            };
        }
        return response;
    }

    
});


Ext.form.Action.ACTION_TYPES['directsubmit'] = Ext.form.Action.DirectSubmit;    

Ext.override(Ext.form.BasicForm, {
    submit : function(options){
        if(this.standardSubmit){
            var v = this.isValid();
            if(v){
                this.el.dom.submit();
            }
            return v;
        }
        this.doAction((this.api && (typeof this.api.submit == 'function')) ? 'directsubmit' : 'submit', options);
        return this;
    },
});


Ext.override(Ext.direct.RemotingProvider, {
    doForm : function(c, m, form, callback, scope){
        var t = new Ext.Direct.Transaction({
            provider: this,
            action: c,
            method: m.name,
            args:[form, callback, scope],
            cb: scope && typeof callback == 'function' ? callback.createDelegate(scope) : callback
        });
        var params = {};
        if(this.fireEvent('beforecall', this, t) !== false){
            Ext.Direct.addTransaction(t);

            if(form instanceof Ext.form.BasicForm) {
                Ext.apply(params, form.basicParams);
                form = form.getEl();
            } else {
                form = Ext.getDom(form);
            }
            var isUpload = String(form.getAttribute("enctype")).toLowerCase() == 'multipart/form-data';

            Ext.apply(params, {
                extTID: t.tid,
                extAction: c,
                extMethod: m.name,
                extType: 'rpc',
                extUpload: String(isUpload)
            });
            if(callback && typeof callback == 'object'){
                Ext.apply(params, callback.params);
            }
            Ext.Ajax.request({
                url: this.url,
                params: params,
                callback: this.onData,
                scope: this,
                form: form,
                isUpload: isUpload,
                ts: t
            });
        }
    }
});

Ext.onReady(function() {
Ext.QuickTips.init();
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

        title: 'UploadHttpRequestParam test',


        api: {
            submit: CallTypes.UploadHttpRequestParam
         
        },
        columnWidth: 0.2,

        height: 410,

        bodyStyle: 'padding: 15px;',

        fileUpload: true,
        defaults: {msgTarget : 'side'},
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
                upload.getForm().submit();
                /*var f = upload.getEl().child('form');

                CallTypes.UploadHttpRequestParam(f, function(e, data) {

                    Ext.Msg.alert('Result', Ext.encode(data.result));
                    upload2.setTitle('File uploaded!');

                    upload2.header.highlight();

                });
*/
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