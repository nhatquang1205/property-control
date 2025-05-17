using FastEndpoints;
using PropertyControl.Commons;
using PropertyControl.Commons.Schemas;
using PropertyControl.Repositories;

namespace PropertyControl.Features.Auth.Login
{
    public class Endpoint : Endpoint<Request, ResponseInfo>
    {
        public readonly IUserRepository _userRepository;
        public Endpoint(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public override void Configure()
        {
            Post("login");
            AllowAnonymous();
            Description(x => x
                .WithName("Login")
                .Produces<ResponseInfo>(200)
                .Produces(404)
                .Produces(500));

            DontThrowIfValidationFails();
        }

        public override async Task HandleAsync(Request request, CancellationToken ct)
        {
            var result = new ResponseInfo();
            if (ValidationFailed)
            {
                result.StatusCode = 400;
                result.Message = "Validation Failed";
            }

            var user = await _userRepository.GetUserByUsernameAndPassword(request.Username, request.Password);

            if (user == null)
            {
                result.StatusCode = 400;
                result.Message = "User not found";
            }
            else
            {
                var jwtData = new JwtData
                {
                    UserId = user.Id,
                    Username = user.Username,
                    RoleId = user.RoleId,
                };

                var token = Security.GenerateJWTCode(jwtData);
            
                result.Data.Add("token", token);
            }

            await SendAsync(result, 200, cancellation: ct);
        }
    }
}