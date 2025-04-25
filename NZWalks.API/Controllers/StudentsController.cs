using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        //GET: https://localhost:portnumber/api/students
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "aima", "ella", "aj", "gabriel", "sophia" };
            return Ok(studentNames);
        }
    }
}
