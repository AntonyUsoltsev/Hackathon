namespace HrDirector.DataBase.Dto
{
    public class WishlistEntity(int employeeId, int chosenEmployeeId, int rank, int hackathonId)
    {
        public int Id { get; set; }
        public int HackathonId { get; set; } = hackathonId;
        public HackathonEntity? Hackathon { get; set; }
        public int EmployeeId { get; set; } = employeeId;
        public EmployeeEntity? Employee { get; set; }
        public int ChosenEmployeeId { get; set; } = chosenEmployeeId;
        public EmployeeEntity? ChosenEmployee { get; set; }
        public int Rank { get; set; } = rank;
    }
}