namespace ACTCore.CollectionService.API.Models.Dto
{
    public class PersonResponseDto
    {
        public string PersonType { get; set; }
        public string FullName { get; set; }
        public string IdCard { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string Occupation { get; set; }
        public decimal? Income { get; set; }
        public string WorkPlace { get; set; }
        public string Email { get; set; }
        public string Relationship { get; set; }
    }
}
