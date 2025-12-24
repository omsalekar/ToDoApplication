namespace ToDoApi.DTO
{
    public class TaskAddDTO
    {
        public int Id { get; set; }
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
    }
}
