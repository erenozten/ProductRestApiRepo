namespace ProductRestApi.Common.Logging;

public static class LoggingTemplates
{
    public const string ProductNotFoundError = "{@ProductLogModel} Ürün bulunamadı.";
    public const string ProductDeletedSuccessfully = "{@ProductLogModel} Product deleted successfully.";
    public const string InvalidIdError = "{@ProductLogModel} Id is invalid.";
    public const string ValidationError = "{@ProductLogModel} Validation Error.";
    public const string ProductNameDuplicateError = "{@ProductLogModel} Product Name is not valid.";
    public const string ProductUpdated = "{@ProductLogModel} Product updated successfully.";
    public const string ProductFoundButDeletionError = "{@ProductLogModel} Product found but an error occurred during deletion.";
    
    
    
}
