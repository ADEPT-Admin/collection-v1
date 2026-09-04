namespace ACTCore.CollectionService.DataImport.Validators
{
    public class ImportValidationResult
    {
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; set; }
        public List<string> CsvFiles { get; set; } = [];
    }
}
