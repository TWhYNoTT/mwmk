namespace Utilities
{
    public static class SD
    {
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";
        public const string MarketerRole = "Marketer";
        public const string Pending = "Pending";//حالة انتظار، تُستخدم لوصف طلب لم يتم معالجته أو الموافقة عليه بعد.

        public const string Approve = "Approved";// حالة مقبول، تُستخدم لوصف طلب تمت الموافقة عليه.
        public const string Proccessing = "Proccessing";// حالة قيد المعالجة، تُستخدم لوصف طلب قيد التجهيز أو التحضير.
        public const string Cancelled = "Cancelled";//Admin حالة ملغى، تُستخدم لوصف طلب تم إلغاؤه من قبل .
        public const string Recycling = "Recycling";// اعاده تدوير المنتج
        public const string Replacing = "Replacing";//استبدال المنتج باخر للمقاس المناسب
        public const string Shipped = "Shipped";// حالة تم الشحن، تُستخدم لوصف طلب تم إرساله إلى العميل.
        public const string SessionKey = "ShopingCartSession";
    }
}
