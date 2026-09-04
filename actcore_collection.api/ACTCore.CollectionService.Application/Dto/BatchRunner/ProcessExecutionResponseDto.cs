namespace ACTCore.CollectionService.Application.Dto
{
    public class ProcessExecutionReasponseDto
    {
        public int Total { get; set; } = 0;

        public int Success { get; set; } = 0;

        public int Failed { get; set; } = 0;

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

    }
}
