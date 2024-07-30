using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRTracker.Data;
using PRTracker.Entities;
using PRTracker.Models;
using System.Net.Http.Headers;

namespace PRTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseDbContext _context;
        private readonly IMapper _mapper;

        public ExerciseController(ExerciseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                response.Data = ex;

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
                response.Data = ex;

                return BadRequest(response);
            }
        }

        [HttpGet("Search/{searchText}")]
        public IActionResult SearchExercises(string searchText)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var searchedExercise = _context.Exercises.Where(x => x.Name.Contains(searchText)).Select(x => new
                {
                    x.Id,
                    x.Name
                }).ToList();

                response.Status = true;
                response.Message = "Success";
                response.Data = searchedExercise;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";
                response.Data = ex;

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
                    var postedModel = _mapper.Map<Exercise>(model);
                    postedModel.UserLifts = new List<UserLift>();

                    _context.Exercises.Add(postedModel);
                    _context.SaveChanges();

                    var createdModel = _mapper.Map<CreateExerciseViewModel>(postedModel);

                    response.Status = true;
                    response.Message = "Created Exercise Successfully";
                    response.Data = createdModel;

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
                response.Data = ex;

                return BadRequest(ex);
            }
        }

        [HttpPut]
        public IActionResult UpdateExercise(UpdateExerciseViewModel model)
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


                    if (model.Name != null)
                    {
                        exerciseDetails.Name = model.Name;
                    }

                    if (model.Description != null)
                    {
                        exerciseDetails.Description = model.Description;
                    }

                    if (model.Instructions != null)
                    {
                        exerciseDetails.Instructions = model.Instructions;
                    }

                    if (model.Images != null && model.Images.Any())
                    {
                        exerciseDetails.Images = model.Images;
                    }

                    exerciseDetails.ModifiedDate = DateTime.UtcNow;

                    _context.Exercises.Update(exerciseDetails);
                    _context.SaveChanges();

                    response.Status = true;
                    response.Message = "Updated Exercise Successfully";
                    response.Data = exerciseDetails;

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
                response.Data = ex;

                return BadRequest(response);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteExercise(int id)
        {
            BaseResponseModel response = new BaseResponseModel();
            try
            {
                var exercise = _context.Exercises.Where(x => x.Id == id).FirstOrDefault();

                if (exercise == null)
                {
                    response.Status = false;
                    response.Message = "Exercise Doesn't Exist";

                    return BadRequest(response);
                }


                _context.Exercises.Remove(exercise);
                _context.SaveChanges();
                
                response.Status = true;
                response.Message = "Exercise Deleted Successfully";
                response.Data = exercise;

                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";
                response.Data = ex;

                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("upload-exercise-image")]
        public async Task<IActionResult> UploadExerciseImage(IFormFile imageFile)
        {

            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var filename = ContentDispositionHeaderValue.Parse(imageFile.ContentDisposition).FileName.TrimStart('\"').TrimEnd('\"');
                string newPath = @"C:\to-delete\";

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string[] allowedImagesExtensions = new string[] { ".jpg", ".jpeg", ".png" };

                if (!allowedImagesExtensions.Contains(Path.GetExtension(filename)))
                {
                    response.Status = false;
                    response.Message = "Only jpg, jpeg, and png files are allowed";

                    return BadRequest(response);
                }

                string newFileName = Guid.NewGuid() + Path.GetExtension(filename);
                string fullFilePath = Path.Combine(newPath, newFileName);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return Ok(new { ProfileImage = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/StaticFiles/{newFileName}" });
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.Message = "Something went wrong";
                response.Data = ex;

                return BadRequest(response);
            }
        }
    }
}
