var left_menu = $('.left_menu');
var bookmark = $('.menu_bookmark');
var menu_area = $('.left_menu .menu_area');
var tag_color = $('.left_menu .menu_area .tag_color');
var sub_list = $('.left_menu .menu_area .sub_list');
var sub_list_li = $('.left_menu .menu_area .sub_list ul li');

$(document).ready(function () {

    // ===== [Initialization] =====
    // Collapse the menu if the width of window is smaller than 1400 pixels
    if ($(window).width() > 1400) {
        CollapseMenu(true);
    }
    else {
        CollapseMenu(false);
    }

    // ===== [Event Handlers of Elements] =====
    // Window size changing event
    $(window).resize(function () {
        if ($(window).width() > 1400) {
            CollapseMenu(true);
        }
        else {
            CollapseMenu(false);
        }

        left_menu.css('height', $(this).height() + $(this).scrollTop());
    });

    // Page scrolling event
    $(window).scroll(function () {
        left_menu.css('height', $(this).height() + $(this).scrollTop());
    });

    // Each tag of menu list clicking event
    tag_color.click(function () {
        $(this).parent().siblings('.menu_area').children('.tag_color').removeClass('active');
        $(this).addClass('active');
        $(this).next('.sub_list').slideToggle("slow");
    });

});

// ===== [Private] Function =====

// Collapse the menu depending on the width of window
function CollapseMenu(flag) {
    // flag: True for larger width window (> 1400 px); False for smaller width window
    if (flag) {
        left_menu.css({ 'left': '0%', 'width': '13%' });
        bookmark.css('display', 'none');

        // Unfold the left menu
        left_menu.mouseleave(function () {
            left_menu.stop(true, false).animate({ 'left': '0%' }, 500);
        });
    }
    else {
        left_menu.css({ 'left': '-195px', 'width': '200px' });
        bookmark.css('display', 'block');
        let FirstTouch = true;

        // Unfold the left menu by touching the bookmark
        bookmark.mousemove(function () {
            if (FirstTouch) {
                left_menu.stop(true, false).animate({ 'left': '0%' }, 100);
                FirstTouch = false;
            }
        });

        // Collapse the left menu by not touching the menu block
        left_menu.mouseleave(function () {
            left_menu.stop(true, false).animate({ 'left': '-195px' }, 500);
            FirstTouch = true;
        });

        $('.left_menu .menu_area .sub_list').hide();
        $('.left_menu .menu_area .active').siblings('.sub_list').show();
    }
}

// Initialize displaying status of the left menu list tag once directing into a webpage
function InitMenuStatus(menu_name, sub_list = null) {
    tag_color.removeClass('active');
    menu_area.eq(menu_name - 1).find('.tag_color').addClass('active');

    if (sub_list !== null) {
        sub_list_li.removeClass('.text_color');
        menu_area.eq(menu_name - 1).find('.sub_list ul li').eq(sub_list - 1).addClass('.text_color');
        menu_area.eq(menu_name - 1).find('.sub_list ul li a').eq(sub_list - 1).addClass('.text_color');
    }
}