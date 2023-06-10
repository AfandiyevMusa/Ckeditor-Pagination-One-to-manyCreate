$(document).ready(function () {


    //add basket

    $(document).on("submit", "#basket-form", function (e) {
        e.preventDefault();

        let productId = $(this).attr("data-id");

        let data = { id: productId };

        $.ajax({
            url: "cart/addbasket",
            type: "Post",
            data: data,
            success: function (res) {
                $("sup.rounded-circle").text(res)
            }

        })

    })

    

    //$(document).on("click", ".show-more-btn", function () {

    //    let parent = $("#parent-elem");

    //    let skiptCount = $(parent).children().length;

    //    let productsCount = $("#products").attr("data-count");


    //    $.ajax({
    //        url: `shop/showmoreorless?skip=${skiptCount}`,
    //        type: "Get",
    //        success: function (res) {
    //            debugger
    //            $(parent).append(res);
    //            skiptCount = $(parent).children().length;
    //            if (skiptCount >= productsCount) {
    //                $(".show-more-btn").addClass("d-none")
    //                $(".show-less-btn").removeClass("d-none")
    //            }
    //        }

    //    })
    //})








})