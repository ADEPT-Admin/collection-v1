/*
#########################################################################
# CVS:       $Id: utilities.js,v 1.106 2006/02/28 08:40:17 chai Exp $
#
# Copyright: (c) 2004-2006 Adaptivate Corporation. All rights reserved
#
# Author:    Panumat Wiwatmaikul (Tong)
#
# Description:
#########################################################################
*/

/*******************************************************************************

* NAME: utilities.js

* AUTHOR: Panumat Wiwatmaikul , IntelSys Co.,Ltd.
* DATE  : 01/01/2548
* Copyright (c) 2004 IntelSys Co.,Ltd. All rights reserved.
* COMMENT:
*
* LAST MODIFIED : 8/2/2548
********************************************************************************/

/********************************************************
 * Browser detection
 ********************************************************/
var NAV = navigator.userAgent.toLowerCase();

var IE = (NAV.indexOf("msie") >= 0 && document.all) ;
var IE4 = ( IE && !document.getElementByID ) ;
var IEMAC = ( IE & NAV.indexOf("mac") >= 0 ) ;

var NN4 = ( document.layers && typeof document.classes != "undefined") ;
var NN6 = ( typeof window.getComputedStyle != "undefined" && typeof document.createRange != "undefined") ;

var W3C = ( IE && NN6 && document.getElementById ) ;

var CE = ( document.captureEvents && document.releaseEvents ) ;

var PX = ( NN4 ) ? '' : 'px' ;

// Dialog const
var pgW = 290 ; // Progress Width
var pgH = 177 ; // Progress Height

/**
 * Check float number
 */
function is_float ( ) {
    if ( (event.keyCode>='0'.charCodeAt() && event.keyCode <= '9'.charCodeAt() ) || event.keyCode == '.'.charCodeAt() )
        event.returnValue = true;
    else {
        event.returnValue = false;
    }
}

/**
 * Check number
 */
function is_num () {
    if (event.keyCode>='0'.charCodeAt() && event.keyCode <= '9'.charCodeAt() ) {
          event.returnValue = true;
          return true;
    }else {
        event.returnValue = false;
        return false;
    }
}


/**
 * Finds whether a variable is a number or a numeric string 
 * @param value is mixed variable
 *@return returns TRUE if var is a number or a numeric string, FALSE otherwise. 
 */
function isInteger( value ) {
    var re = /^[\+\-]?\d*$/ ;
    return re.test( value ) ;
}

/**
 * Finds whether a variable is a float
 * @param value is mixed variable
 *@return returns TRUE if var is a float, FALSE otherwise.
 */
function isFloat( value ) {
    var re = /^[\+\-]?\d*\.?\d*$/ ;
    return re.test( value ) ;
}

/**
 * Finds whether a variable is a alphabetic or numberic
 * @param value is mixed variable
 *@return returns TRUE if var is a alphabetic or numberic, FALSE otherwise.
 */
function isAlphaNum( value ) {
    var re = /^[\w ]+$/ ;
    return re.test ( value ) ;
}

/**
 * Open new window by set center screen
 * @param url is the link location
 * @param w_name is the name of new window
 */
function  openNWC( url, w_name ) {
    var w = screen.width; // Get the width of the screen
    var h = screen.height; // Get the height of the screen

    // The size of the Window
    var win_width = w - 350;
    var win_height = h - 250;

    // Where to place the Window
    var left = (w - win_width)/2;
    var top = (h - win_height)/2;

    var features = 'width='+win_width+',height='+win_height
    features += ',top='+top+',left='+left+',screenX='+left+',screenY='+top;

    window.open(url,w_name, features) ;
}

/**
 *  Set max screen
 */
function maxWindow() {
    window.moveTo(0,0);

    if (document.all){
        top.window.resizeTo(screen.availWidth,screen.availHeight);
    }
    else if (document.layers||document.getElementById)    {
        if (top.window.outerHeight<screen.availHeight||top.window.outerWidth<screen.availWidth)
        {
            top.window.outerHeight = screen.availHeight;
            top.window.outerWidth = screen.availWidth;
        }
    }
}

/**
 * Launch full screen on open new window
 * @param url is the link location
 * @param name is the name of new window
 */
function launchFull(url, name) {
    var str = "left=0,screenX=0,top=0,screenY=0";

    if (window.screen) {
          var ah = screen.availHeight - 30;
          var aw = screen.availWidth - 10;
          str += ",height=" + ah;
          str += ",innerHeight=" + ah;
          str += ",width=" + aw;
          str += ",innerWidth=" + aw;
          str += ", scrollbars=yes" ;
    } else {
        str += ", scrollbars=yes" ;
        str += ",resizable"; // so the user can resize the window manually
    }
    window.open(url, name, str);
}

/**
 * Show dialog with parameter
 * @param url is string that specifies the URL of the document to load and display.
 * @param isModal is boolean to tell modal or modeless
 * @param args is parameter to a callback function or an object  [option]
 * @param width is width of dialog default 350 pixel [option]
 * @param height is height of dialog default 380 pixel [option]
 * @return the returned value from dialog
 */
function showDialog(url, isModal,args, width,height ) {
    var w = width?width: 350 ;
    var h = height?height: 385 ;
    var sDialogFeature = "dialogHeight: "+h+"px; dialogWidth: "+w+"px; center: yes; help: no; resizable:yes; status:no";
    var win;
    if ( isModal && window.showModalDialog) {
        return  window.showModalDialog(url,args,sDialogFeature) ;
    }else if ( !isModal && window.showModelessDialog) {
        return window.showModelessDialog(url,args,sDialogFeature) ;
    } else {
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("This application requires Microsoft Internet Explorer 6.0 or better")+"." ) ;
        }else{
            alert( "This application requires Microsoft Internet Explorer 6.0 or better." ) ;
        } 
    }
}

/**
 * Redirect page
 * @param url is the link location
 */
function redirect(url) {
    if ( window.location )
    {
        window.location.href = url ;
    }else {
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported")+"." ) ;
        }else{
            alert('Your browser doesn\'t supported .') ;
        } 
        
        return false ;
    }
}

/**
 * Reload page
 */
function reload() {
    if ( window.location ) {
        //window.location.reload() ;
        //history.go(0);
        window.location.href=window.location.href ;
    }else {
        if(typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported")+"." ) ;
        }else{
            alert('Your browser doesn\'t supported .') ;
        } 
         return false ;
    }
}

/**
 * Load IFrame page
 * @param iframeName is the name of inner frame
 * @param url is the link location
 */
function loadIframe(iframeName, url) {
  if ( window.frames[iframeName] ) {
    window.frames[iframeName].location = url;
    return true;
  }else return false;
}

/*
 #############################################################
 #                                             String utilities                                                                     #
 #############################################################
 */
 /**
 * Trim string
 */
String.prototype.trim = function() {

    // skip leading and trailing whitespace
    // and return everything in between
    var str=this;

    if( str.replace) {
        str=str.replace(/^\s*(.*)/, "$1");
        str=str.replace(/(.*?)\s*$/, "$1");
    }else {
        if (str.length > 0) while (str.indexOf(' ') == 0) str = str.substr(1);
        if (str.length > 0) while(str.lastIndexOf(' ') == str.length - 1) str = str.substr(0, str.length - 1);
    }

    return str;
}

/**
 * Replace regular expression
 */
String.prototype.ereg_replace = function ereg_replace(pattern, replacement) {
    var str = this ;
    if ( str.replace) {
        return str.replace(pattern, replacement ) ;
    }else {
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported replace method")+"." ) ;
        }else{
            alert('Your browser doesn\'t supported replace method.') ;            
        }       
    }
}

/**
 * Encode/Decode
 */
String.prototype.urlencode = function () {
    var str = this ;
    if ( encodeURIComponent ) {
        str = encodeURIComponent(addBackslash(str)) ;
    } else {
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported encodeURIComponent method")+"." ) ;
        }else{
            alert('Your browser doesn\'t supported encodeURIComponent method.') ;
        }                 
    }
    return str;
}

String.prototype.urldecode = function () {
    var str = this ;
    if ( decodeURIComponent ) {
        str = removeBackslash(decodeURIComponent(str)) ;
    } else {
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported encodeURIComponent method")+"." ) ;
        }else{
            alert('Your browser doesn\'t supported encodeURIComponent method') ;
        }        
    }
    return str;
}

/**
 * Convert all applicable characters to HTML entities
 */
String.prototype.html2entities = function () {
    var str = this ;
    try {
        if ( isNaN( str ) )
            str = html2entities(str) ;

    } catch ( e ) {
        alert( e.message ) ;
    }
    return str;
}

/**
 * Convert all HTML entities to their applicable characters
 */
String.prototype.entities2html = function () {
    var str = this ;
    try {
        if ( isNaN( str ) )
            str = entities2html(str) ;
    } catch ( e ) {
        alert( e.message ) ;
    }
    return str;
}

/*
* encode special characters string before sent to request
* @param str is string for encode
*/
function encodeMyHtml(str) {
     str = str.replace(/\//g,"%2F");
     str = str.replace(/\?/g,"%3F");
     str = str.replace(/=/g,"%3D");
     str = str.replace(/&/g,"%26");
     str = str.replace(/@/g,"%40");
     str = str.replace(/;/g,"%3B");
     str = str.replace(/'/g,"%27");
     str = str.replace(/"/g,"%22");
     return str;
   }

/*
 * add back slash to special character ', ", \
 */
function addBackslash( str ) {
    str = str.replace(/\\/g,"\\\\" ) ; // equal \\
    str = str.replace(/\"/g,"\\\"" ) ; // equal \"
    str = str.replace(/\'/g,"\\'" ) ; // equal \'
    return str ;
}

/*
 * remove back slash from \', \", \\
 */
function removeBackslash( str ) {
    str = str.replace(/\\\\/g,"\\" ) ; // equal \\
    str = str.replace(/\\\"/g,"\"" ) ; // equal \"
    str = str.replace(/\\'/g,"\'" ) ; // equal \'
    return str ;
}

/*
* Convert special characters to the most common html character entities
*/

function html2entities( str ) {
    var re=/[<>"'&]/g ;
    var tmp = "" ;
    if ( isNaN(str) && str.trim() != "" )
        for ( var i= 0; i < str.length; i++ )
            tmp += str.charAt(i).replace(re, function(m){return replacechar(m)}) ;
    return tmp ;
}

function replacechar(match) {
    if (match=="<")
        return "&lt;" ;
    else if (match==">")
        return "&gt;" ;
    else if (match=="\"")
        return "&quot;" ;
    else if (match=="'")
        return "&#039;" ;
    else if (match=="&")
        return "&amp;" ;
    /*else if ( match == " " )  // deplicated
        return "&nbsp;" ;*/
}

/*
*  Convert all HTML entities to their applicable characters
*/

function entities2html( str ) {
    if ( isNaN(str) && str.trim() != "" ) {
        str = str.replace(/&lt;/g, "<") ;
        str = str.replace(/&gt;/g, ">") ;
        str = str.replace(/&quot;/g, "\"") ;
        str = str.replace(/&#039;/g, "'") ;
        str = str.replace(/&amp;/g, "&") ;
        //str = str.replace(/&nbsp;/g, " " ) ; // deplicated
    }
    return str ;
}


/**
 * Returns the first index at which a given element can be found in the array, or -1 if it is not present.
 */
function indexInArray( theArray, value ){
    var arLength = theArray.length;
    for(var i=0; i < arLength ; i++ ) {
        if (theArray[i].trim() == value.trim() ){
            return i;
        }
    }
    return -1;
}

/*
 #############################################################
 #     Image utilities                                        #
 #############################################################
 */

/**
* Image placeholder
* @param img_src is the source image object
* @param img_target is the target image object
*/
function imagePlaceholder(img_src,img_target) {
    var dir = img_src.value ;
    var img_name = dir.substring(dir.lastIndexOf("\\")+1,dir.length ) ;
    img_target.src=dir ;
    img_target.alt= img_name ;
    validformFile = /(.jpg|.JPG|.gif|.GIF)$/ ;
    if (img_name.trim() == "" || !validformFile.test(dir))
    {
        img_target.style.display = "none" ;
    }else
        img_target.style.display = "" ;
}

/**
* Check for image types only
* @param obj_File is the file object of input type=file
* @return boolean
*              true - is image file type .jpg|.JPG|.gif|.GIF
*              false - if not
*/
function isImageFile(obj_File) {
    if( obj_File.value != ""  || obj_File.value != ""  ) {
        validformFile = /(.jpg|.JPG|.gif|.GIF)$/ ;
        if ( obj_File.value !="" && ! validformFile.test(obj_File.value ) ) {
            obj_File.focus();
            obj_File.select() ;
            return false;
        } else return true;
    }
}

/**
 * Check pdf types only
 * @param obj_File is the file object of input type=file
 * @return boolean
 *              true - is image file type .PDF|.pdf
 *              false - if not
 */
function isPDFFile(obj_File) {
     if( obj_File.value != ""  || obj_File.value != ""  ) {
        validformFile = /(.PDF|.pdf)$/ ;
        if ( obj_File.value !="" && ! validformFile.test(obj_File.value ) ) {
            obj_File.focus();
            obj_File.select() ;
            return false;
        } else return true ;
    }
}

/**
 * Check image or pdf types only
 * @param obj_File is the file object of input type=file
 * @return boolean
 *              true - is image file type .jpg|.JPG|.gif|.GIF|.PDF|.pdf
 *              false - if not
 */
function isImageOrPDFFile(obj_File) {
    if( obj_File.value != ""  || obj_File.value != ""  ) {
        validformFile = /(.jpg|.JPG|.gif|.GIF|.PDF|.pdf)$/ ;
        if ( obj_File.value !="" && ! validformFile.test(obj_File.value ) ) {
            obj_File.focus();
            return false;
        } else return true ;
    }
}

/*
 #############################################################
 #                                             Table utilities                                                                     #
 #############################################################
 */
/***********************************************************
 * For highlight row onClick
//desc: Highlight current row by click any cell
//argument: backColor - highlight bgColor of the row
// textColor - highlight text color
//call: like <tr onClick="highLightTR('#c9cc99','cc3333');" id="trO4">

************************************************************/
var preEl ;
var orgBColor;
var orgTColor;

function highLightTR(backColor,textColor, eventObj ){
        if(typeof(preEl)!='undefined') {
            try{
                changeColor(preEl,orgBColor,orgTColor);
            }catch(e){alert(e);}
        }

       var el ;

       if (IE) {
            //  Internet Explorer
            el = eventObj.srcElement;
            el = el.parentElement;
            orgBColor = el.bgColor;
            orgTColor = el.style.color;
        } else if ( NN6)   {
            //  Netscape 6+
            el = eventObj.target;
            el = el.parentNode;
            orgBColor = el.bgColor;
            orgTColor = el.style.color;
        }else {
            if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
                alert( TRANSLATION_DICT("Your browser doesn\'t supported")+"." ) ;
            }else{
                alert("Your browser doesn\'t supported.") ;
            }               
        }

        try{
            changeColor(el,backColor,textColor);
        }catch(e){alert();}

        preEl = el;

}

function changeColor(a_obj,a_bgColor,a_color) {
    for (i=0;i<a_obj.cells.length;i++){
        a_obj.cells[i].bgColor = a_bgColor ;
        a_obj.cells[i].style.color=a_color;
    }
}

/**
 * Highligth selected row by pass row directly
 */
var preEl;
var orgBgColor ;
var orgTxtColor ;

function highlightRow( row, bgColor, txtColor ) {
    if ( typeof(preEl) != 'undefined') {
        highlight(preEl, orgBgColor, orgTxtColor ) ;
    }

    preEl = row ;
    orgBgColor = row.bgColor ;
    orgTxtColor = row.style.color ;
    
    highlight( row, bgColor,txtColor ) ;
}

function highlight(row ,bgColor, txtColor) {
    if ( typeof(row.cells) != "undefined" ) {
        for (var idx = 0; idx < row.cells.length;idx++){
            row.cells[idx].bgColor = bgColor ;
            row.cells[idx].style.color = txtColor ;
        }
    }
}

/**
 * Highligth selected row by pass row directly
 */
function highlightColumn( column, table, bgColor, txtColor ) {
    if ( typeof(preEl) != 'undefined') {
        if ( orgCellIndex < table.rows[0].cells.length ) {
            for ( var rIdx = 0; rIdx < table.rows.length; rIdx++ ) {
                table.rows[rIdx].cells[orgCellIndex].bgColor = orgBgColor ;
                table.rows[rIdx].cells[orgCellIndex].style.color = orgTxtColor ;
            }
        }
    }
    
    preEl = column ;
    orgBgColor = column.bgColor ;
    orgTxtColor = column.style.color ;
    orgCellIndex = column.cellIndex ;
    for ( var rIdx = 0; rIdx < table.rows.length; rIdx++ ) {
        table.rows[rIdx].cells[orgCellIndex].bgColor = bgColor ;
        table.rows[rIdx].cells[orgCellIndex].style.color = txtColor ;
    }
}
 
/*  ##################### Cookie handler ##########################*/

/*******************************************************************
 ******************* Get Cookie *************************************
 *******************************************************************/
function getCookie(cookieName) {
  var docCookies = document.cookie;
  var startIndex = docCookies.indexOf(cookieName);

  if (startIndex == -1) return false;

  startIndex += cookieName.length + 1;
  var endIndex = docCookies.indexOf(";",startIndex);

  if (endIndex == -1)
      endIndex = docCookies.length;

  var cookieValue = docCookies.substring(startIndex, endIndex);

  return unescape(cookieValue);
}

/***********************************************************
 *  Set Cookie
 ***********************************************************/
function setCookie(key,value,expire,host) {
    cookieString = key+"="+escape(value)+";" ;
    cookieString += "expires="+expire+";" ;
    cookieString += "path=/" ;

    // If hostname ends with specific host
    if ( location.hostname.lastIndexOf(host) == location.hostname.length - host.length) {
        cookieString += "; domain=."+host; // Allow cookie on specific domain
    }

    document.cookie = cookieString ;

}

/*
 #############################################################
 #                                             Object utilities                                                                   #
 #############################################################
 */
/*************************************************************
 * Get object support any browser
 * Note : Now this function support for MSIE,Netscape 4, Netscape 6
 ************************************************************/
function getObject( elementId ) {
    var obj ;
      if( typeof(elementId) !='string')
          return elementId ;
      if(!document.all && document.getElementById)  //Netscape Navigator 6
          obj=document.getElementById(elementId) ;
      else if(document.all) //MS IE 5.5+
          //obj=document.all[elementId];
          obj=document.getElementById(elementId) ;
      else if ( document.layers) // Netscape Navigator 4
          obj = document.elementId ;
      else
          obj = null ;
      // In the future could be implement support other browser

      return obj;
}

/*
 * Get source element
 */
function getSrcElement( eventObj ) {
    var el ;
    if (IE) {
        //  Internet Explorer
        try {
          el = eventObj.srcElement ;
        } catch (ex) {alert( ex ) ;
        }

    } else if ( NN6)   {
        //  Netscape 6+
        el = eventObj.target ;
    }else {
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported")+"." ) ;
        }else{
            alert("Your browser doesn\'t supported.") ;
        }       
    }

    return el ;
}

/*
 * Get row element from source event
 */
function getRowSrcElement( eventObj ) {
    var el ;

    if ( eventObj ) {
        el = getSrcElement( eventObj ) ;

        if (IE) {
            el = el.parentElement ;
        } else if ( NN6) {
            el = el.parentNode ;
        }else {
            if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
                alert( TRANSLATION_DICT("Your browser doesn\'t supported")+"." ) ;
            }else{
                alert("Your browser doesn\'t supported.") ;
            } 
        }
    }

    return el ;
}

/****************************************************************/
/* A point representing a location in (x, y) coordinate space, specified in integer precision.*/
/****************************************************************/
function Point(iX, iY){
    this.x = iX;
    this.y = iY;
}

function getXY(aTag){
  var oTmp = aTag;
  var pt = new Point(0,0);

  if ( document.all)
  {
    do {
        pt.x += oTmp.offsetLeft;
        pt.y += oTmp.offsetTop;
        oTmp = oTmp.offsetParent;
    } while( oTmp!= null );
  }else if ( !document.all && document.getElementById ) {
      /*do {
            pt.x += oTmp.layerX;
            pt.y += oTmp.layerY;
            oTmp = ...;
        } while( oTmp!= null );
        */
       if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Your browser doesn\'t supported")+"." ) ;
        }else{
            alert("Your browser doesn\'t supported.") ;
        }
  }

  return pt;
}

/*************************************************/
/* Get screen available width and height */
/*************************************************/
function getScreenWidth() {
    var width  ;
    if (document.all || document.layers||document.getElementById)    {
       width = screen.availWidth;
    }
    else {
        width = 0 ;
    }
    return width;
}

function getScreenHeight() {
    var height  ;
    if (document.all || document.layers||document.getElementById)    {
       height = screen.availHeight;
    }
    else {
        height = 0 ;
    }
    return height ;
}


/***************************************************/
/*            Canceling event buble            */
/***************************************************/
function cancelBubble (e) {
    if (!e)
        var e = window.event;

    e.cancelBubble = true; // MSIE & NN6

    if (e.stopPropagation)
        e.stopPropagation(); // W3C
}

/****************************************************/
/* Get and set positions */
/****************************************************/
function getAbsLeft(o) {
     oLeft = o.offsetLeft ;
     while(o.offsetParent!=null) {
          oParent = o.offsetParent ;
          oLeft += oParent.offsetLeft ;
          oLeft -= oLeft ;
          o = oParent ;
     }
     return oLeft ;
}

function getAbsTop(o) {
     oTop = o.offsetTop ;
     while(o.offsetParent!=null) {
          oParent = o.offsetParent ;
          oTop += oParent.offsetTop ;
          o = oParent ;
     }
     return oTop ;
}

function setLeft(o,oLeft) {
     o.style.left = oLeft + "px" ;
}

function setTop(o,oTop) {
     o.style.top = oTop + "px" ;
}

function setPosition(o,oLeft,oTop) {
     setLeft(o,oLeft) ;
     setTop(o,oTop) ;
}

/*
 #############################################################
 #                                             Layer utilities                                                                     #
 #############################################################
 */
/*
 *  Cross-browser function to get an object's style object given its id
 */
function getStyleObject(objectId) {
    if(document.getElementById && document.getElementById(objectId)) {
        // W3C DOM
        return document.getElementById(objectId).style;
    } else if (document.all && document.all(objectId)) {
        // MSIE 4 DOM
        return document.all(objectId).style;
    } else if (document.layers && document.layers[objectId]) {
        // NN 4 DOM.. note: this won't find nested layers
        return document.layers[objectId];
    } else {
        return false;
    }
}

/*
 * Get a reference to the cross-browser style object and change its visibility
 */
function changeObjectVisibility(objectId, newVisibility) {
    var styleObject = getStyleObject(objectId);
    if(styleObject) {
        //styleObject.visibility = newVisibility;
        if ( newVisibility == 'visible')
        {
            styleObject.display = 'block' ;
        }else {
            styleObject.display = 'none' ;
        }
        return true;
    } else {
        // we couldn't find the object, so we can't change its visibility
        return false;
    }
}

/*
 * Get a reference to the cross-browser style object and display
 * Required : IFrame ID for container .
 */
window.currentlyIFrame  ;

function changeObjectDisplay(objectId, display) {
    var styleObject = getStyleObject(objectId);
    //var obj = getObject( objectId ) ;
    var obj = document.getElementById( objectId ) ;
    var iframeContainer ;
    var newPosX=newPosY=0 ;

    if ( window.currentlyIFrame  ) {
        iframeContainer = window.currentlyIFrame ;
    }else {
        var iframe = document.createElement( "IFRAME" ) ;
        iframe.style.display = "none" ;
        iframe.style.position = "absolute" ;
        iframeContainer = iframe ;
        document.body.appendChild(iframeContainer ) ;
        window.currentlyIFrame = iframeContainer ;
    }

    if( styleObject ) {
        if ( display.toUpperCase() == "BLOCK" ) {
            iframeContainer.style.display = styleObject.display = display ;
            //alert( "object : " + obj +", iframeContainer "+ iframeContainer );
            iframeContainer.style.left = obj.offsetLeft ;
            iframeContainer.style.top = obj.offsetTop ;
            iframeContainer.style.width = obj.offsetWidth ;
            iframeContainer.style.height = obj.offsetHeight ;
        }else {
            styleObject.display = iframeContainer.style.display = display ;
            window.currentlyIFrame = false ;
        }
        return true ;
    }
    else {
        // we couldn't find the object, so we can't change its visibility
        return false;
    }
}

/*
 * Get a reference to the cross-browser style object and move object
 */
function moveObject(objectId, newXCoordinate, newYCoordinate) {
    var styleObject = getStyleObject(objectId);
    if(styleObject) {
        styleObject.left = newXCoordinate+'px';
        styleObject.top = newYCoordinate+'px';
        return true;
    } else {
        // we couldn't find the object, so we can't very well move it
        return false;
    }
}

/* ****************************************************************
 * Change background color on mouse over
 * ****************************************************************/
var preObj;
var preBgColor ;
var preTxtColor ;
function mouseOverMenuItem(targetObjectId, eventObj) {
    if ( eventObj)    {
        eventObj.cancelBubble = true ;
    }
    var targetObj = getStyleObject( targetObjectId ) ;
    preObj = targetObjectId ;
    preBgColor = targetObj.backgroundColor ;
    preTxtColor = targetObj.color ;

    if ( targetObj )    {
        targetObj.backgroundColor = "#AFAEC4" ;
        targetObj.color = "#000000" ;
    }
}

/****************************************************************
 * Restore background color on mouse out
 ****************************************************************
 */
function mouseOutMenuItem( targetObjectId ,eventObj ) {
    if ( eventObj ) {
        eventObj.cancelBubble = true ;
    }
    var styleObject = getStyleObject( targetObjectId ) ;
    if ( styleObject && typeof (preObj) != 'undefined' )    {
        styleObject.backgroundColor = preBgColor ;
        styleObject.color = preTxtColor ;
        preObj = null ;
    }
}

/*********************************************************************
 * Check object for definition
 ********************************************************************
 */
function isDefine ( obj ) {
     return typeof(obj) != "undefined" ? true:false;
}

/********************************************************************
 * Clone object module
 ********************************************************************\
 */
function revertObject(objectSrc, objectTargetId ) {
    if ( objectSrc ) {
        var objNode = objectSrc.cloneNode(true) ;
        var obj = getObject( objectTargetId ) ;

        while(obj.hasChildNodes()) {
            obj.removeChild(obj.firstChild)  ;
        }

        while(objNode.hasChildNodes() ) {
            obj.appendChild(objNode.firstChild)  ;
        }
    }else {        
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT("Object is null")+"." ) ;
        }else{
            alert( "Object is null." ) ;
        }
    }
}

function cloneObject( objectId ) {
    var obj = getObject( objectId ) ;
    if (obj != null ) {
        return obj.cloneNode(true) ;
    } else return null ;
}

/*
 * set statusbar
 */
function setStatus( msg ) {
    var statusBar = parent.statusbarFrame.document.getElementById("stateMsg") ;
    var tmpMsg ;
    if ( msg.length >80 ) {
        tmpMsg = msg.substr(0, 80)+"..." ;
    }else {
        tmpMsg = msg ;
    }
    if(statusBar){
        statusBar.innerHTML = tmpMsg ; // set message
        statusBar.title = msg  // set tooltip for deme use default TBD future use custom tooltips
    }
 }

/*
 * Initialize toolbar
 */
function initToolBar( fileName ) {
    parent.toolbarFrame.location.href = "./"+fileName ;
    return ;
}

/****************************************************************
 * Not allow to click on checkbox and radio box
 ****************************************************************
 */
 function notAllowClick(eventObj) {
        eventObj.returnValue = false;
        return false;
}

/*****************************************************************
 * // Move row cross browser
 *****************************************************************
 */
function move_Row(table, iSource, iTarget ) {
    //alert( "iSrc : "+iSource+" , iTar : "+iTarget ) ;
    if ( IE ) {
        if ( iTarget > -1 && iTarget < table.rows.length )
            table.moveRow( iSource, iTarget ) ;
    } else if ( NN6 ) {
        var srcRows = new Array() ;
        for ( var ri = 0 ; ri < table.rows.length ; ri++ ) {
            srcRows [ri]  = table.rows[ri] ;
        }
        if ( iSource > 0 && iSource > iTarget && iSource < table.rows.length ) { // move up
           var tmp = iSource ;
           iSource = iTarget ;
           iTarget = tmp ;
            for ( var ri = 0 ; ri < table.rows.length ; ri++ ) {
                if ( ri != iSource ) {
                    table.tBodies[0].appendChild( srcRows[ri] ) ;
                }else {
                    var temp = srcRows[ri] ;
                    srcRows[ri] = srcRows[iTarget] ;
                    srcRows[iTarget] = temp ;
                    table.tBodies[0].appendChild( srcRows[ri] ) ;
                }
            }
        } else if ( iSource >= 0 && iTarget < table.rows.length) { // move down
            for ( var ri = 0 ; ri < table.rows.length ; ri++ ) {
                if ( ri != iSource ) {
                    table.tBodies[0].appendChild( srcRows[ri] ) ;
                }else {
                    var temp = srcRows[ri] ;
                    srcRows[ri] = srcRows[iTarget] ;
                    srcRows[iTarget] = temp ;
                    table.tBodies[0].appendChild( srcRows[ri] ) ;
                }
            }
        }
    }
}

function getParentElement(el, pTagName) {
    if (el == null) return null;
    else if (el.nodeType == 1 && el.tagName.toLowerCase() == pTagName.toLowerCase())    // Gecko bug, supposed to be uppercase
        return el;
    else
        return getParentElement(el.parentNode, pTagName);
}

/*****************************************************************
 * // Get child element by tag name
 *****************************************************************
 */
 function getChildElement ( el, cTagName ) {
     if ( el == null )
         return null ;
     else if ( el.nodeType == 1 ) {
        if (  el.tagName.toLowerCase() == cTagName.toLowerCase()) {
            return el ;
        } else {
            var t ;
            for ( var i=0; i < el.childNodes.length ; i++ ) {
                if (  el.childNodes[i].nodeType && isDefine( el.childNodes[i].tagName ) ) {
                    //alert( el.childNodes[i].tagName  ) ;
                    t = el.childNodes[i].firstChild ;

                    if( isDefine( t.tagName ) && t.tagName.toLowerCase() == cTagName.toLowerCase() ) {
                        break ;
                    }
                }
            }

            if( isDefine( t.tagName ) && t.tagName.toLowerCase() == cTagName.toLowerCase() ) {
                return t ;
            } else {
                return null ;
            }
        }
     }
 }

/**
*Retrieve XML document (reusable generic function);
* XML source must be from same domain as HTML file
* @param url the request url string (relative or complete) to an .xml file whose Content-Type is a valid XML type, such as text/xml
* @param param the request parameter string
* @param actionListerFunc the function for operate if state of request changed
* @param processAct the function for operate if state of request is successfully
* @param nAct the name of action ex. SAVED, DEL,...
* @param actToFunc the action that send to process in each editor
* @param messages the message to show if state of request is successfully
* @param resName the name of resource for each editor
* @param main if MainDir = "Y" this editor is in main directory (not sub directory)
* @param displayDialog status is "Y" for show result dialog or "N" don't show result dialog
*/
// Global request and XML document objects
var req ;

var setProcessAct ;
var actName ;
var actToFunc ;
var msgName ;
var resourceName ;
var mainDir = "Y" ;
var dlDisplay = "Y" ;

// Message Dialog width & height
var dlMsgWReq = 450;
var dlMsgHReq = 220;

function xmlHttpRequest( url, param , actionListerFunc, processAct, nAct, act2Func, messages, resName, main, displayDialog ) {
    setProcessAct = processAct ;
    actName = nAct ;
    actToFunc = act2Func ;
    msgName = messages ;
    resourceName = resName ;
    mainDir = main == "N" ? "../":"" ;
    dlDisplay = displayDialog ;
    
    if (param.indexOf("action=set") > -1 && param.indexOf("type=TreatmentList") < 0) {
        param = param+"&confirm=1"
    }

    // branch for native XMLHttpRequest object
    if ( window.XMLHttpRequest ) {
        req = new XMLHttpRequest() ;
        req.onreadystatechange = actionListerFunc ;
        req.open( "POST", url ) ;
        req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded') ;
        req.send( param ) ;
    // branch for IE/Windows ActiveX version
    } else if ( window.ActiveXObject ) {
        req = new ActiveXObject( "Microsoft.XMLHTTP" ) ;
        if ( req ) {
            req.onreadystatechange = actionListerFunc ;
            req.open( "POST", url ) ;
            req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded') ;
            req.send( param ) ;
        }
    }
}

// Parsing such a simple response should be no problem at all
function reqChangeActionListener() {
    var err = new Object() ;
    var MSG_NAME = new Array() ;
     
    if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
        MSG_NAME['INFO'] = TRANSLATION_DICT("Information") ; 
    }else{
        MSG_NAME['INFO'] = "Information" ; 
    }
    
    err.title = MSG_NAME['INFO'] ;
    
    // only if req shows "complete"
    if ( req.readyState == 4 ) {
        // only if "OK"
        if ( req.status == 200 ) {
            response = req.responseText ;            
            
            try {
                //response = response.replace(/None/gi, "NaN" ) ;
                response = transferEvalString(response) ;
                var obj = eval( response ) ;
                var errReport = generateMsgErrReport( obj, "desc" ) ; // sorting error code based on highest level error                
                if ( errReport['err_type'] != "ERROR" ) {
                    if ( errReport['err_type'] == "WARNING"  ) {
                        err.title = "Warning!" ;
                        err.message =  errReport['err_msg'] ;
                        showDialog(mainDir+"messageDialog.spy", true, err, dlMsgWReq, dlMsgHReq ) ;
                    } else {
                        
                        if ( actName == "SAVED" ) {                            
                            if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
                               err.message = obj[1]['label'].urldecode() + " " + msgName + " "+TRANSLATION_DICT("saved")+"." ;
                            }else{
                                err.message = obj[1]['label'].urldecode() + " " + msgName + " saved." ;
                            }
                        } else if ( actName == "DEL" ) {
                            
                            if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
                               err.message = resourceName + " " + msgName + " "+TRANSLATION_DICT("deleted")+"." ;
                            }else{
                                err.message = resourceName + " " + msgName + " deleted." ;
                            }
                        } else {
                            err.message = msgName ;
                        }
                        
                        if ( dlDisplay == "Y" ) {
                            showDialog(mainDir+"messageDialog.spy", true, err, dlMsgWReq, dlMsgHReq ) ;
                        }
                    }
                    setProcessAct ( actToFunc, obj ) ;
                } else {                    
                    err.message = err.message =  errReport['err_msg'] ;
                    showDialog(mainDir+"messageDialog.spy", true, err, dlMsgWReq, dlMsgHReq ) ;
                }
                
            }catch ( e ) {
                if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
                    err.message = TRANSLATION_DICT("Cannot parse response text to array object of javascript")+"."+e.message ;
                }else{
                    err.message = "Cannot parse response text to array object of javascript."+e.message ;
                }                
                showDialog(mainDir+"messageDialog.spy", true, err, dlMsgWReq, dlMsgHReq ) ;
            }
        } else {
            if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
                err.message = TRANSLATION_DICT("There was a problem retrieving the XML data")+" :\n"+req.statusText ;
            }else{
                err.message = "There was a problem retrieving the XML data :\n"+req.statusText ;
            }            
            showDialog(mainDir+"messageDialog.spy", true, err, dlMsgWReq, dlMsgHReq ) ;
        }
    }
}

/*****************************************************************
 * // translate string that not use in eval() function
 *****************************************************************
 */
 function transferEvalString( vStr ) {
    vStr = vStr.replace(/None/gi, "NaN" ) ;
    //vStr = vStr.replace(/\"/gi, "" ) ;
    vStr = vStr.replace(/\'\"/gi, "\"" ) ;
    vStr = vStr.replace(/\n/gi, "" ) ;
    vStr = vStr.replace(/\"\'/gi, "\"" ) ;
    vStr = vStr.replace(/\\/g, "\\\\" ) ;
    //vStr = vStr.replace(/|/g, "\"" ) ;
    return vStr;
 }

//***************************************************************
//Function setCorrentProjectOnHeader
//***************************************************************
 function setCorrentProjectOnHeader( theUser ) {
    var users                   = eval("["+transferEvalString(theUser)+"]");
    var objCurrentProjectName   = top.topframe.document.getElementById( "spCurrentProjectName" );
    objCurrentProjectName.innerHTML = "(" + users[0]['userName'] + ") Current Project:  "+users[0]['currentProject'];

    var objUserId          = top.topframe.document.getElementById( "hidUserID" ) ;
    var objUserName        = top.topframe.document.getElementById( "hidUserName" ) ;

    objUserId.value        = users[0]['userID'];
    objUserName.value      = users[0]['userName'];

 }
 /****************************************************************
 * function isInteger return true when object is number
 ****************************************************************
 * @param strData is object that want to check is number or not
 */
/*function isInteger(strData)
{
    for (var i = 0; i < strData.length; i++)
    {
        // Check that current character is number.
        var c = strData.charAt(i);
        if (((c < "0") || (c > "9"))) {
            return false;
        }
    }
    // All characters are numbers.
    return true;
}*/
 /****************************************************************
 * function isNumeric return true when object is numeric (float or int)
 * (note that above isInteger() function ONLY OK for positive integer!
 * *** TODO *** check above function
 ****************************************************************
 * @param strData is object that want to check is numeric or not
 */

function isNumeric(strData)
{
    var fReal = /^[\+\-]?\d*\.?\d*$/ ;
    if ( fReal.test(strData))
        return true ;
    else
        return false ;
}

function isNumericNoFloat(strData)
{
    var fReal = /^[\+\-]?\d*\d*$/ ;
    if ( fReal.test(strData))
        return true ;
    else
        return false ;
}

 /****************************************************************
 * function isStringLiteral return true when object is single or double
 * quote-delimited string literal
 * *** TODO *** only double-quote delimited seems to work
 * for parameterized function parameter...
 ****************************************************************
 * @param strData is object that want to check
 */

function isStringLiteral(strData) {
    //var reSingleQuoteLit = /^\'[^\'\"]*\'$/ ;
    var reDoubleQuoteLit = /^\"[^\'\"]*\"$/ ;
    if ( reDoubleQuoteLit.test(strData))
        return true ;
    //else if (reSingleQuoteLit.test(strData))
    //  return true;
    else
        return false ;
}


/****************************************************************
 * Function display confirm validate change message
 ****************************************************************
 */
function displayValidatePrompt()
{
    //event.cancleBubble=true;    
    if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
        event.returnValue = TRANSLATION_DICT("Changes will not be saved if you continue")+"?";
    }else{
        event.returnValue = "Changes will not be saved if you continue?";
    }
}

/****************************************************************
 * Function display help popup window
 ****************************************************************
 * @param strParam is parameter that set to PDF file
 */
function openHelp(strParam)
{
    var strSoruce = "doc/help.pdf" ;
    if(strParam != ""){
        strSoruce += strParam ;
     }

    window.open(strSoruce) ;
}

/**
 * Bring up the categories dialog
 */
var objInputTxt ;
function showCategoriesDialog ( txtTargetId ) { 
    var strProjectId = getObject("hidProjectID") ; 
    objInputTxt = getObject( txtTargetId ) ; 
    var url = "promptCategoriesDialog.spy";
    
    if(strProjectId){
        url += "?projectID="+strProjectId.value;  
    }
    
    var returnedVal = showDialog(url, true, window, 310, 405 ) ; 
    if( returnedVal ) { 
        objInputTxt.value = returnedVal.categories ; 
        setChanged( true ) ; 
    } 
} 

/**
 * Error handling for Editor
 * @param errorLists is the array object
 */
function errorHandling( errorLists ) {
    // Message Dialog width & height
    var dlMsgW = "450" ;
    var dlMsgH = "220" ;
    var err = new Object() ;
    if (!errorLists || typeof(errorLists) != 'object') {
        err.title = "Script Error" ;        
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            err.message =  TRANSLATION_DICT("Array object not found")+"." ;
        }else{
            err.message =  "Array object not found." ;
        }
        showDialog("messageDialog.spy", true, err, dlMsgW, dlMsgH ) ;
    } else if ( errorLists.length > 0 ) {
        err.title = "Status Report" ;
        err.message = "" ;
        for ( var idx in errorLists ) {
            err.message += getLevelDescription(errorLists[idx]['code'] ) + " : "+errorLists[idx]['code'] +"\n";
            //err.message +="Type : \""+errorLists[idx]['type'] +"\"\n";
            //err.message +="Action : \""+errorLists[idx]['action'] +"\"\n";
            err.message +="Message : \""+errorLists[idx]['message'] +"\"\n\n";
        }
        showDialog("messageDialog.spy", true, err, dlMsgW, dlMsgH ) ;
    }
}

/**
 * Error handling for dialog
 * @param errorLists is the array object
 */
function diglogErrorHandling( errorLists ) {
    // Message Dialog width & height
    var dlMsgW = "450" ;
    var dlMsgH = "220" ;
    var err = new Object() ;
    if (!errorLists || typeof(errorLists) != 'object') {
        err.title = "Script Error" ;
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            err.message =  TRANSLATION_DICT("Array object not found")+"." ;
        }else{
            err.message =  "Array object not found." ;
        }
        showDialog("../messageDialog.spy", true, err, dlMsgW, dlMsgH ) ;
    } else if ( errorLists.length > 0 ) {
        err.title = "Error Report" ;
        err.message = "" ;
        for ( var idx in errorLists ) {
            err.message += getLevelDescription(errorLists[idx]['code'] ) + " : "+errorLists[idx]['code'] +"\n";
            //err.message +="Type : \""+errorLists[idx]['type'] +"\"\n";
            //err.message +="Action : \""+errorLists[idx]['action'] +"\"\n";
            err.message +="Message : \""+errorLists[idx]['message'] +"\"\n\n";
        }
        showDialog("../messageDialog.spy", true, err, dlMsgW, dlMsgH ) ;
    }
}

/**
 * Get the error level
 * @param code is the code number
 */
function getLevelDescription( code ) {
/*    Information status code 0 - 9999
    Warning status code 10000 – 19999
    Validation Error code 20000 – 59999
    RunTime (Fatal) Error code 60000 – 79999
    System Error code 80000 - 89999
*/
    var info = "Information" ;
    var warning = "Warning" ;
    var validationErr = "Validation Error" ;
    var runtimeErr = "Fatal Error" ;
    var sysErr = "System Error" ;
    code = parseInt( code ) ;
    if ( code >=0 && code <= 9999 ) {
        return info ;
    } else if ( code >=10000 && code <= 19999  ) {
        return warning ;
    } else if ( code >=20000 && code <= 59999  ) {
        return validationErr ;
    } else if ( code >=60000 && code <= 79999  ) {
        return runtimeErr ;
    } else if ( code >=80000 && code <= 89999  ) {
        return sysErr ;
    } else {
        return "Code "+code+" out of range" ;
    }
}

/**
 * Handling when session expired
 * @param url is target location if session expired
 */
function sessionHandling ( url ) {
    var loc ; 
    if ( top.window.dialogArguments ) {
        if ( top.parent && !top.parent.closed ) {
            loc = encodeURIComponent( top.window.dialogArguments.top.location ) ;  // use encode for fix problem for "http://"
            if ( loc.indexOf( 'mainFramework.html') != -1 ) {
                if ( top.window.dialogArguments.setChanged) 
                    top.window.dialogArguments.setChanged( false ) ;
                else
                    top.window.dialogArguments.top.mainFrame.contentFrame.setChanged( false ) ;
                top.window.dialogArguments.top.location = url ;
                top.window.close() ;
            } else {
                top.window.dialogArguments.location = top.window.dialogArguments.location ;
                top.window.close() ;
            }
        } else {
            loc = encodeURIComponent( window.dialogArguments.top.location ) ;  // use encode for fix problem for "http://"
            if ( loc.indexOf( 'mainFramework.html') != -1 ) {
                window.dialogArguments.setChanged( false ) ;
                window.dialogArguments.top.location = url ;
                window.close() ;
            } else {
                //alert( "NOT IMPLEMENTED YET" ) ;
                top.window.dialogArguments.location = top.window.dialogArguments.location ;
                top.window.close() ;
            }
        }
    } else {        
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            alert( TRANSLATION_DICT('Wrong parent window referrence') ) ;
        }else{
            alert( 'Wrong parent window referrence' ) ;
        }
    }
}

/**
 * Sort function
 */
function ascending(a,b) {
    return a['code']-b['code'];
}

function descending(a,b) {
    return b['code']-a['code'];
}

/**
 * Generate error message content with sorting direction
 *@param errorLists is array object that need to produce
 *@param direction is sort method "asc/desc"
 *@return return an array object with error type and error message
 */
function generateMsgErrReport( errorLists, direction ) {
    // Message Dialog width & height
    var dlMsgW = "450" ;
    var dlMsgH = "220" ;
    var err = new Object() ;  
    
    if (!errorLists || typeof(errorLists) != 'object') {
        err.title = "Script Error" ;
        if (typeof TRANSLATION_DICT=="function"){//check this variable first, if it does not exist just use the old message---
            err.message =  TRANSLATION_DICT("Array object not found")+"." ;
        }else{
            err.message =  "Array object not found." ;
        }
        showDialog("messageDialog.spy", true, err, dlMsgW, dlMsgH ) ;
    } else {
        var isError = false ;
        var isWarning = false ;
        var msg = "" ;
        var err = new Array() ;
        var count = 0 ;
        for ( var idx = 0 ; idx<errorLists[0].length; idx++ ) { 
            var code = errorLists[0][idx]['statusCode'] ;
            var message = errorLists[0][idx]['message'].urldecode() ;
            var strInfoCode = validateActionCode( code ) ; 
            if (strInfoCode != "SUCCESS" && strInfoCode != "WARNING") {
                isError = true ;
                err[count] = new Array() ;
                err[count]['code'] = code ;
                err[count]['message'] = message ;
                count ++ ;
            } else if ( strInfoCode == "WARNING" ) {
                isWarning = true ;
                err[count] = new Array() ;
                err[count]['code'] = code ;
                err[count]['message'] = message ;
                count ++ ;
            }
        }
        
        if ( direction && direction == "desc" )
            err.sort(descending) ;
        else
            err.sort(ascending) ;
        
        for ( var idx = 0 ; idx<err.length; idx++ ) { 
            var code =err[idx]['code'] ;
            var message = err[idx]['message'] ;
            msg += "["+getLevelDescription(code)+"] : "+code+"\n" ;
            msg += "[Message] : "+message+"\n\n" ;
        }
        
        var arr = new Array() ;
        arr['err_type'] = isError?"ERROR":isWarning?"WARNING":"" ;
        arr['err_msg'] = msg ;
        
        return arr ;
    }
}

/**
 * Change format categories
 */
String.prototype.toCategories = function () {
    var str = this ;
    str = setFormatCategory(str) ;
    return str;
}



/*
* str is category data that user entry
* return strResult that is category format ['xx','yy']  
*/
function setFormatCategory( str ) {
    var strResult = "[";
    if (str == null || str == "") {
        return "[]";
    } else {
        var myArray = new Array ( ) ;
        myArray = str.split(",") ;
        for ( var i = 0 ; i < myArray.length ; i++ ) {
            if( i > 0 ) {
                strResult += "," ;
            }
            strResult += "'" + myArray[i].urlencode() + "'" ;
        }
        strResult += "]" ;
    }
    return strResult ;
}

//for flash display
function FlashInstalled()
{
	result = false;

	if (navigator.mimeTypes && navigator.mimeTypes["application/x-shockwave-flash"])
	{
		result = navigator.mimeTypes["application/x-shockwave-flash"].enabledPlugin;
	}
	else if (document.all && (navigator.appVersion.indexOf("Mac")==-1))
	{
		// IE Windows only -- check for ActiveX control, have to hide code in eval from Netscape (doesn't like try)
		eval ('try {var xObj = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");if (xObj)	result = true; xObj = null;	} catch (e)	{}');
	}
	return result;
}

function FlashWriteWhite(url,width,height,color)
{
	document.write('<object id="myFlash" classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"');
	document.write('  codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" ');
	document.write('  width="' + width + '%" height="' + height + '">');
	document.write(' <param name="movie" value="' + url + '"> <param name="allowScriptAccess" value="sameDomain"> <param name="quality" value="high"> <param name="scale" value="noscale"> <param name="bgcolor" value="' + color + '">  '); 
	document.write(' <embed src="' + url + '" quality="high" scale="noscale" bgcolor="' + color + '" allowScriptAccess="sameDomain" ');
	document.write(' swliveconnect=false width="' + width + '%" height="' + height + '"');
	document.write(' type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/shockwave/download/index.cgi?p1_prod_version=shockwaveflash">');
	document.write(' </embed></object>');
}

function FlashWriteFullScreen(url,width,height,color)
{
	document.write('<object id="myFlash" classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"');
	document.write('  codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,29,0" ');
	document.write('  width="' + width + '%" height="' + height + '%">');
	document.write(' <param name="movie" value="' + url + '"> <param name="allowScriptAccess" value="sameDomain"> <param name="quality" value="high"> <param name="scale" value="noscale"> <param name="bgcolor" value="' + color + '">  '); 
	document.write(' <embed src="' + url + '" quality="high" scale="noscale" bgcolor="' + color + '" allowScriptAccess="sameDomain" ');
	document.write(' swliveconnect=false width="' + width + '%" height="' + height + '%"');
	document.write(' type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/shockwave/download/index.cgi?p1_prod_version=shockwaveflash">');
	document.write(' </embed></object>');
}
/*
//function redirect to log on page
function timeOutcallback(productName, url)//("scroingplus", "url")
{
    if(productName == "scroingplus"){//alert(productName);alert(url);
        window.top.location=url + "/logon.spy?productName=ScoringPlus" ;
    }else{
        window.top.location=url + "/logon.spy";
    }
}*/

function calStrongPassword(strPass)
{
	var countComplex = 0;
    //re = /\W+$/; //special character
    re = /[^a-zA-Z0-9]/;
    if(re.test(strPass)) {
        countComplex +=1; 
    } 
    re = /[0-9]/;
    if(re.test(strPass)) {
        countComplex +=1; 
    } 
    re = /[a-z]/;
    if(re.test(strPass)) {
        countComplex +=1; 
    } 
    re = /[A-Z]/;
    if(re.test(strPass)) {
        countComplex +=1; 
    } 
    return countComplex;
}

