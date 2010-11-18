
Ext.onReady(function(){
    Ext.Direct.addProvider(Ext.app.REMOTING_API);

    var tree = new Ext.tree.TreePanel({
        width: 400,
        height: 400,
        autoScroll: true,
        renderTo: document.body,
        root: new Ext.tree.AsyncTreeNode({
		        text: 'Root-Node',
		        draggable:false,
		        // erst beim ersten refresh expandieren
		        expanded: false,
		        cls: 'x-tree-noicon',
		        id:'1'
		    })       ,
        loader: new Ext.tree.TreeLoader({
            paramsAsHash: true,
            directFn: TreeAction.getChildNodes
        }),
        fbar: [{
            text: 'Reload root',
            handler: function(){
                tree.getRootNode().reload();
            }
        }]
    });
});
setTimeout(function() {
    Ext.get('loading').remove();
    Ext.fly('loading-mask').fadeOut({
        remove: true
    });
    /*store.load({params: {
            start: 0,          // specify params for the first page load if using paging
            limit: myPageSize,
            foo:   'bar'
    }});*/

}, 250);