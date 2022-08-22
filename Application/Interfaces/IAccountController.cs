using Application.DTOs;

using Domain;

using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces;

public interface IAccountController
{
    Task<Tuple<string,string>> CheckIfUserPropsAreTaken(AppRegisterDto registerDto);
    Task SetRefreshToken(AppUser appUser);
}