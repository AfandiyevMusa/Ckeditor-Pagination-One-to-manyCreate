$(document).ready(function () {


    $(document).on("click", ".show-more-btn", function () {

        let parent = $("#parent-elem");

        let skiptCount = $(parent).children().length;

        let productsCount = $("#products").attr("data-count");


        $.ajax({
            url: `shop/showmoreorless?skip=${skiptCount}`,
            type: "Get",
            success: function (res) {
                debugger
                $(parent).append(res);
                skiptCount = $(parent).children().length;
                if (skiptCount >= productsCount) {
                    $(".show-more-btn").addClass("d-none")
                    $(".show-less-btn").removeClass("d-none")
                }
            }

        })
    })

    $(document).on("click", ".show-less-btn", function () {

        let parent = $("#parent-elem");
        let skiptCount = 0;

        $.ajax({
            url: `shop/showmoreorless?skip=${skiptCount}`,
            type: "Get",
            success: function (res) {
                $(parent).empty();
                $(parent).append(res);
              
                $(".show-less-btn").addClass("d-none")
                $(".show-more-btn").removeClass("d-none")
            }

        })
    })
})