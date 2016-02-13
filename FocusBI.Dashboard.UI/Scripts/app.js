var App = {
    // calls api method and returns json
    GetApiData: function (url, parameters) {
        url = currentDomain + url;
        //alert(url);
        //alert('Url: ' + url + ' parameters: ' + parameters);
        var json;
        $.ajax({
            url: url,
            async: false,
            data: parameters,
            cache: false,
            context: $(this),
            success: function (data, status, jqXHR) {
                if (data.exception) {
                    $('.blockUI').unblock();
                    App.MsgDialog('type-warning', data.exception);
                }
                else {
                    json = data;
                }
            },
            error: function (jqXHR, exception) {
                //jscript error
                $('.blockUI').unblock();
                App.MsgDialog('type-warning', jqXHR.responseText);
            },
        });
        return json;
    },
    // Bootstrap dialog 
    MsgDialog: function (type, text) {
        BootstrapDialog.show({
            message: text,
            type: type,
            buttons: [
                {
                    label: 'Close',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    
    FormatDate: function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ':' + seconds + ' '  + ampm;
        return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + "  " + strTime;
    }

}




