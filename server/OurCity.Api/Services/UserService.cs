using OurCity.Api.Common;
using OurCity.Api.Infrastructure;
using OurCity.Api.Common.Dtos.User;
using OurCity.Api.Services.Mappings;

namespace OurCity.Api.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetUsers();
    Task<Result<UserResponseDto>> GetUserById(int id);
    Task<Result<UserResponseDto>> CreateUser(UserRequestDto userRequestDto);
    Task<Result<UserResponseDto>> UpdateUser(int id, UserPutRequestDto userRequestDto);
    Task<Result<UserResponseDto>> DeleteUser(int id);
}

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsers()
    {
        return (await _userRepository.GetAllUsers()).ToDtos();
    }

    public async Task<Result<UserResponseDto>> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return Result<UserResponseDto>.Failure("User not found.");
        }
        return Result<UserResponseDto>.Success(user.ToDto());
    }

    public async Task<Result<UserResponseDto>> CreateUser(UserRequestDto userRequestDto)
    {
        var createdUser = await _userRepository.CreateUser(userRequestDto.ToEntity());
        return Result<UserResponseDto>.Success(createdUser.ToDto());
    }

    public async Task<Result<UserResponseDto>> UpdateUser(int id, UserPutRequestDto userPutRequestDto)
    {
        // check if the user id exists in db
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser == null)
        {
            return Result<UserResponseDto>.Failure("User not found.");
        }

        // update the user fields
        if (userPutRequestDto.Username != null)
        {
            existingUser.Username = userPutRequestDto.Username;
        }
        if (userPutRequestDto.Email != null)
        {
            existingUser.Email = userPutRequestDto.Email;
        }
        if (userPutRequestDto.DisplayName != null)
        {
            existingUser.DisplayName = userPutRequestDto.DisplayName;
        }
        existingUser.UpdatedAt = DateTime.UtcNow;

        // persist updates to User
        var updatedUser = await _userRepository.UpdateUser(existingUser);
        return Result<UserResponseDto>.Success(updatedUser.ToDto());
    }

    public async Task<Result<UserResponseDto>> DeleteUser(int id)
    {
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser == null)
        {
            return Result<UserResponseDto>.Failure("User not found.");
        }

        await _userRepository.DeleteUser(id);
        return Result<UserResponseDto>.Success(existingUser.ToDto());
    }
}
