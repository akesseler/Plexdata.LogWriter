
function setCopyright(selector) {

    if (selector) {

        let date = new Date();
        let year = date.getFullYear();
        let text = 'Copyright&nbsp;&copy;&nbsp;' + year + '&nbsp;<a href="http://www.plexdata.de" target="_blank">plexdata.de</a>';

        $(makeSelector(selector)).html(text);
    }
}

function makeSelector(value) {

    const hash = '#';

    if (value) {

        let index = value.indexOf(hash);

        if (index < 0) {
            return hash + value;
        }
    }

    return value;
}

function selectCaller(caller) {

    if (caller) {

        try {

            let parent = $(makeSelector('navigation'));

            if (parent) {

                $(parent).children('a').each(function () {
                    $(this).removeClass('selected');
                });

                if (typeof caller === "string") {
                    caller = makeSelector(caller);
                }

                $(caller).addClass('selected');
            }
        }
        catch (error) {
            console.log(error);
        }
    }

    return false;
}

function showContent(caller, chapter, section) {

    selectCaller(caller);

    if (chapter) {

        try {

            let target = makeSelector('content');

            $(target).load(chapter);

            $(target).ready(function () {

                try {

                    // As first bring current target into view.
                    $(target).scrollTop(0);

                    let selector = makeSelector(section);

                    if (selector) {

                        // Prevent execution in the hope that 500 ms is enough wait time to delay scrolling.
                        let waittime = $(selector).offset() !== undefined ? 0 : 500;

                        setTimeout(function () {

                            try {

                                let offset = $(selector).offset().top - $(target).offset().top;

                                $(target).scrollTop(offset);
                            }
                            catch (error) {
                                console.log(error);
                            }
                        }, waittime);
                    }
                }
                catch (error) {
                    console.log(error);
                }
            });
        }
        catch (error) {
            console.log(error);
        }
    }

    return false;
}

function onScrollContent() {

    let content = $(makeSelector('content'));
    let button = $('.jump-top-button');

    if (content && button) {

        if ($(content).scrollTop() > 20) {

            $(button).show();

        } else {

            $(button).hide();
        }
    }

    return false;
}

function onJumpTop() {

    let content = $(makeSelector('content'));

    if (content) {
        $(content).scrollTop(0);
    }

    return false;
}

function copyToClipboard(selector) {

    try {

        var $temp = $("<textarea>");
        $("body").append($temp);
        $temp.val($(makeSelector(selector)).text()).select();
        document.execCommand("copy");
        $temp.remove();
    }
    catch (error) {
        console.log(error);
    }

    return false;
}
