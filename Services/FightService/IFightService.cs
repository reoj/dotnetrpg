using dotnetrpg.DTOs.Fights;

namespace dotnetrpg.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<ResultAttackDTO>> WeaponAttack (WeaponAttackDTO attack);
    }
}