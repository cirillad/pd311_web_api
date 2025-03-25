using FluentValidation;
using pd311_web_api.BLL.DTOs.Role;

namespace pd311_web_api.BLL.DTOs.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public IEnumerable<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }

    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(256).WithMessage("Email must be at most 256 characters long");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required")
                .MinimumLength(3).WithMessage("UserName must be at least 3 characters long")
                .MaximumLength(50).WithMessage("UserName must be at most 50 characters long");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name must be at most 50 characters long");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleForEach(x => x.Roles).SetValidator(new RoleValidator());
        }
    }


    public class RoleValidator : AbstractValidator<RoleDto>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Role ID is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Role name is required")
                .MaximumLength(50).WithMessage("Role name must be at most 50 characters long");
        }
    }


}
