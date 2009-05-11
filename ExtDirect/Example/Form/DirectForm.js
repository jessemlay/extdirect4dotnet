Ext.onReady(function() {

    
    Ext.Direct.addProvider(Ext.app.REMOTING_API);
    var out = new Ext.form.DisplayField({
        cls: 'x-form-text',
        id: 'out'
    });
    var simple = new Ext.FormPanel({
        labelWidth: 75, // label settings here cascade unless overridden
        url: 'save-form.php',
        frame: true,
        title: 'Simple Form',
        bodyStyle: 'padding:5px 5px 0',
        width: 350,
        defaults: { width: 230 },
        defaultType: 'textfield',

        items: [{
            fieldLabel: 'First Name',
            name: 'first',
            allowBlank: false
        }, {
            fieldLabel: 'Last Name',
            name: 'last'
        }, {
            fieldLabel: 'Company',
            name: 'company'
        }, {
            fieldLabel: 'Email',
            name: 'email',
            vtype: 'email'
        }, new Ext.form.TimeField({
            fieldLabel: 'Time',
            name: 'time',
            minValue: '8:00am',
            maxValue: '6:00pm'
        })
        ],

        buttons: [{
            text: 'Save',
            handler: function() {
                TestAction.submit(simple.getForm().el, function(result, e) {
                    var t = e.getTransaction();
                    out.append(String.format('<p><b>Successful call to {0}.{1} with response:</b><xmp>{2}</xmp></p>',
                           t.action, t.method, Ext.encode(result)));
                    out.el.dom.parentNode.scrollTop = out.el.dom.parentNode.scrollHeight;
                });
            }
        }, {
            text: 'Cancel'
}]
        });

        simple.render(document.body);
        var p = new Ext.Panel({
            title: 'Remote Call Log',
            width: 600,
            height: 200,
            layout: 'fit',
            items: [out]

        }).render('cont');
        setTimeout(function() {
            Ext.get('loading').remove();
            Ext.fly('loading-mask').fadeOut({
                remove: true
            });
        }, 250);

    });