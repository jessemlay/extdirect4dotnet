<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Grid.aspx.cs" Inherits="ExtDirect.Example.Grid.Grid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
        <style type="text/css">
      #loading-mask{
        position:absolute;
        left:0;
        top:0;
        width:100%;
        height:100%;
        z-index:20000;
        background-color:white;
    }
    #loading{
        position:absolute;
        left:45%;
        top:40%;
        padding:2px;
        z-index:20001;
        height:auto;
    }
    #loading a {
        color:#225588;
    }
    #loading .loading-indicator{
        background:white;
        color:#444;
        font:bold 13px tahoma,arial,helvetica;
        padding:10px;
        margin:0;
        height:auto;
    }
    #loading-msg {
        font: normal 10px arial,tahoma,sans-serif;
    }
    </style> 

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
    </div>
    <div id="loading-mask" style=""></div>
    <div id="loading">
        <div class="loading-indicator"><img src="../../shared/extjs/images/extanim32.gif" width="32" height="32" style="margin-right:8px;float:left;vertical-align:top;"/>Ext Direct <br /><span id="loading-msg">Loading ...</span></div>
    </div>
    <link rel="stylesheet" type="text/css" href="../../ext-3.0/resources/css/ext-all.css" />
 	<script type="text/javascript" src="../../ext-3.0/adapter/ext/ext-base.js"></script> 	
    <script type="text/javascript" src="../../ext-3.0/ext-all.js"></script>
    <script type="text/javascript" src="../../Proxy.ashx"></script>
    <script type="text/javascript" src="GridDirect.js"></script>
    <h1>Direct</h1>
    <p>The js is not minified so it is readable. See <a href="GridDirect.js">Direct.js</a>.</p>
    <p>Download Code <a href="Direct.zip">DirectGrid.zip</a>.</p>
    <div id="dCont" style=" margin-left:10px; margin-top:10px;"></div>
    </form>
</body>
</html>
