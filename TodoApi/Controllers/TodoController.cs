using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace TodoApi.Controllers
{
    [ApiController]
    [Produces(MediaTypeNames.Application.Json, MediaTypeNames.Application.Xml)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class TodoController : ControllerBase
    {
        private readonly TodoDbContext todoDbContext;

        public TodoController(TodoDbContext todoDbContext)
        {
            this.todoDbContext = todoDbContext;
        }

        /// <summary>
        /// Gets all todos.
        /// </summary>
        /// <response code="200">Returns the list of todos.</response>
        [Route("/todo")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Todo>))]
        public IActionResult GetAll()
        {
            throw new EntityNotFoundException();
            return Ok(todoDbContext.Todos);
        }

        /// <summary>
        /// Gets a todo by id.
        /// </summary>
        /// <param name="id">Todo id</param>
        /// <response code="200">Returns the todo.</response>
        /// <response code="404">Todo not found.</response>
        [Route("/todo/{id:int}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Todo))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(void))]
        public IActionResult GetById(int id)
        {
            var todo = todoDbContext.Todos.FirstOrDefault(x => x.Id == id);
            if (todo == null)
                return NotFound();

            return Ok(todo);
        }

        /// <summary>
        /// Creates a new todo.
        /// </summary>
        /// <response code="201">Todo created.</response>
        /// <response code="400">Invalid request.</response>
        [Route("/todo")]
        [HttpPost]
        [Consumes("application/json")] // Request media type for this action
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Todo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(void))]
        public IActionResult Create([FromBody] Todo todo)
        {
            if (todo == null)
                return BadRequest();

            todoDbContext.Todos.Add(todo);
            todoDbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }
    }
}
