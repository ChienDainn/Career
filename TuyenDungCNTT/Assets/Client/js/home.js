
// Khi có sự kiện click vào cửa sổ (window), loại bỏ lớp 'active' khỏi phần tử có lớp 'search__autocom--box'
window.addEventListener('click', () => {
    _(".search__autocom--box").classList.remove('active');
})

// Show login form
__('#btn-login, .login--close, .main__login').forEach((item) => {
    item.addEventListener('click', () => {
        _('.main__login').classList.toggle('show');
    })
})

__('.register--close, .main__register').forEach((item) => {
    item.addEventListener('click', () => {
        _('.main__register').classList.toggle('show');
    })
})
// Ngăn sự kiện click từ lan truyền ra cửa sổ chính
__('.main__login--wrapper, .main__register--wrapper').forEach((item) => {
    item.addEventListener('click', (e) => {
        e.stopPropagation();
    });
});
// Chuyển đổi giữa biểu mẫu đăng nhập và đăng ký
_('#link-register').addEventListener('click', (e) => {
    _('.main__register').classList.add('show');
    _('.main__login').classList.remove('show');
});

_('#link-login').addEventListener('click', (e) => {
    _('.main__register').classList.remove('show');
    _('.main__login').classList.add('show');
});

// Login controller
var user = {
    // Hàm khởi tạo
    init: function () {
        // Gọi các hàm login và register khi user được khởi tạo
        user.login();
        user.register();
    },

    // Hàm xử lý đăng nhập người dùng
    login: function () {
        // Gắn một bộ xử lý sự kiện click cho phần tử có ID 'form_btn-login'
        $('#form_btn-login').off('click').on('click', (e) => {
            // Lấy giá trị từ các trường email và mật khẩu mà người dùng nhập
            let login_email = $('#login_email').val();
            let login_password = $('#login_password').val();

            // Tạo một đối tượng data chứa thông tin đăng nhập
            var data = {
                "login_email": login_email,
                "login_password": login_password
            };

            // Kiểm tra nếu cả email và mật khẩu không rỗng
            if (login_email.length > 0 && login_password.length > 0) {
                e.preventDefault(); // Ngăn mặc định hành vi gửi biểu mẫu

                // Gửi một yêu cầu AJAX đến máy chủ
                $.ajax({
                    url: "/Home/Login",          // URL để gửi yêu cầu
                    type: "POST",                // Phương thức HTTP (POST)
                    data: JSON.stringify(data),  // Dữ liệu được gửi dưới dạng JSON
                    dataType: "json",            // Loại dữ liệu phản hồi được mong đợi
                    contentType: "application/json",  // Loại nội dung của yêu cầu
                    success: function (response) {
                        // Xử lý phản hồi từ máy chủ
                        if (response.success) {
                            // Nếu đăng nhập thành công, chuyển hướng đến trang chủ
                            window.location.href = "/";
                        } else {
                            // Nếu đăng nhập thất bại, hiển thị một thông báo lỗi trong 2 giây
                            $('#login_message').text(response.data);
                            setTimeout(() => {
                                $('#login_message').text("");
                                $('#login_message').css("display","block");
                            }, 2000);
                        }
                    },
                    error: () => {
                        // Hiển thị một thông báo cảnh báo nếu có lỗi xảy ra
                        alert("Đã có lỗi xảy ra..."); // Có lỗi xảy ra...
                    }
                });
            }
        });
    },

    // Các hàm khác như 'register' có thể được định nghĩa ở đây


    register: function () {
        // Gắn một bộ xử lý sự kiện click cho phần tử có ID 'form_btn-register'
        $('#form_btn-register').off('click').on('click', (e) => {
            // Lấy giá trị từ các trường tên, email, mật khẩu và xác nhận mật khẩu mà người dùng nhập
            let register_name = $('#register_name').val();
            let register_email = $('#register_email').val();
            let register_password = $('#register_password').val();
            let password_confirm = $('#password_confirm').val();

            // Tạo một đối tượng data chứa thông tin đăng ký
            var data = {
                "register_name": register_name,
                "register_email": register_email,
                "register_password": register_password,
                "password_confirm": password_confirm
            };

            // Kiểm tra nếu cả tên, email và mật khẩu không rỗng
            if (register_name.length > 0 && register_email.length > 0 && register_password.length > 0) {
                e.preventDefault(); // Ngăn mặc định hành vi gửi biểu mẫu

                // Gửi một yêu cầu AJAX đến máy chủ để đăng ký
                $.ajax({
                    url: "/Home/Register",      // URL để gửi yêu cầu
                    type: "POST",              // Phương thức HTTP (POST)
                    data: JSON.stringify(data),  // Dữ liệu được gửi dưới dạng JSON
                    dataType: "json",          // Loại dữ liệu phản hồi được mong đợi
                    contentType: "application/json",  // Loại nội dung của yêu cầu
                    success: function (response) {
                        // Xử lý phản hồi từ máy chủ
                        if (response.success) {
                            // Nếu đăng ký thành công, thực hiện các thay đổi giao diện
                            _('.main__register').classList.remove('show');
                            _('.main__login').classList.add('show');
                        } else {
                            // Nếu đăng ký thất bại, hiển thị một thông báo lỗi trong 2 giây
                            $('#register_message').text(response.data);
                            setTimeout(() => {
                                $('#register_message').text("");
                                $('#register_message').css("display", "block");
                            }, 2000);
                        }
                        console.log(response);  // In phản hồi ra màn hình console
                    },
                    error: () => {
                        // Hiển thị một thông báo cảnh báo nếu có lỗi xảy ra trong quá trình gửi yêu cầu
                        alert("Đã có lỗi xảy ra..."); // Có lỗi xảy ra...
                    }
                });
            }
        })
    }

}

user.init();