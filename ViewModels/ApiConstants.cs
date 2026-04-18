namespace CarSales.Web.ViewModels
{
    public static class ApiMessages
    {
        // Car Messages
        public const string CarListFetchedSuccessfully = "Lấy danh sách xe thành công";
        public const string CarDetailFetchedSuccessfully = "Lấy chi tiết xe thành công";
        public const string CarCreatedSuccessfully = "Thêm xe thành công";
        public const string CarUpdatedSuccessfully = "Cập nhật xe thành công";
        public const string CarDeletedSuccessfully = "Xóa xe thành công";

        // Brand Messages
        public const string BrandListFetchedSuccessfully = "Lấy danh sách hãng xe thành công";
        public const string BrandDetailFetchedSuccessfully = "Lấy chi tiết hãng xe thành công";
        public const string BrandCreatedSuccessfully = "Thêm hãng xe thành công";
        public const string BrandUpdatedSuccessfully = "Cập nhật hãng xe thành công";
        public const string BrandDeletedSuccessfully = "Xóa hãng xe thành công";

        // Deposit Messages
        public const string DepositListFetchedSuccessfully = "Lấy danh sách đặt cọc thành công";
        public const string DepositDetailFetchedSuccessfully = "Lấy chi tiết đặt cọc thành công";
        public const string DepositCreatedSuccessfully = "Tạo đặt cọc thành công";
        public const string DepositConfirmedSuccessfully = "Xác nhận đặt cọc thành công";
        public const string DepositDeletedSuccessfully = "Xóa đặt cọc thành công";

        // PurchaseRequest Messages
        public const string PurchaseRequestListFetchedSuccessfully = "Lấy danh sách yêu cầu mua thành công";
        public const string PurchaseRequestDetailFetchedSuccessfully = "Lấy chi tiết yêu cầu mua thành công";
        public const string PurchaseRequestCreatedSuccessfully = "Tạo yêu cầu mua thành công";
        public const string PurchaseRequestStatusUpdatedSuccessfully = "Cập nhật trạng thái yêu cầu mua thành công";
        public const string PurchaseRequestDeletedSuccessfully = "Xóa yêu cầu mua thành công";

        // Common Error Messages
        public const string InvalidData = "Dữ liệu không hợp lệ";
        public const string ResourceNotFound = "Không tìm thấy tài nguyên";
        public const string CarNotFound = "Không tìm thấy xe";
        public const string BrandNotFound = "Không tìm thấy hãng xe";
        public const string DepositNotFound = "Không tìm thấy đặt cọc";
        public const string PurchaseRequestNotFound = "Không tìm thấy yêu cầu mua";
        public const string UnauthorizedAction = "Bạn không có quyền thực hiện hành động này";
        public const string ServerError = "Lỗi server, vui lòng thử lại sau";
    }
}