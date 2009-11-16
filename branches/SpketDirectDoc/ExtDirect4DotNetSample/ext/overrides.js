
Ext.override(Ext.data.DataProxy, {
    request : function(action, rs, params, reader, callback, scope, options) {
        if (!this.api[action] && !this.load) {
            throw new Ext.data.DataProxy.Error('action-undefined', action);
        }
        params = params || {};
        if ((action === Ext.data.Api.actions.read) ? this.fireEvent("beforeload", this, params) : this.fireEvent("beforewrite", this, action, rs, params) !== false) {
            return this.doRequest.apply(this, arguments);
        }
        else {
            callback.call(scope || this, null, options, false);
        }
    }
});

Ext.override(Ext.data.DirectProxy, {
    doRequest : function(action, rs, params, reader, callback, scope, options) {
        var args = [],
            directFn = this.api[action] || this.directFn;

        switch (action) {
            case Ext.data.Api.actions.create:
                args.push(params.jsonData);		// <-- create(Hash)
                break;
            case Ext.data.Api.actions.read:
                // If the method has no parameters, ignore the paramOrder/paramsAsHash.
                if(directFn.directCfg.method.len > 0){
                    if(this.paramOrder){
                        for(var i = 0, len = this.paramOrder.length; i < len; i++){
                            args.push(params[this.paramOrder[i]]);
                        }
                    }else if(this.paramsAsHash){
                        args.push(params);
                    }
                }
                break;
            case Ext.data.Api.actions.update:
                args.push(params.jsonData);        // <-- update(Hash/Hash[])
                break;
            case Ext.data.Api.actions.destroy:
                args.push(params.jsonData);        // <-- destroy(Int/Int[])
                break;
        }

        var trans = {
            params : params || {},
            request: {
                callback : callback,
                scope : scope,
                arg : options
            },
            reader: reader
        };

        args.push(this.createCallback(action, rs, trans), this);
        return directFn.apply(window, args);
    }
});


Ext.override(Ext.data.Store, {
    destroyRecord : function(store, record, index) {
        if (this.modified.indexOf(record) != -1) {  // <-- handled already if @cfg pruneModifiedRecords == true
            this.modified.remove(record);
        }
        //if (!record.phantom) {
            this.removed.push(record);

            // since the record has already been removed from the store but the server request has not yet been executed,
            // must keep track of the last known index this record existed.  If a server error occurs, the record can be
            // put back into the store.  @see Store#createCallback where the record is returned when response status === false
            record.lastIndex = index;

            if (this.autoSave === true) {
                this.save();
            }
        //} 
    },

    execute : function(action, rs, options, /* private */ batch) {
        // blow up if action not Ext.data.CREATE, READ, UPDATE, DESTROY
        if (!Ext.data.Api.isAction(action)) {
            throw new Ext.data.Api.Error('execute', action);
        }
        // make sure options has a fresh, new params hash
        options = Ext.applyIf(options||{}, {
            params: {}
        });
        if(batch !== undefined){
            this.addToBatch(batch);
        }
        // have to separate before-events since load has a different signature than create,destroy and save events since load does not
        // include the rs (record resultset) parameter.  Capture return values from the beforeaction into doRequest flag.
        var doRequest = true;

        if (action === 'read') {
            Ext.applyIf(options.params, this.baseParams);
            doRequest = this.fireEvent('beforeload', this, options);
        }
        else {
            
            // if Writer is configured as listful, force single-record rs to be [{}] instead of {}
            // TODO Move listful rendering into DataWriter where the @cfg is defined.  Should be easy now.
            if (this.writer.listful === true && this.restful !== true) {
                rs = (Ext.isArray(rs)) ? rs : [rs];
            }
            // if rs has just a single record, shift it off so that Writer writes data as '{}' rather than '[{}]'
            else if (Ext.isArray(rs) && rs.length == 1) {
                rs = rs.shift();
            }
            
            // Write the action to options.params
            if ((doRequest = this.fireEvent('beforewrite', this, action, rs, options)) !== false) {
                // execute merging logic here....
                // make shure we use an array here
                var records = (Ext.isArray(rs)) ? rs : [rs];
                for(var i = 0, len = records.length; i < len; i++) {
                    var rec = records[i];
                    // check if this record allready has an transaction that is queued
                    if(rec.transaction && rec.transaction.getStatus() == "queued") {
                        // check if it is an delteAction
                        if(action == 'destroy') {
                            // if action is destroy cancel all queued Actions since they not nessecary any more...
                            rec.transaction.cancel();
                            if(rec.phantom) {
                                // if it is a phantom we dont want an request to be done...
                                // not yet TODO add some logic
                                doRequest = false;
                                // remove the record from the removed array
                                this.removed.splice(this.removed.indexOf(rec), 1);
                            }
                        } else {
                            // set doRequest to false since the request is allready in the queue
                            doRequest = false;
                            // merge current action with queud one.
                            var data = this.writer[action + 'Record'](rs);
                            var args = rec.transaction.args[0][this.reader.meta.root];
                            Ext.apply(args, data);
                        }
                    } else if(rec.transaction && rec.transaction.getStatus() == "sending") {
                        // wait for the destory or update execution till the record is no phantom anymore ...
                        if(action == 'destroy' || action == 'update') {
                            doRequest = false;
                        }
                    }
                }  
                if(doRequest)              
                    this.writer.apply(options.params, this.baseParams, action, rs);
                
            }
            
            
            
        }
        if (doRequest !== false) {
            // Send request to proxy.
            if (this.writer && this.proxy.url && !this.proxy.restful && !Ext.data.Api.hasUniqueUrl(this.proxy, action)) {
                options.params.xaction = action;    // <-- really old, probaby unecessary.
            }
            // Note:  Up until this point we've been dealing with 'action' as a key from Ext.data.Api.actions.
            // We'll flip it now and send the value into DataProxy#request, since it's the value which maps to
            // the user's configured DataProxy#api
            // TODO Refactor all Proxies to accept an instance of Ext.data.Request (not yet defined) instead of this looooooong list
            // of params.  This method is an artifact from Ext2.
            var transaction = this.proxy.request(Ext.data.Api.actions[action], rs, options.params, this.reader, this.createCallback(action, rs, batch), this, options);
            if(transaction instanceof Ext.Direct.Transaction) {
                if(rs instanceof Ext.data.Record) {
                    rs.transaction = transaction;
                }
            }
        }
        return doRequest;
    }
});


Ext.override(Ext.direct.RemotingProvider , {

    onData: function(opt, success, xhr){
        if(success){
            var events = this.getEvents(xhr);
            for(var i = 0, len = events.length; i < len; i++){
                var e = events[i],
                    t = this.getTransaction(e);
                this.fireEvent('data', this, e);
                if(t){
                    this.doCallback(t, e, true);
                    t.done();
                    Ext.Direct.removeTransaction(t);
                }
            }
        }else{
            var ts = [].concat(opt.ts);
            for(var i = 0, len = ts.length; i < len; i++){
                var t = this.getTransaction(ts[i]);
                if(t && t.retryCount < this.maxRetries){
                    t.retry();
                }else{
                    var e = new Ext.Direct.ExceptionEvent({
                        data: e,
                        transaction: t,
                        code: Ext.Direct.exceptions.TRANSPORT,
                        message: 'Unable to connect to the server.',
                        xhr: xhr
                    });
                    this.fireEvent('data', this, e);
                    if(t){
                        this.doCallback(t, e, false);
                        Ext.Direct.removeTransaction(t);
                    }
                }
            }
        }
    },


    doCall : function(c, m, args){
        var data = null, hs = args[m.len], scope = args[m.len+1];

        if(m.len !== 0){
            data = args.slice(0, m.len);
        }

        var t = new Ext.Direct.Transaction({
            provider: this,
            args: args,
            action: c,
            method: m.name,
            data: data,
            cb: scope && Ext.isFunction(hs) ? hs.createDelegate(scope) : hs
        });

        if(this.fireEvent('beforecall', this, t) !== false){
            Ext.Direct.addTransaction(t);
            this.queueTransaction(t);
            this.fireEvent('call', this, t);
            return t;
        }
    },

    createMethod : function(c, m){
        var f;
        if(!m.formHandler){
            f = function(){
                return this.doCall(c, m, Array.prototype.slice.call(arguments, 0));
            }.createDelegate(this);
        }else{
            f = function(form, callback, scope){
                this.doForm(c, m, form, callback, scope);
            }.createDelegate(this);
        }
        f.directCfg = {
            action: c,
            method: m
        };
        return f;
    },
    
    combineAndSend : function(){
        console.log("combineAndSend:", this.callBuffer);
        var len = this.callBuffer.length;
        if(len > 0){
            this.doSend(len == 1 ? this.callBuffer[0] : this.callBuffer);
            this.callBuffer = [];
        }
    },

    doSend : function(data){
        console.log("doSend was called with:", data);
        var o = {
            url: this.url,
            callback: this.onData,
            scope: this,
            ts: data,
            timeout: this.timeout
        }, callData;

        if(Ext.isArray(data)){
            callData = [];
            for(var i = 0, len = data.length; i < len; i++){
                callData.push(this.getCallData(data[i]));
                data[i].sent();
            }
        }else{
            callData = this.getCallData(data);
            data.sent();
        }
        console.log("calldata was created from data:", callData);
        if(this.enableUrlEncode){
            var params = {};
            params[Ext.isString(this.enableUrlEncode) ? this.enableUrlEncode : 'data'] = Ext.encode(callData);
            o.params = params;
        }else{
            o.jsonData = callData;
        }
        Ext.Ajax.request(o);
    },

    queueTransaction: function(t){
        if(t.form){
            this.processForm(t);
            return;
        }
        
        this.callBuffer.push(t);
        console.log("queuedTransaction");
        console.log(this.callBuffer);
        if(this.enableBuffer){
            if(!this.callTask){
                this.callTask = new Ext.util.DelayedTask(this.combineAndSend, this);
            }
            this.callTask.delay(Ext.isNumber(this.enableBuffer) ? this.enableBuffer : 10);
        }else{
            this.combineAndSend();
        }
    },

    removeTransaction: function(t) {
       
        for(var found = 0; found < this.callBuffer.length; found++) {
            if(this.callBuffer[found].tid == t.tid) {
                
                this.callBuffer.splice(found, 1);
                return true;
            }
        }
        return false;
    }
});


/**
 * @class Ext.Direct.Transaction
 * @extends Object
 * <p>Supporting Class for Ext.Direct (not intended to be used directly).</p>
 * @constructor
 * @param {Object} config
 */
Ext.Direct.Transaction = Ext.extend(Ext.util.Observable, {
    constructor : function(config){
        Ext.apply(this, config);
        this.tid = ++Ext.Direct.TID;
        this.retryCount = 0;
        this.status = "queued";
    },
    
    getStatus : function(){
        return this.status;
    },
    
    send: function(){
        this.status = "queued";
        this.provider.queueTransaction(this);
    },
    
    /**
     * gets called by the provider when this Transaction has done its way into a Ajaxrequest
     */
    sent: function(){
        this.status = "sending";
        //this.fireEvent('sent', this);
    },
    
    done : function() {
        this.status = "executed";
       // this.fireEvent('executed', this);
    },
    
    /**
     * cancels this transaction by removing it from the providers call Buffer 
     * @return {boolean} returns true if canceling of this transaction was successfull 
     *                  returns false if it couldnt get find in the callBuffer (indicats that this transaction has been send allready)  
     */
    cancel: function() {
        this.status = "canceled";
        return this.provider.removeTransaction(this);
    },

    retry: function(){
        this.retryCount++;
        this.send();
    },

    getProvider: function(){
        return this.provider;
    }
});

