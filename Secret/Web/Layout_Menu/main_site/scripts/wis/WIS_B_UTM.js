var WIS_B_UTM;
(function (WIS_B_UTM) {
    var UTM_Url = "/FeedBack/UTM";
    function Get_UrlParameter(elem) {
        var sPageURL = decodeURIComponent(window.location.search.substring(1)), sURLVariables = sPageURL.split('&'), sParameterName, i;
        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] === elem) {
                return sParameterName[1] === undefined ? true : sParameterName[1];
            }
        }
    }
    WIS_B_UTM.Get_UrlParameter = Get_UrlParameter;
    function Get() {
        var wis_page = document.location.href;
        var utm_medium = Get_UrlParameter("utm_medium");
        var utm_source = Get_UrlParameter("utm_source");
        var utm_campaign = Get_UrlParameter("utm_campaign");
        var block = Get_UrlParameter("block");
        var utm_term = Get_UrlParameter("utm_term");
        var utm_content = Get_UrlParameter("utm_content");
        var position = Get_UrlParameter("position");
        var keyword = Get_UrlParameter("keyword");
        var elem = {
            wis_page: wis_page,
            utm_medium: utm_medium,
            utm_source: utm_source,
            utm_campaign: utm_campaign,
            block: block,
            utm_term: utm_term,
            utm_content: utm_content,
            position: position,
            keyword: keyword
        };
        return elem;
    }
    WIS_B_UTM.Get = Get;
    function Send() {
        var valid = false;
        var elem_UTM = Get();
        if (elem_UTM.utm_medium !== undefined) {
            valid = true;
        }
        if (elem_UTM.utm_source !== undefined) {
            valid = true;
        }
        if (elem_UTM.utm_campaign !== undefined) {
            valid = true;
        }
        if (elem_UTM.block !== undefined) {
            valid = true;
        }
        if (elem_UTM.utm_term !== undefined) {
            valid = true;
        }
        if (elem_UTM.utm_content !== undefined) {
            valid = true;
        }
        if (elem_UTM.position !== undefined) {
            valid = true;
        }
        if (elem_UTM.keyword !== undefined) {
            valid = true;
        }
        if (valid) {
            var data_json = {
                "wis_page": elem_UTM.wis_page,
                "utm_medium": elem_UTM.utm_medium,
                "utm_source": elem_UTM.utm_source,
                "utm_campaign": elem_UTM.utm_campaign,
                "block": elem_UTM.block,
                "utm_term": elem_UTM.utm_term,
                "utm_content": elem_UTM.utm_content,
                "position": elem_UTM.position,
                "keyword": elem_UTM.keyword
            };
            $.ajax({
                type: 'POST',
                cache: false,
                url: UTM_Url,
                data: data_json,
                success: function (data) {
                    if (!data.check) {
                        console.log("UTM - Ошибка отправки сообщения.");
                    }
                },
                error: function () {
                    console.log("UTM - Ошибка отправки сообщения, обратитесь к разрабочику или проверьте интернет-соединение.");
                }
            });
        }
    }
})(WIS_B_UTM || (WIS_B_UTM = {}));
//# sourceMappingURL=WIS_B_UTM.js.map