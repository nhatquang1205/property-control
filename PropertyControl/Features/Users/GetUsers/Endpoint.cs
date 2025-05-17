using FastEndpoints;
using PropertyControl.Databases.Entities;
using PropertyControl.Repositories;

namespace PropertyControl.Features.Users.GetUsers
{
    public class Endpoint : EndpointWithoutRequest<List<UserResponse>>
    {
        public readonly IUserRepository _userRepository;
        public Endpoint(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public override void Configure()
        {
            Get("users");
            Description(x => x
                .WithName("Get Users")
                .Produces<List<UserResponse>>(200)
                .Produces(404)
                .Produces(500));

            DontThrowIfValidationFails();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var users = (await _userRepository.GetAllUsers()).Select(x => new UserResponse
            {
                Age = x.Age,
                Username = x.Username
            }).ToList();

            await SendAsync(users, 200, ct);
        }
    }
}