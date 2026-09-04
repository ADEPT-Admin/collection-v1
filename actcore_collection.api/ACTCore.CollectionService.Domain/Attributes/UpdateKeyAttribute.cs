
namespace ACTCore.CollectionService.Domain.Attributes
{
    /// <summary>
    /// ระบุ Property ที่ใช้เป็น Business Key สำหรับ Update
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UpdateKeyAttribute : Attribute
    {
        public int Order { get; set; }

        public UpdateKeyAttribute(int order = 0)
        {
            Order = order;
        }
    }
}
