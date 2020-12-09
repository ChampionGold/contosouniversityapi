using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.DTOS;
using TodoAPI.DTOS.Converters;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class ApiController : Controller
    {
        //Injection dependency para la contextdb de ContosoUniversity
        private readonly ContosoUniversityContext _context;

        public ApiController(ContosoUniversityContext context)
        {
            _context = context;
        }

        // GET: todos los estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetStudents()
        {
            return await _context.Students
                .Select(x => PersonToDTO.StudentToDTO(x))
                .ToListAsync();
        }

        // GET: estudiante en especifico
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetStudent(int id)
        {
            var todoItem = await _context.Students.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return PersonToDTO.StudentToDTO(todoItem);
        }


        //Hacer upd a estudiante en especifico
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }

            var currentPerson = await _context.Students.FindAsync(id);

            if (currentPerson == null)
            {
                return NotFound();
            }

            currentPerson.FirstName = person.FirstName;
            currentPerson.LastName = person.LastName;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_context.Students.Any(s => s.Id == id))
            {
                return NotFound();
            }

            return NoContent();
        }

        //Crear Estudiante
        [HttpPost]
        public async Task<ActionResult<Person>> CreateStudent(Person person)
        {
            var newPerson = new Student
            {
                FirstName = person.FirstName,
                LastName = person.LastName
            };

            _context.Students.Add(newPerson);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = newPerson.Id },
                PersonToDTO.StudentToDTO(newPerson));
        }

        //Borrar estudiante
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var exstudent = await _context.Students.FindAsync(id);

            if (exstudent == null)
            {
                return NotFound();
            }

            _context.Students.Remove(exstudent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
