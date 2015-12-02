// JavaScript Document
//Mở shopping cart
$(document).ready(function(){
    $("#btnAdd, #btnCart").click(function(){
        $("#mShopping_cart").modal();
    });
});

//Mở register
$(document).ready(function(){
    $("#btnRegister_nav").click(function(){
        $("#Register_box").modal();
    });
});

//Mở feedback
$(document).ready(function(){
    $("#btnFeedback_nav").click(function(){
        $("#Feedback_box").modal();
    });
});
function show_detail(){
	window.location="detail-product.html";
};

$(document).ready(function(){
    $(".btnDetail").click(function(){
        show_detail();
    });
});
$(document).ready(function(){
    $(".item_image").click(function(){
        show_detail();
    });
});

//Tooltip
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip(); 
});

//Profile
$(document).ready(function(){
    $(".nav-pills a").click(function(){
        $(this).tab('show');
    });

});

function deleteProductBills() {
    var x;
    if (confirm("Bạn muốn xóa đĩa này ra khỏi hóa đơn không?") == true) {
        x = "Code";
    } else {
    }
};
function deleteBills() {
    var x;
    if (confirm("Bạn muốn hủy hóa đơn này không?") == true) {
        x = "Code";
    } else {
    }
};

function deleteProductCart() {
    var x;
    if (confirm("Bạn muốn xóa đĩa này ra khỏi giỏ hàng không?") == true) {
        x = "Code";
    } else {
    }
};
