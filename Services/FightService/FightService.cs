using dotnetrpg.Data;
using dotnetrpg.DTOs.Fights;

namespace dotnetrpg.Services.FightService
{
    public class FightService:IFightService
    {
        private readonly DataContext _context;
        #region Constructor
        public FightService(DataContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public Task<ServiceResponse<ResultAttackDTO>> WeaponAttack(WeaponAttackDTO attack)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}