using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PRTracker.Data;
using PRTracker.Entities;
using PRTracker.Models;

namespace PRTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ExerciseDbContext _context;
        private readonly IMapper _mapper;

        public UserController(ExerciseDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var userCount = _context.Users.Count();
                var userList = _context.Users.ToList();

                response.Status = true;
                response.Message = "Success";
                response.Data = new { Users = userList, Count = userCount };

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
        public IActionResult GetUserByID(int id)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();

                if (user == null)
                {
                    response.Status = false;
                    response.Message = "Record Doesn't Exist";

                    return BadRequest(response);
                }

                response.Status = true;
                response.Message = "Success";
                response.Data = user;

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
        public IActionResult CreateUser(CreateUserViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    var postedModel = _mapper.Map<User>(model);
                    postedModel.UserLifts = new List<UserLift>();

                    _context.Users.Add(postedModel);
                    _context.SaveChanges();

                    var createdModel = _mapper.Map<CreateUserViewModel>(postedModel);

                    response.Status = true;
                    response.Message = "Created User Successfully";
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
        public IActionResult UpdateUser(UpdateUserViewModel model)
        {
            BaseResponseModel response = new BaseResponseModel();

            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Id <= 0)
                    {
                        response.Status = false;
                        response.Message = "Invalid User Id";
                        response.Data = ModelState;

                        return BadRequest(response);
                    }

                    var userDetails = _context.Users.Where(x => x.Id == model.Id).FirstOrDefault();

                    if (userDetails == null)
                    {
                        response.Status = false;
                        response.Message = "Invalid Field";
                        response.Data = ModelState;

                        return BadRequest(response);
                    }

                    if (model.UserName != null)
                    {
                        userDetails.UserName = model.UserName;
                    }

                    if (model.Email != null)
                    {
                        userDetails.Email = model.Email;
                    }

                    if (model.PasswordHash != null)
                    {
                        userDetails.PasswordHash = model.PasswordHash;
                    }

                    userDetails.ModifiedDate = DateTime.UtcNow;

                    _context.SaveChanges();

                    response.Status = true;
                    response.Message = "Updated Exercise Successfully";
                    response.Data = userDetails;

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
        public IActionResult DeleteUser(int id)
        {
            BaseResponseModel response = new BaseResponseModel();
            try
            {
                var user = _context.Users.Where(x => x.Id == id).FirstOrDefault();

                if (user == null)
                {
                    response.Status = false;
                    response.Message = "User Doesn't Exist";

                    return BadRequest(response);
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                response.Status = true;
                response.Message = "User Deleted Successfully";
                response.Data = user;

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
