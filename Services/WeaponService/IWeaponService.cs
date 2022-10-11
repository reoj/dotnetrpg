using dotnetrpg.DTOs.Character;
using dotnetrpg.DTOs.Weapon;

namespace dotnetrpg.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO nwWeapon);
    }
}