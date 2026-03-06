$(function () {
    var path = window.location.pathname.toLowerCase();

    $(".sidebar-nav .nav-item").each(function () {
        var href = ($(this).attr("href") || "").toLowerCase();
        if (href && path.startsWith(href)) {
            $(this).addClass("active");
        }
    });

    $("#sidebarToggle").on("click", function () {
        $("#appSidebar").toggleClass("open");
    });

    $(document).on("click", function (event) {
        var $sidebar = $("#appSidebar");
        var isMobile = window.matchMedia("(max-width: 991.98px)").matches;
        if (!isMobile || !$sidebar.hasClass("open")) return;

        var $target = $(event.target);
        if ($target.closest("#appSidebar").length === 0 && $target.closest("#sidebarToggle").length === 0) {
            $sidebar.removeClass("open");
        }
    });
});
