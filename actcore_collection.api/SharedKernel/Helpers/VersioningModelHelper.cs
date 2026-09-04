using SharedKernel.Models;

namespace SharedKernel.Helpers
{
    public static class VersioningModelHelper
    {
        public static void SetCreatedAudit<T>(T model, string userName) where T : VersionBaseModel
        {
            var now = DateTime.Now;
            model.CreatedBy = userName;
            model.CreatedDate = now;
            model.UpdatedBy = userName;
            model.UpdatedDate = now;
        }

        public static void SetUpdatedAudit<T>(T model, string userName) where T : VersionBaseModel
        {
            model.UpdatedBy = userName;
            model.UpdatedDate = DateTime.Now;
        }
    }
}
