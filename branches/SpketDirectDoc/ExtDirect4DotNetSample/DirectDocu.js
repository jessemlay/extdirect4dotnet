
/** */ 
var TreeAction = {
/**
 * Method description 
 * @param id Id the Node to load 
 * @param params extra Prameter you want to call the loadmethod with 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 getChildNodes : function (id, paramscb, scope) {}};
/** */ 
var CallTypes = {/**
 * Method description 
 * @param {string} name parameter Descirption
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 Echo : function (name, cb, scope) {},/**
 * Method description 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 GetTime : function (cb, scope) {},/**
 * Method description 
 * @param form a dom form Node or an Ext.form.BasicForm 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 UploadHttpRequestParam : function (form,cb, scope) {},/**
 * Method description 
 * @param form a dom form Node or an Ext.form.BasicForm 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 UploadNamedParameter : function (form,cb, scope) {},/**
 * Method description 
 * @param {string} age parameter Descirption
 * @param {string} firstName parameter Descirption
 * @param {string} lastName parameter Descirption
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 SaveMethod : function (age, firstName, lastName, cb, scope) {},/**
 * Method description 
 * @param form a dom form Node or an Ext.form.BasicForm 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 SaveMethod : function (form,cb, scope) {},/**
 * Method description 
 * @param {string} d1 parameter Descirption
 * @param {string} d2 parameter Descirption
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 DateSample : function (d1, d2, cb, scope) {}};
/**
 * 
            A sample Ext.Direct - Action - Class
            
            Class is markt as DirectAction via the Direct Action attribute [DirectAction]
            
            The Class extends ActionWithSessionState wich makes the curren SessionState in this Action
            availible via the Session member.
             */ 
var CRUDSampleMethods = {/**
 * Method description 
 * @param {Ext.data.Record} rec 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 create : function (rec,cb, scope) {},/**
 * Method description 
 * @param {Object} params parameter of the Store 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 read : function (params,cb, scope) {},/**
 * Method description 
 * @param id Id of the Record to update 
 * @param record {Object} the recordFields you wish to update 
 * @param cb {Function} The Callback Function (gets called when the Method returned from the Server) 
 * @param scope {Object} The Scope to call the Callbackfunction with. 
 */ 
 update : function (id, recordcb, scope) {},/**
 * Method description 
 * @param id Id of the Record to delete 
 * @param