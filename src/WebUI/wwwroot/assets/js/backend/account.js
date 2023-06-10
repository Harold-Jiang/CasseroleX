define(['jquery', 'bootstrap', 'backend','form'], function ($, undefined, Backend,Form) {
    var Controller = {
        login: function () {
            var lastlogin = localStorage.getItem("lastlogin");
            if (lastlogin) {
                lastlogin = JSON.parse(lastlogin);
                $("#profile-img").attr("src", Backend.api.cdnurl(lastlogin.avatar));
                $("#profile-name").val(lastlogin.username);
            }

            //让错误提示框居中
            Fast.config.toastr.positionClass = "toast-top-center";

            //本地验证未通过时提示
            $("#login-form").data("validator-options", {
                invalid: function (form, errors) {
                    $.each(errors, function (i, j) {
                        Toastr.error(j);
                    });
                },
                target: '#errtips'
            });

            //为表单绑定事件
            Form.api.bindevent($("#login-form"), function (data) {
                localStorage.setItem("lastlogin", JSON.stringify({
                    id: data.id,
                    username: data.username,
                    avatar: data.avatar
                }));
                location.href = Backend.api.fixurl(data.url);
            }, function (data) {
                $("input[name=captcha]").next(".input-group-addon").find("img").trigger("click");
            });
        },


        //login: function () {
             
        //    var lastlogin = localStorage.getItem("lastlogin");
        //    if (lastlogin) {
        //        lastlogin = JSON.parse(lastlogin);
        //        $("#profile-img").attr("src", Backend.api.cdnurl(lastlogin.avatar));
        //        $("#profile-name").val(lastlogin.username);
        //    }

        //    if ($("#captchaImg").length > 0) {
        //        //验证码
        //        Controller.toggleCode();

        //        //本地验证未通过时提示
        //        $("#captchaImg").click(function () {
        //            Controller.toggleCode();
        //        });
        //    } 

        //    //让错误提示框居中
        //    Fast.config.toastr.positionClass = "toast-top-center";

        //    //本地验证未通过时提示
        //    $("#login-form").data("validator-options", {
        //        invalid: function (form, errors) { 
        //            $.each(errors, function (i, j) {
        //                Toastr.error(j);
        //            });
        //        },
        //        target: '#errtips'
        //    });

        //    //为表单绑定事件
        //    Form.api.bindevent($("#login-form"), function (data) {
        //        localStorage.setItem("lastlogin", JSON.stringify({
        //            id: data.id,
        //            username: data.username,
        //            avatar: data.avatar
        //        }));
        //        location.href = Backend.api.fixurl(data.url);
        //    }, function (data) {
        //        // $("#captchaImg").trigger("click");
        //        Controller.toggleCode();
        //    });
        //},
        //toggleCode: function () {
        //    //验证码
        //    Fast.api.ajax({
        //        url: 'account/getcode',
        //        loading: false,
        //        data: {}
        //    }, function (ret, res) {
        //        Layer.alert("验证码获取错误", { title: "警告", icon: 0 });
        //    }, function (ret, res) {
        //        $("input[name=codekey]").val(res.key);
        //        $("#captchaImg").attr("src", res.image);
        //        return false;
        //    });
        //}
    };

    return Controller;
});
