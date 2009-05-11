Ext.onReady(function() {
  
    
    Ext.Direct.addProvider(Ext.app.REMOTING_API, {
        type: 'polling',
        url: '../Pool.ashx'

    });   
    
    var out = new Ext.form.DisplayField({
        cls: 'x-form-text',        
        id: 'out'
    });
    var text = new Ext.form.TextField({
        width: 300,
        emptyText: 'Echo input'
    });
    var call = new Ext.Button({
        text: 'Echo',
        handler: function() {
            TestAction.doEcho(text.getValue(), function(result, e) {
                var t = e.getTransaction();
                out.append(String.format('<p><b>Successful call to {0}.{1} with response:</b><xmp>{2}</xmp></p>',
                           t.action, t.method, Ext.encode(result)));
                out.el.dom.parentNode.scrollTop = out.el.dom.parentNode.scrollHeight;
            });
        }
    });
    var num = new Ext.form.TextField({
        width: 80,
        emptyText: 'firstValue',
        style: 'text-align:left;'
    });
    var num2 = new Ext.form.TextField({
        width: 80,
        emptyText: 'Second value',
        style: 'text-align:left;'
    });
    var multiply = new Ext.Button({
        text: 'Multiply',
        handler: function() {
            TestAction.multiply(num.getValue(), num2.getValue(),function(result, e) {
                var t = e.getTransaction();
                if (e.status) {
                    out.append(String.format('<p><b>Successful call to {0}.{1} with response:</b><xmp>{2}</xmp></p>',
                            t.action, t.method, Ext.encode(result)));
                } else {
                    out.append(String.format('<p><b>Call to {0}.{1} failed with message:</b><xmp>{2}</xmp></p>',
                            t.action, t.method, e.message));
                }
                out.el.dom.parentNode.scrollTop = out.el.dom.parentNode.scrollHeight;
            });
        }
    });
    text.on('specialkey', function(t, e) {
        if (e.getKey() == e.ENTER) {
            call.handler();
        }
    });

    var p = new Ext.Panel({
        title: 'Remote Call Log',
        width: 600,
        height: 300,        
        autoScroll:true,
        layout: 'fit',
        items: [out],
        bbar: [text, call, '-', num, num2, multiply]
    }).render("dCont");
    Ext.Direct.on('message', function(e) {
        out.append(String.format('<p><i>{0}</i></p>', e.data));
        out.el.dom.parentNode.scrollTop = out.el.dom.parentNode.scrollHeight;
    });
    setTimeout(function() {
        Ext.get('loading').remove();
        Ext.fly('loading-mask').fadeOut({
            remove: true
        });
    }, 250);

});