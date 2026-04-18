using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarSales.Web.Controllers;

public class PagesController : Controller
{
    private static readonly Dictionary<string, ContentPageViewModel> PageConfigs = new(StringComparer.OrdinalIgnoreCase)
    {
        ["online-deposit"] = CreatePage("online-deposit", "Đặt cọc trực tuyến", "Đặt cọc xe nhanh chóng", "Thực hiện giữ chỗ mẫu xe yêu thích và nhận tư vấn từ nhân viên kinh doanh trong thời gian sớm nhất.", "Xem xe đang có", "/Cars/Index", "Liên hệ tư vấn", "/Pages/Details/contact"),
        ["available-cars"] = CreatePage("available-cars", "Kiểm tra xe có sẵn", "Kho xe sẵn sàng giao", "Cập nhật danh sách xe có sẵn tại showroom và xe có thể giao nhanh trong ngày.", "Xem danh sách xe", "/Cars/Index", "Yêu cầu báo giá", "/Pages/Details/request-a-quote"),
        ["build-and-price"] = CreatePage("build-and-price", "Lựa chọn mẫu xe và báo giá", "Cấu hình theo nhu cầu", "Chọn phiên bản, màu sắc và trang bị phù hợp để nhận báo giá tối ưu theo ngân sách.", "Khám phá xe", "/Cars/Index", "Nhận báo giá", "/Pages/Details/request-a-quote"),
        ["price-list"] = CreatePage("price-list", "Bảng giá", "Bảng giá tham khảo mới nhất", "Tra cứu giá bán theo từng dòng xe và phiên bản, cập nhật theo chương trình ưu đãi hiện hành.", "Xem mẫu xe", "/Cars/Index", "Liên hệ chi tiết", "/Pages/Details/contact"),
        ["parts-and-accessories"] = CreatePage("parts-and-accessories", "Phụ kiện & Phụ tùng", "Danh mục chính hãng", "Tìm hiểu phụ kiện, phụ tùng và gói nâng cấp phù hợp cho xe của bạn.", "Đặt lịch dịch vụ", "/Pages/Details/service-booking", "Liên hệ bộ phận phụ tùng", "/Pages/Details/contact"),
        ["test-drive"] = CreatePage("test-drive", "Đăng ký lái thử", "Trải nghiệm thực tế trước khi mua", "Đặt lịch lái thử nhanh, chọn địa điểm và thời gian phù hợp cùng tư vấn chuyên sâu.", "Đăng ký ngay", "/Pages/Details/test-drive-booking", "Xem xe", "/Cars/Index"),
        ["request-a-quote"] = CreatePage("request-a-quote", "Yêu cầu báo giá", "Nhận báo giá theo nhu cầu", "Gửi yêu cầu để nhận tư vấn gói xe, ưu đãi và phương án tài chính phù hợp.", "Xem xe", "/Cars/Index", "Liên hệ tư vấn", "/Pages/Details/contact"),

        ["services"] = CreatePage("services", "Dịch vụ", "Giải pháp chăm sóc toàn diện", "Cung cấp đầy đủ dịch vụ bảo dưỡng, sửa chữa và chăm sóc xe theo tiêu chuẩn kỹ thuật.", "Đặt lịch dịch vụ", "/Pages/Details/service-booking", "Tìm trung tâm", "/Pages/Details/service-centers"),
        ["service-booking"] = CreatePage("service-booking", "Đặt lịch Dịch vụ trực tuyến", "Đặt lịch nhanh gọn", "Đăng ký thời gian bảo dưỡng hoặc kiểm tra xe trực tuyến để tiết kiệm thời gian chờ.", "Liên hệ ngay", "/Pages/Details/contact", "Xem quy trình", "/Pages/Details/quick-maintenance-process"),
        ["service-estimate"] = CreatePage("service-estimate", "Báo Giá Tạm tính Chi phí Dịch vụ", "Ước tính chi phí minh bạch", "Nhận thông tin chi phí dự kiến theo hạng mục dịch vụ để chủ động kế hoạch tài chính.", "Đặt lịch dịch vụ", "/Pages/Details/service-booking", "Liên hệ cố vấn", "/Pages/Details/contact"),
        ["quick-maintenance-process"] = CreatePage("quick-maintenance-process", "Quy trình bảo dưỡng nhanh", "Tiêu chuẩn rõ ràng", "Thực hiện kiểm tra, báo giá, bảo dưỡng và bàn giao theo quy trình tối ưu trải nghiệm khách hàng.", "Đặt lịch", "/Pages/Details/service-booking", "Lịch định kỳ", "/Pages/Details/maintenance-schedule"),
        ["maintenance-schedule"] = CreatePage("maintenance-schedule", "Lịch bảo dưỡng định kỳ", "Chăm sóc xe đúng hạn", "Theo dõi mốc bảo dưỡng theo quãng đường để đảm bảo hiệu suất và độ bền xe.", "Đặt lịch ngay", "/Pages/Details/service-booking", "Liên hệ hỗ trợ", "/Pages/Details/contact"),
        ["service-centers"] = CreatePage("service-centers", "Cơ sở bảo hành bảo dưỡng", "Mạng lưới trung tâm", "Tìm địa điểm bảo hành, bảo dưỡng gần nhất với đội ngũ kỹ thuật viên chuyên nghiệp.", "Liên hệ trung tâm", "/Pages/Details/contact", "Xem dịch vụ", "/Pages/Details/services"),
        ["door-to-door-service"] = CreatePage("door-to-door-service", "Nhận & giao xe tận nơi miễn phí", "Tiện lợi và tiết kiệm thời gian", "Dịch vụ nhận và giao xe tận nơi, giúp bạn bảo dưỡng xe mà không ảnh hưởng lịch làm việc.", "Đặt lịch nhận xe", "/Pages/Details/service-booking", "Liên hệ hỗ trợ", "/Pages/Details/contact"),
        ["extended-warranty-terms"] = CreatePage("extended-warranty-terms", "Chứng nhận và Điều khoản hợp đồng bảo hành mở rộng", "Bảo vệ xe dài hạn", "Nắm rõ phạm vi, điều kiện và quyền lợi của các gói bảo hành mở rộng.", "Xem chính sách bảo hành", "/Pages/Details/warranty-policy", "Liên hệ tư vấn", "/Pages/Details/contact"),
        ["owners"] = CreatePage("owners", "Chủ xe", "Thông tin dành cho khách hàng sở hữu xe", "Tổng hợp tài liệu, dịch vụ và quyền lợi dành riêng cho chủ xe UTC Car.", "Dịch vụ bảo dưỡng", "/Pages/Details/services", "Liên hệ hỗ trợ", "/Pages/Details/contact"),
        ["contact"] = CreatePage("contact", "Liên hệ", "Kết nối với UTC Car", "Liên hệ bộ phận kinh doanh và chăm sóc khách hàng để được hỗ trợ nhanh nhất.", "Xem xe", "/Cars/Index", "Về trang chủ", "/"),

        ["payment-policy"] = CreatePage("payment-policy", "Chính sách thanh toán", "Phương thức linh hoạt", "Cập nhật phương thức thanh toán và các lưu ý giúp giao dịch nhanh chóng, minh bạch.", "Tư vấn mua xe", "/Pages/Details/contact", "Xem xe", "/Cars/Index"),
        ["warranty-policy"] = CreatePage("warranty-policy", "Chính sách bảo hành", "Cam kết hậu mãi", "Thông tin chi tiết về phạm vi bảo hành và điều kiện áp dụng cho từng dòng xe.", "Liên hệ dịch vụ", "/Pages/Details/contact", "Đặt lịch dịch vụ", "/Pages/Details/service-booking"),
        ["shipping-policy"] = CreatePage("shipping-policy", "Chính sách giao nhận vận chuyển", "Bàn giao an toàn", "Quy định giao nhận xe tại showroom hoặc giao tận nơi theo khu vực hỗ trợ.", "Liên hệ vận chuyển", "/Pages/Details/contact", "Xem xe", "/Cars/Index"),
        ["privacy-policy"] = CreatePage("privacy-policy", "Chính sách bảo mật thông tin", "Bảo vệ dữ liệu khách hàng", "Cam kết lưu trữ và xử lý thông tin cá nhân theo nguyên tắc bảo mật và minh bạch.", "Xem điều khoản", "/Pages/Details/terms-and-conditions", "Liên hệ", "/Pages/Details/contact"),
        ["terms-and-conditions"] = CreatePage("terms-and-conditions", "Điều kiện và điều khoản", "Quy định sử dụng dịch vụ", "Tổng hợp các điều khoản trong quá trình mua bán, sử dụng dịch vụ và bảo hành.", "Liên hệ hỗ trợ", "/Pages/Details/contact", "Về trang chủ", "/"),
        ["course-requirements"] = CreatePage("course-requirements", "Đối chiếu yêu cầu học phần", "Tổng hợp tính năng theo đề bài", "Trang tổng hợp các chức năng chính đã triển khai và định hướng bổ sung để hoàn thiện đồ án.", "Xem danh sách xe", "/Cars/Index", "Liên hệ", "/Pages/Details/contact"),
        ["security-and-i18n"] = CreatePage("security-and-i18n", "Bảo mật và quốc tế hóa", "An toàn và mở rộng", "Định hướng triển khai các biện pháp bảo mật và hỗ trợ đa ngôn ngữ cho hệ thống.", "Xem yêu cầu học phần", "/Pages/Details/course-requirements", "Liên hệ", "/Pages/Details/contact"),
        ["testing-and-quality"] = CreatePage("testing-and-quality", "Kiểm thử và chất lượng", "Đảm bảo tính ổn định", "Kế hoạch kiểm thử chức năng và tiêu chí nghiệm thu trước khi bàn giao sản phẩm.", "Xem yêu cầu học phần", "/Pages/Details/course-requirements", "Về trang chủ", "/"),

        ["custom-order"] = CreatePage("custom-order", "Đặt xe theo yêu cầu", "Cấu hình riêng cho bạn", "Hỗ trợ đặt xe theo phiên bản, màu sắc và trang bị theo nhu cầu cá nhân.", "Yêu cầu báo giá", "/Pages/Details/request-a-quote", "Xem xe", "/Cars/Index"),
        ["consignment"] = CreatePage("consignment", "Ký gửi xe", "Giải pháp bán xe thuận tiện", "Hỗ trợ định giá, trưng bày và tìm kiếm khách mua phù hợp cho xe ký gửi.", "Liên hệ ký gửi", "/Pages/Details/contact", "Xem thị trường xe", "/Cars/Index"),
        ["trade-in"] = CreatePage("trade-in", "Đổi xe", "Nâng cấp xe dễ dàng", "Đổi xe cũ lấy xe mới với quy trình thẩm định minh bạch và tư vấn phương án tài chính tối ưu.", "Tư vấn đổi xe", "/Pages/Details/contact", "Xem xe mới", "/Cars/Index"),

        ["deposit-management"] = CreatePage("deposit-management", "Quản lý đặt cọc", "Theo dõi trạng thái cọc", "Xem thông tin giao dịch đặt cọc và cập nhật tiến độ xử lý từ đội ngũ tư vấn.", "Xe đang mở bán", "/Cars/Index", "Liên hệ hỗ trợ", "/Pages/Details/contact"),
        ["account-profile"] = CreatePage("account-profile", "Tài khoản", "Thông tin cá nhân", "Quản lý thông tin tài khoản, cập nhật liên hệ và lịch sử giao dịch của bạn.", "Về trang chủ", "/", "Liên hệ hỗ trợ", "/Pages/Details/contact"),
        ["test-drive-booking"] = CreatePage("test-drive-booking", "Đăng ký lái thử", "Đặt lịch trải nghiệm", "Xác nhận thời gian lái thử và nhận hỗ trợ từ tư vấn viên UTC Car.", "Xem xe", "/Cars/Index", "Liên hệ", "/Pages/Details/contact")
    };

    static PagesController()
    {
        ConfigureFooterPages();
    }

    [HttpGet]
    public IActionResult Details(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug) || !PageConfigs.TryGetValue(slug, out var page))
        {
            return NotFound();
        }

        return View(page);
    }

    private static ContentPageViewModel CreatePage(
        string slug,
        string title,
        string intro,
        string description,
        string primaryButtonText,
        string primaryButtonUrl,
        string secondaryButtonText,
        string secondaryButtonUrl)
    {
        return new ContentPageViewModel
        {
            Slug = slug,
            Title = title,
            Intro = intro,
            Description = description,
            PrimaryButtonText = primaryButtonText,
            PrimaryButtonUrl = primaryButtonUrl,
            SecondaryButtonText = secondaryButtonText,
            SecondaryButtonUrl = secondaryButtonUrl,
            Highlights =
            [
                "Nội dung đang được cập nhật theo chuẩn thương hiệu UTC Car.",
                "Bạn có thể liên hệ trực tiếp để nhận thông tin chi tiết mới nhất.",
                "Đội ngũ tư vấn hỗ trợ nhanh chóng trong giờ hành chính."
            ]
        };
    }

    private static void ConfigureFooterPages()
    {
        ConfigurePaymentPolicy();
        ConfigureWarrantyPolicy();
        ConfigureShippingPolicy();
        ConfigurePrivacyPolicy();
        ConfigureTermsAndConditions();
        ConfigureCourseRequirementPages();
    }

    private static void ConfigureCourseRequirementPages()
    {
        if (PageConfigs.TryGetValue("course-requirements", out var requirementPage))
        {
            requirementPage.Highlights =
            [
                "Có phân quyền người dùng: Admin, Staff, Customer.",
                "Có Filtering, AJAX và đã bổ sung Paging tại trang Cars.",
                "Có CRUD, upload ảnh, session/cookies, validation annotations và REST API."
            ];

            requirementPage.ContentBlocks =
            [
                new ContentBlockViewModel
                {
                    Heading = "3.1 - 3.3: Chức năng nền tảng",
                    Description = "Các yêu cầu về kiến trúc và tính năng chính đã được triển khai trong hệ thống.",
                    Items =
                    [
                        "Phân quyền theo vai trò và area quản trị riêng.",
                        "Lọc theo từ khóa, loại xe, hãng xe; hỗ trợ phân trang danh sách.",
                        "CRUD xe trong khu vực Admin, upload ảnh đại diện xe.",
                        "Session/cookies cho đăng nhập, ghi nhớ email.",
                        "Validation thông qua DataAnnotations trên model.",
                        "Các API controller theo mô hình RESTFUL."
                    ]
                },
                new ContentBlockViewModel
                {
                    Heading = "3.4 - 3.5: Hướng hoàn thiện",
                    Description = "Các hạng mục nên tiếp tục bổ sung để tăng mức hoàn chỉnh học phần.",
                    Items =
                    [
                        "Bổ sung bộ kiểm thử tự động cho Controller/API.",
                        "Tăng cường bảo mật: anti-forgery, policy, logging, rate limit.",
                        "Quốc tế hóa giao diện bằng resource file và chọn ngôn ngữ."
                    ]
                }
            ];
        }

        if (PageConfigs.TryGetValue("security-and-i18n", out var securityPage))
        {
            securityPage.ContentBlocks =
            [
                new ContentBlockViewModel
                {
                    Heading = "Bảo mật",
                    Description = "Nhóm biện pháp nên triển khai ở lớp ứng dụng và API.",
                    Items =
                    [
                        "Sử dụng HTTPS, giới hạn CORS và bảo vệ CSRF cho form.",
                        "Kiểm soát truy cập theo vai trò và logging sự kiện quan trọng.",
                        "Ràng buộc dữ liệu đầu vào và xử lý lỗi an toàn."
                    ]
                },
                new ContentBlockViewModel
                {
                    Heading = "Quốc tế hóa (i18n)",
                    Description = "Chuẩn bị kiến trúc để hỗ trợ nhiều ngôn ngữ.",
                    Items =
                    [
                        "Tách nội dung text sang resource file.",
                        "Tùy chọn ngôn ngữ theo cookie hoặc profile người dùng.",
                        "Định dạng ngày/tiền tệ theo CultureInfo."
                    ]
                }
            ];
        }

        if (PageConfigs.TryGetValue("testing-and-quality", out var testingPage))
        {
            testingPage.ContentBlocks =
            [
                new ContentBlockViewModel
                {
                    Heading = "Kiểm thử chức năng",
                    Description = "Đề xuất checklist trước nghiệm thu.",
                    Items =
                    [
                        "Đăng ký/đăng nhập và phân quyền truy cập đúng role.",
                        "CRUD xe, upload ảnh, lọc và phân trang hoạt động chính xác.",
                        "API trả đúng mã trạng thái và dữ liệu mong đợi."
                    ]
                },
                new ContentBlockViewModel
                {
                    Heading = "Kiểm thử giao diện",
                    Description = "Đảm bảo đáp ứng yêu cầu responsive và trải nghiệm người dùng.",
                    Items =
                    [
                        "Kiểm thử trên desktop/tablet/mobile.",
                        "Kiểm tra menu, footer và các trang nội dung mới.",
                        "Đánh giá tốc độ tải trang và tính nhất quán giao diện."
                    ]
                }
            ];
        }
    }

    private static void ConfigurePaymentPolicy()
    {
        if (!PageConfigs.TryGetValue("payment-policy", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Hỗ trợ đặt cọc nhanh và xác nhận trong ngày làm việc.",
            "Linh hoạt phương thức thanh toán theo tiến độ mua xe.",
            "Cam kết hóa đơn, chứng từ đầy đủ và minh bạch."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Hình thức thanh toán",
                Description = "Khách hàng có thể lựa chọn phương thức phù hợp trong quá trình giao dịch.",
                Items =
                [
                    "Chuyển khoản ngân hàng vào tài khoản công ty.",
                    "Thanh toán trực tiếp tại showroom qua POS hoặc tiền mặt theo hạn mức pháp luật.",
                    "Thanh toán từng đợt theo hợp đồng mua bán."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Tiến độ thanh toán tham khảo",
                Description = "Tùy từng dòng xe và hợp đồng thực tế, tiến độ có thể điều chỉnh.",
                Items =
                [
                    "Đợt 1: Đặt cọc giữ xe.",
                    "Đợt 2: Thanh toán trước khi làm thủ tục bàn giao.",
                    "Đợt 3: Hoàn tất phần còn lại trước khi nhận xe hoặc theo lịch giải ngân ngân hàng."
                ]
            }
        ];
    }

    private static void ConfigureWarrantyPolicy()
    {
        if (!PageConfigs.TryGetValue("warranty-policy", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Bảo hành theo điều kiện nhà sản xuất và chính sách đại lý.",
            "Đội ngũ kỹ thuật tiếp nhận nhanh các yêu cầu kiểm tra.",
            "Linh kiện chính hãng, quy trình minh bạch."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Phạm vi bảo hành",
                Description = "Áp dụng cho các lỗi kỹ thuật do nhà sản xuất trong thời gian bảo hành còn hiệu lực.",
                Items =
                [
                    "Động cơ, hộp số và hệ thống điện theo tiêu chuẩn hãng.",
                    "Các hạng mục linh kiện khác theo sổ bảo hành xe.",
                    "Thời hạn và giới hạn quãng đường tùy từng mẫu xe."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Trường hợp không thuộc bảo hành",
                Description = "Một số tình huống phát sinh từ điều kiện sử dụng sẽ không nằm trong phạm vi bảo hành.",
                Items =
                [
                    "Hao mòn tự nhiên của vật tư tiêu hao.",
                    "Hư hỏng do tai nạn, ngập nước hoặc tự ý thay đổi kết cấu xe.",
                    "Bảo dưỡng không đúng lịch khuyến nghị của hãng."
                ]
            }
        ];
    }

    private static void ConfigureShippingPolicy()
    {
        if (!PageConfigs.TryGetValue("shipping-policy", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Giao xe tại showroom hoặc tận nơi theo yêu cầu.",
            "Kiểm tra xe và hồ sơ trước khi bàn giao.",
            "Hỗ trợ khách hàng ở nhiều tỉnh thành."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Phạm vi giao nhận",
                Description = "UTC Car hỗ trợ bàn giao xe linh hoạt theo khu vực và thỏa thuận hợp đồng.",
                Items =
                [
                    "Nhận xe trực tiếp tại showroom.",
                    "Giao xe tận nơi tại nội thành theo lịch hẹn.",
                    "Hỗ trợ vận chuyển liên tỉnh qua đơn vị đối tác."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Quy trình bàn giao",
                Description = "Mỗi xe đều được kiểm tra và xác nhận tình trạng trước khi khách nhận.",
                Items =
                [
                    "Đối chiếu giấy tờ xe, hợp đồng và thông tin thanh toán.",
                    "Kiểm tra ngoại thất, nội thất, tính năng cơ bản cùng tư vấn viên.",
                    "Ký biên bản bàn giao và hướng dẫn sử dụng nhanh."
                ]
            }
        ];
    }

    private static void ConfigurePrivacyPolicy()
    {
        if (!PageConfigs.TryGetValue("privacy-policy", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Thông tin cá nhân được lưu trữ có kiểm soát.",
            "Không chia sẻ trái phép cho bên thứ ba.",
            "Khách hàng có quyền yêu cầu cập nhật hoặc xóa dữ liệu theo quy định."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Dữ liệu được thu thập",
                Description = "Dữ liệu cần thiết cho mục đích tư vấn, mua bán và chăm sóc khách hàng.",
                Items =
                [
                    "Họ tên, số điện thoại, email và địa chỉ liên hệ.",
                    "Thông tin giao dịch, đặt cọc và nhu cầu sản phẩm.",
                    "Lịch sử tương tác trên website và biểu mẫu đăng ký."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Mục đích sử dụng",
                Description = "UTC Car chỉ sử dụng thông tin cho các mục tiêu liên quan đến dịch vụ.",
                Items =
                [
                    "Tư vấn sản phẩm, báo giá và xử lý yêu cầu dịch vụ.",
                    "Cập nhật chương trình ưu đãi phù hợp nếu khách hàng đồng ý nhận tin.",
                    "Nâng cao chất lượng chăm sóc và hậu mãi."
                ]
            }
        ];
    }

    private static void ConfigureTermsAndConditions()
    {
        if (!PageConfigs.TryGetValue("terms-and-conditions", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Áp dụng cho toàn bộ giao dịch và dịch vụ tại UTC Car.",
            "Điều khoản có thể cập nhật theo quy định pháp luật hiện hành.",
            "Khách hàng vui lòng đọc kỹ trước khi xác nhận giao dịch."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Điều khoản chung",
                Description = "Điều khoản giúp bảo vệ quyền lợi của cả khách hàng và doanh nghiệp.",
                Items =
                [
                    "Thông tin niêm yết trên website có thể thay đổi theo thời điểm.",
                    "Các ưu đãi chỉ áp dụng trong thời gian công bố.",
                    "Hợp đồng ký kết là căn cứ pháp lý cuối cùng cho từng giao dịch."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Giải quyết khiếu nại",
                Description = "UTC Car ưu tiên xử lý phản hồi nhanh chóng và minh bạch.",
                Items =
                [
                    "Tiếp nhận khiếu nại qua hotline, email hoặc trực tiếp tại showroom.",
                    "Phản hồi bước đầu trong giờ làm việc gần nhất.",
                    "Phối hợp các bộ phận liên quan để xử lý theo đúng quy trình."
                ]
            }
        ];
    }
}
