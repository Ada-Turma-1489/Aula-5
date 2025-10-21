namespace TodoApi
{
    /// <summary>
    /// Represents a Todo object
    /// </summary>
    public class Todo
    {
        /// <summary>
        /// The Todo is
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The Todo Title
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// The Todo Priority
        /// </summary>
        public Prioridade Prioridade { get; set; }
    }
}