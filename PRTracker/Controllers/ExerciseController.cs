using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRTracker.Data;
using PRTracker.Entities;
using PRTracker.Models;

namespace PRTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseDbContext _context;

        public ExerciseController(ExerciseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllExercises()
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var exerciseCount = _context.Exercises.Count();
                var exerciseList = _context.Exercises.ToList();

                response.Status = true;
                response.Message = "Success";
                response.Data = new { Exercises = exerciseList, Count = exerciseCount };

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetExerciseByID(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var exercise = _context.Exercises.Where(x => x.Id == id).FirstOrDefault();

                if (exercise == null)
                {
                    response.Status = false;
                    response.Message = "Record Doesn't Exist";

                    return BadRequest(response);
                }

                response.Status = true;
                response.Message = "Success";
                response.Data = exercise;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpPost]
        public IActionResult CreateExercise(CreateExerciseViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    var postedModel = new Exercise()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Instructions = model.Instructions,
                        Images = model.Images,
                    };

                    _context.Exercises.Add(postedModel);
                    _context.SaveChanges();

                    response.Status = true;
                    response.Message = "Created Exercise Successfully";
                    response.Data = postedModel;

                    return Ok(response);
                }
                else
                {
                    response.Status = false;
                    response.Message = "Invalid Field";
                    response.Data = ModelState;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }

        [HttpPut]
        public IActionResult UpdateExercise(CreateExerciseViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid Exercise Id";
                        response.Data = ModelState;

                        return BadRequest(response);
                    }

                    var exerciseDetails = _context.Exercises.Where(x => x.Id == model.Id).FirstOrDefault();

                    if (exerciseDetails == null)
                    {
                        response.Status = false;
                        response.Message = "Invalid Field";
                        response.Data = ModelState;

                        return BadRequest(response);
                    }


                    exerciseDetails.Name = model.Name;
                    exerciseDetails.Description = model.Description;
                    exerciseDetails.Instructions = model.Instructions;
                    exerciseDetails.Images = model.Images;

                    var postedModel = new Exercise()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Instructions = model.Instructions,
                        Images = model.Images,
                    };

                    _context.Exercises.Add(postedModel);
                    _context.SaveChanges();

                    response.Status = true;
                    response.Message = "Created Exercise Successfully";
                    response.Data = postedModel;

                    return Ok(response);
                }
                else
                {
                    response.Status = false;
                    response.Message = "Invalid Field";
                    response.Data = ModelState;

                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";

                return BadRequest(response);
            }
        }
    }
}
