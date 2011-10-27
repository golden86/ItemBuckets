$.fn.outerHTML = function (s) {
    return (s)
			? this.before(s).remove()
			: $('<p>').append(this.eq(0).clone()).html();
}
$(function () {
    /**
    * the element
    */


    $(".msg_body").hide();
    //toggle the componenet with class msg_body
    $(".msg_head").click(function () {
        $(this).next(".msg_body").slideToggle(600);
    });

    $(".msg_body1").hide();
    //toggle the componenet with class msg_body
    $(".msg_head1").click(function () {
        $(this).next(".msg_body1").slideToggle(600);
    });

    $(".msg_body2").hide();
    //toggle the componenet with class msg_body
    $(".msg_head2").click(function () {
        $(this).next(".msg_body2").slideToggle(600);
    });

    $(".msg_body3").hide();
    //toggle the componenet with class msg_body
    $(".msg_head3").click(function () {
        $(this).next(".msg_body3").slideToggle(600);
    });

    $(".msg_body4").hide();
    //toggle the componenet with class msg_body
    $(".msg_head4").click(function () {
        $(this).next(".msg_body4").slideToggle(600);
    });

    var $ui = $('#ui_element');
    $ui.find('.addition').focus();

    $(".command").click(function () {

        $ui.find('.addition').val($ui.find('.addition').val() + $(this).html());
        $ui.find('.addition').focus();
    });

    $(".topsearch").click(function () {

        $ui.find('.addition').val($ui.find('.addition').val() + $(this).html());
        $ui.find('.addition').focus();
    });


    $ui.find(".addition").live('keydown', function (e) {
        var keyCode = e.keyCode || e.which;
        if (keyCode == 13) {
            var filesearches = $ui.find('.boxme li .file');
            var searchsearches = $ui.find('.boxme li .search');
            var templatesearches = $ui.find('.boxme li .template');
            var tagsearches = $ui.find('.boxme li .tag');
            var startsearches = $ui.find('.boxme li .start');
            var endsearches = $ui.find('.boxme li .end');
            var searchArray = new Array();

            $.each(searchsearches, function () {
                searchArray.push({ 'type': 'search', 'value': $(this).text() });
            });
            $.each(filesearches, function () {
                searchArray.push({ 'type': 'file', 'value': $(this).text() });
            });
            $.each(templatesearches, function () {
                searchArray.push({ 'type': 'template', 'value': $(this).text() });
            });
            $.each(tagsearches, function () {
                searchArray.push({ 'type': 'tag', 'value': $(this).text() });
            });
            $.each(startsearches, function () {
                searchArray.push({ 'type': 'start', 'value': $(this).text() });
            });
            $.each(endsearches, function () {
                searchArray.push({ 'type': 'end', 'value': $(this).text() });
            });


            //                    $.each(searchArray, function (i, v) {
            //                        alert(v.type);
            //                        alert(v.value);
            //                    });

            $.post("/Results.aspx").success(function (data) { alert(data); }).complete(function (data) { alert(data); });
        }
    });


    $(".sb_search").click(function () {
        var filesearches = $ui.find('.boxme li .file');
        var searchsearches = $ui.find('.boxme li .search');
        var templatesearches = $ui.find('.boxme li .template');
        var tagsearches = $ui.find('.boxme li .tag');
        var startsearches = $ui.find('.boxme li .start');
        var endsearches = $ui.find('.boxme li .end');
        var searchArray = new Array();

        $.each(searchsearches, function () {
            searchArray.push({ 'type': 'search', 'value': $(this).text() });
        });
        $.each(filesearches, function () {
            searchArray.push({ 'type': 'file', 'value': $(this).text() });
        });
        $.each(templatesearches, function () {
            searchArray.push({ 'type': 'template', 'value': $(this).text() });
        });
        $.each(tagsearches, function () {
            searchArray.push({ 'type': 'tag', 'value': $(this).text() });
        });
        $.each(startsearches, function () {
            searchArray.push({ 'type': 'start', 'value': $(this).text() });
        });
        $.each(endsearches, function () {
            searchArray.push({ 'type': 'end', 'value': $(this).text() });
        });


        //                    $.each(searchArray, function (i, v) {
        //                        alert(v.type);
        //                        alert(v.value);
        //                    });

        $.post("/Results.aspx", { 'search': searchArray }).success(function () { alert("second success"); });
        
    });

    /**
    * on focus and on click display the dropdown, 
    * and change the arrow image
    */
    $ui.find('.addition').bind('focus click', function () {
        $ui.find('.sb_down')
					   .addClass('sb_up')
					   .removeClass('sb_down')
					   .andSelf()
					   .find('.sb_dropdown')
					   .show();

    });

    $ui.find('.addition').bind('keypress', function () {
        $ui.find('.sb_down')
					   .addClass('sb_up')
					   .removeClass('sb_down')
					   .andSelf()
					   .find('.sb_dropdown')
					   .show();
    });


    function yourfunction(item) {

        item.hide();
    }


    $ui.find(".addition").live('keydown', function (e) {
        var keyCode = e.keyCode || e.which;

        if (keyCode == 9) {
            e.preventDefault();
            // call custom function here

            if ($ui.find(".addition").val().indexOf('filetype:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('filetype:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/file.png\') no-repeat center center;padding: 0px 11px;"></span><p class="file">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');
                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });





            }

            if ($ui.find(".addition").val().indexOf('text:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('text:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/search.gif\') no-repeat center center;padding: 0px 11px;"></span><p class="search">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });


            }


            if ($ui.find(".addition").val().indexOf('template:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('template:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/template.png\') no-repeat center center;padding: 0px 11px;"></span><p class="template">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });


            }

            if ($ui.find(".addition").val().indexOf('tag:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('tag:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/tag.png\') no-repeat center center;padding: 0px 11px;"></span><p class="tag">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });


            }
        }
    });

    $ui.find(".addition").live('keyup', function (event) {
        if (event.which == 39 || event.which == 9 || event.keycode == 9) {

            if ($ui.find(".addition").val().indexOf('filetype:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('filetype:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/file.png\') no-repeat center center;padding: 0px 11px;"></span><p class="file">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');
                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });





            }

            if ($ui.find(".addition").val().indexOf('text:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('text:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/search.gif\') no-repeat center center;padding: 0px 11px;"></span><p class="search">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });


            }


            if ($ui.find(".addition").val().indexOf('template:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('template:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/template.png\') no-repeat center center;padding: 0px 11px;"></span><p class="template">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });


            }

            if ($ui.find(".addition").val().indexOf('tag:') > -1) {

                var substr = $ui.find(".addition").val().split(':')[1];
                var newHTML = $ui.find(".addition").val().replace('tag:' + substr, '');

                $ui.find(".addition").val(newHTML);

                $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/tag.png\') no-repeat center center;padding: 0px 11px;"></span><p class="tag">' + substr + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                $('.remove').live('click', function () {
                    $(this).parents("li:first").remove();
                });


            }


        }

        if (event.which == 59) {

            if ($ui.find(".addition").val().indexOf('start:') > -1) {

                $ui.find(".addition").datepicker({

                    onClose: function (dateText, inst) {
                        var newHTML = $ui.find(".addition").val().replace('start:', '').replace(dateText, '');
                        $ui.find(".addition").val(newHTML);

                        $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/startdate.png\') no-repeat center center;padding: 0px 11px;"></span><p class="start">' + dateText + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                        $('.remove').live('click', function () {
                            $(this).parents("li:first").remove();
                        });
                        $ui.find(".addition").datepicker("destroy");

                    }
                });
                $ui.find(".addition").datepicker("show");
                $(".addition").bind("datepickercreate", function (event, ui) {
                    $(this).show();
                });



            }


            if ($ui.find(".addition").val().indexOf('end:') > -1) {

                $ui.find(".addition").datepicker({

                    onClose: function (dateText, inst) {
                        var newHTML = $ui.find(".addition").val().replace('end:', '').replace(dateText, '');
                        $ui.find(".addition").val(newHTML);

                        $ui.find(".boxme").prepend('<li class="token-input-token-facebook"><span style="background: url(\'/images/date.png\') no-repeat center center;padding: 0px 11px;"></span><p class="end">' + dateText + '</p><span class="token-input-delete-token-facebook remove">×</span></li>');

                        $('.remove').live('click', function () {
                            $(this).parents("li:first").remove();
                        });
                        $ui.find(".addition").datepicker("destroy");

                    }
                });
                $ui.find(".addition").datepicker("show");
                $(".addition").bind("datepickercreate", function (event, ui) {
                    $(this).show();
                });



            }

        }

    });

    /**
    * on mouse leave hide the dropdown, 
    * and change the arrow image
    */
    $ui.bind('mouseleave', function () {
        $ui.find('.sb_up')
					   .addClass('sb_down')
					   .removeClass('sb_up')
					   .andSelf()
					   .find('.sb_dropdown')
					   .hide();
    });

    /**
    * selecting all checkboxes
    */
    $ui.find('.sb_dropdown').find('label[for="all"]').prev().bind('click', function () {
        $(this).parent().siblings().find(':checkbox').attr('checked', this.checked).attr('disabled', this.checked);
    });
});