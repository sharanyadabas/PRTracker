using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRTracker.Data;
using PRTracker.Entities;
using PRTracker.Models;

namespace PRTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLiftController : ControllerBase
    {
        private readonly ExerciseDbContext _context;
        private readonly IMapper _mapper;

        public UserLiftController(ExerciseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetUserLiftByID(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var userlift = _context.UserLifts.Where(x => x.Id == id).FirstOrDefault();

                if (userlift == null)
                {
                    response.Status = false;
                    response.Message = "Record Doesn't Exist";

                    return BadRequest(response);
                }

                response.Status = true;
                response.Message = "Success";
                response.Data = userlift;

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

        [HttpGet("exercise/{userId}/{exerciseId}")]
        public IActionResult GetExerciseLift(int userId, int exerciseId)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var userlift = _context.UserLifts.Where(x => x.UserId == userId && x.ExerciseId == exerciseId).ToList();

                if (userlift == null || !userlift.Any())
                {
                    response.Status = false;
                    response.Message = "Record Doesn't Exist";

                    return BadRequest(response);
                }

                response.Status = true;
                response.Message = "Success";
                response.Data = userlift;

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

        [HttpGet("user/{userId}")]
        public IActionResult GetUserLiftByUser(int userId)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var userlift = _context.UserLifts.Where(x => x.UserId == userId).ToList();

                if (userlift == null || !userlift.Any())
                {
                    response.Status = false;
                    response.Message = "Record Doesn't Exist";

                    return BadRequest(response);
                }

                response.Status = true;
                response.Message = "Success";
                response.Data = userlift;

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
        public IActionResult CreateUserLift(CreateUserLiftViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    var postedModel = _mapper.Map<UserLift>(model);

                    _context.UserLifts.Add(postedModel);
                    _context.SaveChanges();

                    var createdModel = _mapper.Map<CreateUserLiftViewModel>(postedModel);

                    response.Status = true;
                    response.Message = "Created UserLift Successfully";
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
        public IActionResult UpdateUserLift(UpdateUserLiftViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid UserLift Id";
                        response.Data = ModelState;

                        return BadRequest(response);
                    }

                    var userLiftDetails = _context.UserLifts.Where(x => x.Id == model.Id).FirstOrDefault();

                    if (userLiftDetails == null)
                    {
                        response.Status = false;
                        response.Message = "Invalid Field";
                        response.Data = ModelState;

                        return BadRequest(response);
                    }


                    if (model.ExerciseId.HasValue)
                    {
                        userLiftDetails.ExerciseId = model.ExerciseId.Value;
                    }

                    if (model.Date.HasValue)
                    {
                        userLiftDetails.Date = model.Date.Value;
                    }                                                
                                                                     
                    if (model.Sets.HasValue)                   
                    {                                                
                        userLiftDetails.Sets = model.Sets.Value;
                    }                                                
                                                                     
                    if (model.Reps.HasValue)                   
                    {                                                
                        userLiftDetails.Reps = model.Reps.Value;
                    }                                                
                                                                     
                    if (model.Weight.HasValue)                   
                    {                                                
                        userLiftDetails.Weight = model.Weight.Value;
                    }

                    if (model.Notes != null)
                    {
                        userLiftDetails.Notes = model.Notes;
                    }
                    
                    _context.SaveChanges();

                    response.Status = true;
                    response.Message = "Updated UserLift Successfully";
                    response.Data = userLiftDetails;

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
        public IActionResult DeleteUserLift(int id)
        {
            BaseResponseModel response = new BaseResponseModel();
            try
            {
                var userlift = _context.UserLifts.Where(x => x.Id == id).FirstOrDefault();

                if (userlift == null)
                {
                    response.Status = false;
                    response.Message = "User Doesn't Exist";

                    return BadRequest(response);
                }


                _context.UserLifts.Remove(userlift);
                _context.SaveChanges();

                response.Status = true;
                response.Message = "User Deleted Successfully";
                response.Data = userlift;

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
    }

}
