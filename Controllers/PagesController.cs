using CarSales.Web.Data;
using CarSales.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarSales.Web.Controllers;

public class PagesController : Controller
{
    private const string ContactSlug = "contact";
    private const string RequestAQuotePath = "/Pages/RequestAQuote";

    private readonly ApplicationDbContext _context;

    public PagesController(ApplicationDbContext context)
    {
        _context = context;
    }

    private static string DetailsUrl(string slug) => $"/Pages/Details?slug={Uri.EscapeDataString(slug)}";

    private static readonly Dictionary<string, ContentPageViewModel> PageConfigs = new(StringComparer.OrdinalIgnoreCase)
    {
        ["online-deposit"] = CreatePage("online-deposit", "Đặt cọc trực tuyến", "Đặt cọc xe nhanh chóng", "Thực hiện giữ chỗ mẫu xe yêu thích và nhận tư vấn từ nhân viên kinh doanh trong thời gian sớm nhất.", "Xem xe đang có", "/Cars/Index", "Liên hệ tư vấn", DetailsUrl(ContactSlug)),
        ["available-cars"] = CreatePage("available-cars", "Kiểm tra xe có sẵn", "Kho xe sẵn sàng giao", "Cập nhật danh sách xe có sẵn tại showroom và xe có thể giao nhanh trong ngày.", "Xem danh sách xe", "/Cars/Index", "Yêu cầu báo giá", RequestAQuotePath),
        ["build-and-price"] = CreatePage("build-and-price", "Lựa chọn mẫu xe và báo giá", "Cấu hình theo nhu cầu", "Tùy chọn dòng xe, phiên bản và trang bị để nhận báo giá phù hợp với ngân sách của bạn.", "Khám phá xe", "/Cars/Index", "Yêu cầu báo giá", DetailsUrl(ContactSlug)),
        ["price-list"] = CreatePage("price-list", "Bảng giá", "Cập nhật mức giá mới nhất", "Tra cứu bảng giá tham khảo theo từng dòng xe và phiên bản đang phân phối.", "Xem xe", "/Cars/Index", "Yêu cầu báo giá", RequestAQuotePath),
        ["test-drive"] = CreatePage("test-drive", "Đăng ký lái thử", "Trải nghiệm thực tế trước khi mua", "Đặt lịch lái thử nhanh, chọn địa điểm và thời gian phù hợp cùng tư vấn chuyên sâu.", "Đăng ký ngay", "/Pages/Details/test-drive-booking", "Xem xe", "/Cars/Index"),
        ["request-a-quote"] = CreatePage("request-a-quote", "Yêu cầu báo giá", "Nhận báo giá theo nhu cầu", "Gửi yêu cầu để nhận tư vấn gói xe, ưu đãi và phương án tài chính phù hợp.", "Xem xe", "/Cars/Index", "Liên hệ tư vấn", DetailsUrl(ContactSlug)),
        ["coming-soon"] = CreatePage("coming-soon", "Sắp ra mắt", "Tính năng đang phát triển", "Tính năng này đang được hoàn thiện và sẽ sớm được cập nhật trong thời gian tới.", "Về trang chủ", "/", "Liên hệ", DetailsUrl(ContactSlug)),
        ["contact"] = CreatePage("contact", "Liên hệ", "Kết nối với UTC Car", "Liên hệ bộ phận kinh doanh và chăm sóc khách hàng để được hỗ trợ nhanh nhất.", "Xem xe", "/Cars/Index", "Về trang chủ", "/"),

        ["payment-policy"] = CreatePage("payment-policy", "Chính sách thanh toán", "Phương thức linh hoạt", "Cập nhật phương thức thanh toán và các lưu ý giúp giao dịch nhanh chóng, minh bạch.", "Tư vấn mua xe", DetailsUrl(ContactSlug), "Xem xe", "/Cars/Index"),
        ["warranty-policy"] = CreatePage("warranty-policy", "Chính sách bảo hành", "Cam kết hậu mãi", "Thông tin chi tiết về phạm vi bảo hành và điều kiện áp dụng cho từng dòng xe.", "Liên hệ dịch vụ", DetailsUrl(ContactSlug), "Đặt lịch dịch vụ", "/Pages/Details/coming-soon"),
        ["shipping-policy"] = CreatePage("shipping-policy", "Chính sách giao nhận vận chuyển", "Bàn giao an toàn", "Quy định giao nhận xe tại showroom hoặc giao tận nơi theo khu vực hỗ trợ.", "Liên hệ vận chuyển", DetailsUrl(ContactSlug), "Xem xe", "/Cars/Index"),
        ["privacy-policy"] = CreatePage("privacy-policy", "Chính sách bảo mật thông tin", "Bảo vệ dữ liệu khách hàng", "Cam kết lưu trữ và xử lý thông tin cá nhân theo nguyên tắc bảo mật và minh bạch.", "Xem điều khoản", "/Pages/Details/terms-and-conditions", "Liên hệ", DetailsUrl(ContactSlug)),
        ["terms-and-conditions"] = CreatePage("terms-and-conditions", "Điều kiện và điều khoản", "Quy định sử dụng dịch vụ", "Tổng hợp các điều khoản trong quá trình mua bán, sử dụng dịch vụ và bảo hành.", "Liên hệ hỗ trợ", DetailsUrl(ContactSlug), "Về trang chủ", "/"),
        ["security-and-i18n"] = CreatePage("security-and-i18n", "Bảo mật và quốc tế hóa", "An toàn và mở rộng", "Định hướng triển khai các biện pháp bảo mật và hỗ trợ đa ngôn ngữ cho hệ thống.", "Về trang chủ", "/", "Liên hệ", DetailsUrl(ContactSlug)),
        ["testing-and-quality"] = CreatePage("testing-and-quality", "Kiểm thử và chất lượng", "Đảm bảo tính ổn định", "Kế hoạch kiểm thử chức năng và tiêu chí nghiệm thu trước khi bàn giao sản phẩm.", "Về trang chủ", "/", "Liên hệ", DetailsUrl(ContactSlug)),

        ["custom-order"] = CreatePage("custom-order", "Đặt xe theo yêu cầu", "Cấu hình riêng cho bạn", "Hỗ trợ đặt xe theo phiên bản, màu sắc và trang bị theo nhu cầu cá nhân.", "Yêu cầu báo giá", RequestAQuotePath, "Xem xe", "/Cars/Index"),
        ["consignment"] = CreatePage("consignment", "Ký gửi xe", "Giải pháp bán xe thuận tiện", "Hỗ trợ định giá, trưng bày và tìm kiếm khách mua phù hợp cho xe ký gửi.", "Liên hệ ký gửi", DetailsUrl(ContactSlug), "Xem thị trường xe", "/Cars/Index"),
        ["trade-in"] = CreatePage("trade-in", "Đổi xe", "Nâng cấp xe dễ dàng", "Đổi xe cũ lấy xe mới với quy trình thẩm định minh bạch và tư vấn phương án tài chính tối ưu.", "Tư vấn đổi xe", DetailsUrl(ContactSlug), "Xem xe mới", "/Cars/Index"),

        ["deposit-management"] = CreatePage("deposit-management", "Quản lý đặt cọc", "Theo dõi trạng thái cọc", "Xem thông tin giao dịch đặt cọc và cập nhật tiến độ xử lý từ đội ngũ tư vấn.", "Xe đang mở bán", "/Cars/Index", "Liên hệ hỗ trợ", DetailsUrl(ContactSlug)),
        ["account-profile"] = CreatePage("account-profile", "Tài khoản", "Thông tin cá nhân", "Quản lý thông tin tài khoản, cập nhật liên hệ và lịch sử giao dịch của bạn.", "Về trang chủ", "/", "Liên hệ hỗ trợ", DetailsUrl(ContactSlug)),
        ["test-drive-booking"] = CreatePage("test-drive-booking", "Đăng ký lái thử", "Đặt lịch trải nghiệm", "Xác nhận thời gian lái thử và nhận hỗ trợ từ tư vấn viên UTC Car.", "Xem xe", "/Cars/Index", "Liên hệ", DetailsUrl(ContactSlug))
    };

    static PagesController()
    {
        ConfigureFooterPages();
    }

    [HttpGet]
    public IActionResult Details(string slug)
    {
        if (string.Equals(slug, "request-a-quote", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction(nameof(RequestAQuote));
        }

        if (string.Equals(slug, "account-profile", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction(nameof(AccountProfile));
        }

        if (string.IsNullOrWhiteSpace(slug) || !PageConfigs.TryGetValue(slug, out var page))
        {
            return NotFound();
        }

        return View(page);
    }

    [HttpGet]
    public IActionResult RequestAQuote(int? carId)
    {
        var user = GetCurrentUser();
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cars = _context.Cars
            .Where(x => x.Status == "Available")
            .OrderByDescending(x => x.IsFeatured)
            .ThenBy(x => x.CarName)
            .Select(x => new RequestAQuoteCarOptionViewModel
            {
                CarId = x.CarId,
                CarName = x.CarName,
                Price = x.Price,
                Thumbnail = x.Thumbnail
            })
            .ToList();

        var selectedCarId = carId.HasValue && cars.Any(x => x.CarId == carId.Value)
            ? carId.Value
            : cars.FirstOrDefault()?.CarId ?? 0;

        var selectedCar = cars.FirstOrDefault(x => x.CarId == selectedCarId);

        var hasOpenPurchaseRequest = selectedCarId > 0 && _context.PurchaseRequests.Any(x =>
            x.UserId == user.UserId &&
            x.CarId == selectedCarId &&
            (x.Status == "New" || x.Status == "Processing"));

        var vm = new RequestAQuotePageViewModel
        {
            Cars = cars,
            SelectedCarId = selectedCarId,
            SelectedCar = selectedCar,
            HasOpenPurchaseRequest = hasOpenPurchaseRequest
        };

        return View(vm);
    }

    [HttpGet]
    public IActionResult AccountProfile()
    {
        var user = GetCurrentUser();
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var vm = new AccountProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Address = user.Address,
            Avatar = user.Avatar
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AccountProfile(AccountProfileViewModel model)
    {
        var user = GetCurrentUser();
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        model.Email = user.Email;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        user.FullName = model.FullName.Trim();
        user.Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone.Trim();
        user.Address = string.IsNullOrWhiteSpace(model.Address) ? null : model.Address.Trim();
        user.Avatar = string.IsNullOrWhiteSpace(model.Avatar) ? null : model.Avatar.Trim();

        _context.SaveChanges();

        HttpContext.Session.SetString("FullName", user.FullName);

        TempData["Success"] = "Cập nhật thông tin tài khoản thành công.";
        return RedirectToAction(nameof(AccountProfile));
    }

    private Models.User? GetCurrentUser()
    {
        int.TryParse(HttpContext.Session.GetString("UserId"), out var userId);
        if (userId <= 0)
        {
            return null;
        }

        return _context.Users.FirstOrDefault(x => x.UserId == userId && x.IsActive);
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
        ConfigureContactPage();
        ConfigureBuyPages();
        ConfigurePaymentPolicy();
        ConfigureWarrantyPolicy();
        ConfigureShippingPolicy();
        ConfigurePrivacyPolicy();
        ConfigureTermsAndConditions();
        ConfigureSecurityAndTestingPages();
    }

    private static void ConfigureBuyPages()
    {
        ConfigureOnlineDepositPage();
        ConfigureAvailableCarsPage();
        ConfigureBuildAndPricePage();
        ConfigurePriceListPage();
    }

    private static void ConfigureOnlineDepositPage()
    {
        if (!PageConfigs.TryGetValue("online-deposit", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Giữ chỗ xe nhanh với quy trình trực tuyến đơn giản.",
            "Xác nhận thông tin cọc và liên hệ trong thời gian sớm nhất.",
            "Hỗ trợ tư vấn phương án thanh toán phù hợp."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Quy trình đặt cọc",
                Description = "Các bước cơ bản để hoàn tất yêu cầu đặt cọc online.",
                Items =
                [
                    "Chọn mẫu xe và điền thông tin liên hệ.",
                    "Xác nhận số tiền cọc theo tư vấn.",
                    "Nhận thông báo xác nhận từ nhân viên kinh doanh."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Lưu ý khi đặt cọc",
                Description = "Đảm bảo thông tin chính xác để xử lý nhanh hơn.",
                Items =
                [
                    "Sử dụng số điện thoại và email đang hoạt động.",
                    "Kiểm tra điều kiện hoàn/hủy cọc trước khi xác nhận.",
                    "Liên hệ hotline nếu cần hỗ trợ khẩn."
                ]
            }
        ];
    }

    private static void ConfigureAvailableCarsPage()
    {
        if (!PageConfigs.TryGetValue("available-cars", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Danh sách xe có sẵn được cập nhật thường xuyên.",
            "Có thể giao nhanh với nhiều lựa chọn phiên bản.",
            "Nhận tư vấn trực tiếp theo ngân sách và nhu cầu sử dụng."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Xe sẵn giao",
                Description = "Nguồn xe hiện có tại hệ thống showroom.",
                Items =
                [
                    "Xe mới đủ phiên bản theo từng dòng.",
                    "Một số mẫu có ưu đãi theo chương trình tháng.",
                    "Kiểm tra tồn kho theo khu vực hỗ trợ."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Cách kiểm tra nhanh",
                Description = "Chủ động nắm trạng thái xe trước khi đến showroom.",
                Items =
                [
                    "Tra cứu danh sách xe trên mục Tìm xe.",
                    "Gửi yêu cầu báo giá để giữ thông tin mẫu xe quan tâm.",
                    "Liên hệ tư vấn để xác nhận lịch xem xe."
                ]
            }
        ];
    }

    private static void ConfigureBuildAndPricePage()    
    {
        if (!PageConfigs.TryGetValue("build-and-price", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Tùy chọn phiên bản và trang bị theo nhu cầu cá nhân.",
            "Ước tính chi phí dựa trên cấu hình xe thực tế.",
            "Dễ dàng so sánh phương án trước khi ra quyết định."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Tùy chọn cấu hình",
                Description = "Xây dựng mẫu xe phù hợp phong cách và ngân sách.",
                Items =
                [
                    "Chọn dòng xe, phiên bản và màu sắc.",
                    "Thêm gói phụ kiện theo nhu cầu sử dụng.",
                    "So sánh trang bị giữa các phiên bản."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Nhận báo giá",
                Description = "Nhận bảng giá tạm tính dựa trên cấu hình đã chọn.",
                Items =
                [
                    "Giá xe theo phiên bản và trang bị.",
                    "Chi phí dự kiến cho phụ kiện đi kèm.",
                    "Liên hệ tư vấn để chốt báo giá chi tiết."
                ]
            }
        ];
    }

    private static void ConfigurePriceListPage()
    {
        if (!PageConfigs.TryGetValue("price-list", out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Bảng giá tham khảo theo từng dòng xe đang bán.",
            "Cập nhật theo chương trình ưu đãi theo thời điểm.",
            "Hỗ trợ tư vấn chi phí lăn bánh theo khu vực."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Giá niêm yết tham khảo",
                Description = "Mức giá có thể thay đổi theo phiên bản và thời điểm.",
                Items =
                [
                    "Giá theo dòng xe và cấu hình tiêu chuẩn.",
                    "Chênh lệch theo màu ngoại thất và tùy chọn thêm.",
                    "Liên hệ để nhận báo giá mới nhất."
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Chi phí dự kiến khi nhận xe",
                Description = "Một số chi phí thường gặp ngoài giá xe.",
                Items =
                [
                    "Lệ phí trước bạ theo địa phương.",
                    "Phí đăng ký biển số và đăng kiểm.",
                    "Bảo hiểm và các khoản dịch vụ liên quan."
                ]
            }
        ];
    }

    private static void ConfigureContactPage()
    {
        if (!PageConfigs.TryGetValue(ContactSlug, out var page))
        {
            return;
        }

        page.Highlights =
        [
            "Hotline luôn sẵn sàng hỗ trợ trong giờ hành chính.",
            "Tư vấn mua xe, dịch vụ và hậu mãi theo nhu cầu thực tế.",
            "Hỗ trợ đặt lịch làm việc nhanh qua hotline hoặc email."
        ];

        page.ContentBlocks =
        [
            new ContentBlockViewModel
            {
                Heading = "1. Thông tin liên hệ",
                Description = "Kết nối trực tiếp với UTC Car qua các kênh chính thức.",
                Items =
                [
                    "Hotline: 0000.000.000",
                    "Email: info@utc-car.vn",
                    "Địa chỉ: 3 Cầu Giấy, Ngọc Khánh, Láng, Hà Nội"
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "2. Thời gian hỗ trợ",
                Description = "Đội ngũ tư vấn tiếp nhận yêu cầu và phản hồi nhanh.",
                Items =
                [
                    "Thứ 2 - Thứ 7: 08:00 - 18:00",
                    "Chủ nhật: 08:00 - 12:00",
                    "Ngoài giờ: để lại thông tin, chúng tôi sẽ liên hệ sớm nhất"
                ]
            },
            new ContentBlockViewModel
            {
                Heading = "3. Mạng xã hội",
                Description = "Theo dõi UTC Car để cập nhật thông tin ưu đãi và sản phẩm mới.",
                Items =
                [
                    "Facebook: https://www.facebook.com",
                    "YouTube: https://www.youtube.com",
                    "Instagram: https://www.instagram.com",
                    "Zalo: https://zalo.me"
                ]
            }
        ];
    }

    private static void ConfigureSecurityAndTestingPages()
    {
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
