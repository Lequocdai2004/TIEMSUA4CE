let currentSlide = 0;

function showSlide(index) {
    let slides = document.querySelectorAll(".slide");
    let totalSlides = slides.length;

    // Đảm bảo index luôn nằm trong giới hạn
    if (index < 0) {
        currentSlide = totalSlides - 1;
    } else if (index >= totalSlides) {
        currentSlide = 0;
    } else {
        currentSlide = index;
    }

    // Ẩn tất cả các slides
    slides.forEach(slide => slide.classList.remove("active"));

    // Hiển thị slide hiện tại
    slides[currentSlide].classList.add("active");
}


function addToCart(productId) {
    // Gửi yêu cầu AJAX để thêm sản phẩm vào giỏ hàng
    $.ajax({
        url: '@Url.Action("ThemGioHang", "GioHang")', // Đường dẫn đến action AddToCart trong controller GioHang
        type: 'POST',
        data: { id: productId },
        success: function (response) {
            // Nếu thêm thành công, chuyển hướng đến trang giỏ hàng
            window.location.href = '@Url.Action("TrangGioHang", "GioHang")';
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi nếu có
            alert('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.');
        }
    });
}
</script >

function prevSlide() {
    showSlide(currentSlide - 1);
}

function nextSlide() {
    showSlide(currentSlide + 1);
}
    $(document).ready(function () {
        // Load header & footer vào trang
        $("#header").load("/lienket/components/header.html");
        $("#footer").load("/lienket/components/footer.html");
        $("#product-section").load("/sanpham/allproduct.html");

        let scrollPosition = 0;

        // Khi nhấn vào "Tài khoản", lưu vị trí hiện tại và mở modal



        //$("#accountLink").click(function (event) {
        //    event.preventDefault();
        //    scrollPosition = window.scrollY; // Lưu vị trí cuộn
        //    $("#loginModal").fadeIn();
        //    $("body").css({
        //        "overflow": "hidden",
        //        "position": "fixed",
        //        "width": "100%"
        //    }); // Ngăn cuộn trang khi modal mở
        //});



         // Khi nhấn vào nút đóng
         $(".close").click(function () {
            $("#loginModal").fadeOut(function () {
                $("body").css({
                    "overflow": "auto",
                    "position": "static"
                });
                window.scrollTo(0, scrollPosition);
                window.location.href = "/pages/index.html"; // Chuyển hướng về trang chủ
            });
        });

        // Khi nhấn ra ngoài modal cũng đóng
        $(window).click(function (event) {
            if (event.target.id === "loginModal") {
                $("#loginModal").fadeOut(function () {
                    $("body").css({
                        "overflow": "auto",
                        "position": "static"
                    });
                    window.scrollTo(0, scrollPosition);
                    window.location.href = "/pages//index.html"; // Chuyển hướng về trang chủ
                });
            }
        });

        $("#sendOTP").click(function () {
            let phone = $("#phone").val().trim();

            if (phone === "") {
                alert("Vui lòng nhập số điện thoại!");
                return;
            }

            // Kiểm tra số điện thoại có hợp lệ không (chỉ chứa số, độ dài hợp lý)
            let phoneRegex = /^[0-9]{9,11}$/; // Chấp nhận số từ 9 đến 11 chữ số
            if (!phoneRegex.test(phone)) {
                alert("Số điện thoại không hợp lệ. Vui lòng nhập lại!");
                return;
            }

            // Nếu hợp lệ, giả lập gửi OTP
            alert("Mã OTP đã được gửi đến số điện thoại " + phone);       
        });


        // Khi bấm vào Giỏ hàng, hiển thị modal
        $("#cartButton").click(function () {
            $("#cartModal").fadeIn();
        });

        // Khi bấm vào dấu X, đóng modal
        $("#cartModal .close").click(function () {
            $("#cartModal").fadeOut();
            window.location.href = "/pages/index.html";
        });

        // Đóng modal khi click ra ngoài
        $(window).click(function (event) {
            if ($(event.target).is("#cartModal")) {
                $("#cartModal").fadeOut();
                window.location.href = "/pages/index.html";
            }
        });
        $(".buy-now").click(function () {
            window.location.href = "/sanpham/product.html"; // Đổi thành link trang sản phẩm của bạn
        });


        let params = new URLSearchParams(window.location.search);
        let keyword = params.get("q") ? params.get("q").toLowerCase().trim() : "";

        if (keyword) {
            let searchResults = $("#searchResults");

            $.get("/sanpham/allproduct.html", function (data) {
                let htmlData = $("<div>").html(data);

                let matchingProducts = htmlData.find(".product-card").filter(function () {
                    let productTitle = $(this).find(".card-title").text().toLowerCase().trim();
                    return productTitle.includes(keyword);
                });

                if (matchingProducts.length > 0) {
                    let formattedResults = $("<div class='d-flex flex-wrap justify-content-start'></div>");
                    matchingProducts.each(function () {
                        let colDiv = $("<div class='col-md-4 mb-4'></div>").append($(this));
                        formattedResults.append(colDiv);
                    });
                    searchResults.html(formattedResults);
                } else {
                    searchResults.html("<p class='text-center'>Không tìm thấy sản phẩm nào.</p>");
                }
            });
        } else {
            $("#searchResults").html("<p class='text-center'>Vui lòng nhập từ khóa tìm kiếm.</p>");
        }


    });

    let currentPage = 2;
    
    function goToPage(page) {
        document.querySelectorAll(".page-btn").forEach(btn => btn.classList.remove("active"));
        document.querySelectorAll(".page-btn")[page].classList.add("active");
        currentPage = page;
    }
    
    function prevPage() {
        if (currentPage > 1) {
            goToPage(currentPage - 1);
        }
    }
    
    function nextPage() {
        if (currentPage < 8) {
            goToPage(currentPage + 1);
        }
    }

    function goToDetail(productId) {
        window.location.href = `/sanpham/chitietsanpham.html?id=${productId}`;
    }

function addToCart(productId) {
    // Gửi yêu cầu AJAX để thêm sản phẩm vào giỏ hàng
    $.ajax({
        url: '@Url.Action("ThemGioHang", "GioHang")', // Đường dẫn đến action AddToCart trong controller GioHang
        type: 'POST',
        data: { id: productId },
        success: function (response) {
            // Nếu thêm thành công, chuyển hướng đến trang giỏ hàng
            window.location.href = '@Url.Action("TrangGioHang", "GioHang")';
        },
        error: function (xhr, status, error) {
            // Xử lý lỗi nếu có
            alert('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.');
        }
    });
}
    


    