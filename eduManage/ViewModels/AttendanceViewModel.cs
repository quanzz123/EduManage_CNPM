namespace eduManage.ViewModels
{
    public class AttendanceViewModel
    {
        public int SessionId { get; set; }
        public int classId { get; set; }
        public List<AttendanceItem> Students { get; set; }
    }
}
