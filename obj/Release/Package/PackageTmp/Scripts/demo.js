var i18n;
var session_id;
var enviroment;
var locale = 'en_US';
var api;
var urlParams = {};
var rp_modal;
var rp_embedded;

window.onload = function () {
    initUrlParams();
    initEnviroment().then(function () {
        initLocale();
     
        initSession();
        initHandlers();
        rp_modal = new Reepay.ModalCheckout();

        setTimeout(function () {
            openWindow();
        }, 2000); 

        //document.getElementById('content').style.display = "block";
    });

    function initHandlers() {
        //document.getElementById("console-toggle").addEventListener("click", toggleConsole);
        //document.getElementById("rp-console-btn").addEventListener("click", toggleConsole);

        //document.getElementById("settings-toggle").addEventListener("click", toggleSettings);
        //document.getElementById("openSettingsButton").addEventListener("click", toggleSettings);

        //document.getElementById("rp-btn-embedded").addEventListener("click", showEmbedded);
        //document.getElementById("rp-btn-modal").addEventListener("click", openModal);
        //document.getElementById("rp-btn-window").addEventListener("click", openWindow);
        //document.getElementById("rp-btn-window1").addEventListener("click", openWindow);

        //document.getElementById("language-select").addEventListener("change", changeLanguage);
    }
 


    function changeLanguage(event) {
        var e = document.getElementById("language-select");
        var lang = e.options[e.selectedIndex].value;
        loadI18n(lang, true);
    }

    //#region Console Movement
    var mousePosition;
    var offset = [30, 30];
    var isDown = false;
    var consoleWindow = document.getElementById('consoleWindow');

    consoleWindow.addEventListener('mousedown', function (e) {
        isDown = true;
        offset = [
            consoleWindow.offsetLeft - e.clientX,
            consoleWindow.offsetTop - e.clientY
        ];
    }, true);

    document.addEventListener('mouseup', function () {
        isDown = false;
    }, true);

    document.addEventListener('mousemove', function (event) {
        event.preventDefault();
        if (isDown) {
            mousePosition = {
                x: event.clientX,
                y: event.clientY
            };
            consoleWindow.style.left = (mousePosition.x + offset[0]) + 'px';
            consoleWindow.style.top = (mousePosition.y + offset[1]) + 'px';
        }
    }, true);
    //#endregion

};

function initUrlParams() {
    var urlQueries = window.location.href.split('?')[1];
    if (urlQueries) {
        urlQueries = urlQueries.split('&');
    }
    if (urlQueries) {
        urlQueries.forEach(function (query) {
            var param = query.split('=');
            urlParams[param[0]] = param[1] ? param[1] : true;
        });
        if (urlParams['cancel'])
            addConsoleMessage('Window', 'Cancel', 'warn');
        if (urlParams['accept'])
            addConsoleMessage('Window', 'Accept (invoice_id: ' + urlParams['invoice'] + ')', 'success');
        if (urlParams['console'])
            toggleConsole();
    }
}

function Saveordervalue() {
    var FirstName = $("#txtfnamep").val() || '';
    var LastName = $("#txtlnamep").val() || '';
    var MobileNo = $("#txtmobilep").val() || '';
    var EmailId = $("#txtemailp").val() || '';

    var isvalid = true;

    if (FirstName == '' || FirstName == undefined || FirstName == null) {
        $(".firstnamepic").addClass("backgroundcolorred");
        isvalid = false;
    }

    if (LastName == '' || LastName == undefined || LastName == null) {
        $(".lastnamepic").addClass("backgroundcolorred");
        isvalid = false;
    }

    if (MobileNo == '' || MobileNo == undefined || MobileNo == null) {
        $(".mobilenopic").addClass("backgroundcolorred");
        isvalid = false;
    }

    if (!isvalid) {
        return true;
    } ddldelvtime
    var myObj = { FirstName: $("#txtfnamep").val(), LastName: $("#txtlnamep").val(), MobileNo: $("#txtmobilep").val(), EmailId: $("#txtemailp").val(), Time: $("#ddldelvtime").val() };
    $.ajax({
        type: "POST",
        url: '/ShoppingCart/Savepickup',
        data:
        {
            data: JSON.stringify(myObj),
        },
        async: false,
        success: function (result) {


            return false;            
        },
        error: function () {
            alert("error");
        }
    });
}

function initEnviroment() {
    enviroment ='prod';
    //enviroment = urlParams['env'] ? urlParams['env'] : 'prod';
    return new Promise(function (resolve, reject) {
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                enviroments = this.responseXML;
                var nodes = enviroments.getElementsByTagName("enviroment");
                for (i = 0; i < nodes.length; i++) {
                    if (nodes[i].getAttribute('env') === enviroment) {                        
                        var checkout_js = nodes[i].getElementsByTagName('checkout-js')[0].childNodes[0].nodeValue;
                        var script = document.createElement('script');
                        script.src = checkout_js;
                        api = nodes[i].getElementsByTagName('checkout-api')[0].childNodes[0].nodeValue;
                        document.head.appendChild(script);
                        script.onload = function () {
                            resolve();
                        };
                        script.onerror = function () {
                            addConsoleMessage('No checkout script', script.src, 'error');
                        }
                    }
                }
                addConsoleMessage('Enviroment', enviroment, 'info');
            }
        };
        xhttp.open("GET", "../Scripts/env.xml", true);
        xhttp.send();
    });
}

function initSession() {    
    //session_id = urlParams['session'] ? urlParams['session'] : urlParams['id'];
    //if (session_id) {
    //    addConsoleMessage('Session', session_id, 'info');
    //    document.getElementById('openSettingsButton').style.display = "none";
    //}
    //else {
        getSession();
    //}
}

function initLocale() {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            i18n = this.responseXML;
            loadI18n(locale, false);
        }
    };
    xhttp.open("GET", "../Scripts/i18n.xml", true);
    xhttp.send();
}

function getUrlWithParams(type) {    
    var location = window.location.href.split('?')[0];
    var location = location.replace("/payment_page", "");

    if (type == "sucsses") {
        location += '/Payment_confirmation?';
    }
    else {
        location += '/Payment_Cancel?';
    }

    location += urlParams['env'] ? ('&' + 'env=' + urlParams['env']) : '';
    location += urlParams['console'] ? '&' + 'console' : '';
    return location;
}

function getSession() {    
    var amounts = $('#totalamount').text().trim().split(/\.(?=[^\.]+$)/);
    var amountsadd = amounts[0].toString() + amounts[1].toString();

    var handler = "checkout_test_order_" + new Date().getTime().toString();
    var params = {
        configuration: "default",
        locale: locale,
        recurring: true,
        accept_url: getUrlWithParams('sucsses') + '&accept',
        cancel_url: getUrlWithParams('cancel') + '&cancel',
        //publicKey: "priv_4fa87ecdf42139b40ec6bbf84f914cf4:",
        order: {
            handle: handler,
            amount: parseInt(amountsadd),
            currency: "DKK",
            customer_handle: null,
            customer: { handle: "co_demo_user" },
            ordertext: null,
            settle: false,
            logo: "http://reepay.com/images/logo.svg"
            //order_lines: [
            //    { ordertext: "Product 1", amount: "50", vat: 0, quantity: "3", amount_incl_vat: false },
            //    { ordertext: "Product 2", amount: "300", vat: 0, quantity: 1, amount_incl_vat: false }
            //]
        }
    }
   // var url = api + "/v1/session/demo_charge";
    var url = api + "/v1/session/charge";

    //var url = "https://checkout-api.reepay.com/v1/session/charge"
    var json = JSON.stringify(params);
    var xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);

    xhr.setRequestHeader("accept", "application/json");
    xhr.setRequestHeader("content-type", "application/json");
    //xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
    //xhr.setRequestHeader("u", "priv_4fa87ecdf42139b40ec6bbf84f914cf4:");
    //xhr.setRequestHeader("Content-Type", "application/json");
    xhr.setRequestHeader("authorization", "Basic cHJpdl84NTMwYjJhZWE5NzU0NzhlMThlMGNhODgyMzljZjcwNDo=");
    //xhr.setRequestHeader("authorization", "Basic cHJpdl80ZmE4N2VjZGY0MjEzOWI0MGVjNmJiZjg0ZjkxNGNmNDo=");
    xhr.onload = function () {        
        var response = JSON.parse(xhr.responseText);
        if (xhr.readyState == 4 && xhr.status == "200") {
            session_id = response.id;
            addConsoleMessage('Session', session_id, 'info');
        } else {
            addConsoleMessage('Session', "Error: " + response, 'error');
        }
    }
    xhr.send(json);
}

function loadI18n(language, reload) {
    locale = language;
    var elements_to_translate = "window";//document.querySelectorAll('[translate]');
    var nodes = i18n.getElementsByTagName("locale");
    for (i = 0; i < nodes.length; i++) {
        if (nodes[i].getAttribute('locale') === language) {
            for (var j = 0; j < elements_to_translate.length; j++) {
                var elem = elements_to_translate[j];
                var translation_value = elem.getAttribute('translate');
                elem.innerHTML = nodes[i].getElementsByTagName(translation_value)[0].childNodes[0].nodeValue;
            }
        }
    }
    var settingsWindow = document.getElementById('settingsOverlay');
    settingsWindow.classList.remove('enabled');
    addConsoleMessage('Locale', language, 'info');
    if (reload)
        getSession(); translate
}

function addConsoleMessage(origin, message, type) {
    //var console = document.getElementById('console');
    var d = new Date();
    var datestring = "[" + d.getDate() + "-" + (d.getMonth() + 1) + "-" + d.getFullYear() + " " + d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds() + "]";
    var p = document.createElement('p');
    p.innerText = origin + ": " + message;
    p.insertAdjacentHTML('afterbegin', '<span>' + datestring + '</span>');
    p.classList.add(type);
    //console.appendChild(p);
    p.scrollIntoView();
}

function toggleSettings() {
    var settingsWindow = document.getElementById('settingsOverlay');
    settingsWindow.classList.toggle('enabled');
}

function toggleConsole(e) {
    if (e)
        e.stopPropagation();
    var consoleWindow = document.getElementById('consoleWindow');
    consoleWindow.classList.toggle('enabled');
}

function showEmbedded() {
    addConsoleMessage('Embedded', 'Opened', 'info');
    document.getElementById("rp_container").className = "open";
    // rp_embedded = new Reepay.EmbeddedCheckout(session_id, {html_element: 'rp_container', showReceipt: false});
    rp_embedded = new Reepay.EmbeddedCheckout(session_id, { html_element: 'rp_container' });
    rp_embedded.addEventHandler(Reepay.Event.Accept, function (data) { addConsoleMessage('Embedded', 'Accept ' + createMessageFromData(data), 'success') });
    rp_embedded.addEventHandler(Reepay.Event.Error, function (data) { addConsoleMessage('Embedded', data.error + ' ' + createMessageFromData(data), 'error') });
    rp_embedded.addEventHandler(Reepay.Event.Cancel, function (data) { addConsoleMessage('Embedded', 'Cancel ' + createMessageFromData(data), 'warn') });
    setTimeout(function () {
        document.getElementById("rp_container").scrollIntoView({ behavior: 'smooth' });
    }, 300);

}

function openWindow() {    
    addConsoleMessage('Window', 'Opened', 'info');
    new Reepay.WindowCheckout(session_id);
}

function openModal() {
    if (rp_embedded) {
        rp_embedded.destroy();
        document.getElementById("rp_container").classList.remove("open");
    }
    addConsoleMessage('Modal', 'Opened', 'info');
    // rp_modal.show(session_id, {showReceipt: false});
    rp_modal.show(session_id);
    rp_modal.addEventHandler(Reepay.Event.Accept, function (data) { addConsoleMessage('Modal', 'Accept ' + createMessageFromData(data), 'success') });
    rp_modal.addEventHandler(Reepay.Event.Error, function (data) { addConsoleMessage('Modal', data.error + ' ' + createMessageFromData(data), 'error') });
    rp_modal.addEventHandler(Reepay.Event.Cancel, function (data) { addConsoleMessage('Modal', 'Cancel ' + createMessageFromData(data), 'warn'); });
    rp_modal.addEventHandler(Reepay.Event.Close, function (data) { addConsoleMessage('Modal', 'Close ' + createMessageFromData(data), 'info') });
}

function openSubscriptionEmbedded() {
    addConsoleMessage('Embedded', 'Opened', 'info');
    document.getElementById("rp_container").className = "open";
    new Reepay.EmbeddedSubscription(session_id);
}

function openSubscriptionModal() {
    new Reepay.ModalSubscription(session_id);
}

function openSubscriptionWindow() {
    new Reepay.WindowSubscription(session_id);
}

function createMessageFromData(data) {
    if (!data) {
        return '';
    }

    var str = '(';
    str += data.invoice ? ' invoice: ' + data.invoice : '';
    str += data.customer ? ' customer: ' + data.customer : '';
    str += data.payment_method ? ' payment_method: ' + data.payment_method : '';
    str += ')';
    return str;
}