namespace ProductRestApi.Common.Logging;

public static class LoggingTemplates
{
    public const string ProductNotFoundError = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Ürün bulunamadı.";
    public const string ProductDeletedSuccessfully = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Product deleted successfully.";
    public const string InvalidIdError = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Id is invalid.";
    public const string ValidationError = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Validation Error.";
    public const string ProductNameDuplicateError = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Product Name is not valid.";
    public const string ProductUpdated = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Product updated successfully.";
    public const string ProductFoundButDeletionError = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Product found but an error occurred during deletion.";
    public const string ProductsFound = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Products found.";
    public const string ProductCreatedSuccessfully = $"{{@{LoggingMessageTemplate.ProductLogModel}}} Ürün başarıyla oluşturuldu.";
    
    
}
