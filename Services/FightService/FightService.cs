using dotnetrpg.Data;
using dotnetrpg.DTOs.Fights;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ServiceResponse<ResultAttackDTO>> WeaponAttack(WeaponAttackDTO attack)
        {
            //generate response
            var response = new ServiceResponse<ResultAttackDTO>();
            try
            {
                var attacker = await _context.Characters
                    .Include(ch => ch.CurrentWeapon)
                    .FirstOrDefaultAsync(ch => ch.Id == attack.AttackerID);
                var opponent = await _context.Characters
                    .Include(ch => ch.CurrentWeapon)
                    .FirstOrDefaultAsync(ch => ch.Id == attack.OponentID);
                if (attacker != null && opponent != null)
                {
                    int dmg = attacker.CurrentWeapon.Damage + (new Random().Next(5,attacker.Strenght));
                    dmg -= new Random().Next(0,opponent.Strenght);

                    

                    if (dmg > 0)
                    {
                        response.Message = $"{attacker.Name} dealed{dmg} damage to {opponent.Name}";
                        opponent.HitPoints -= dmg;
                    }
                    if(opponent.HitPoints == 0){
                        response.Message = $"{opponent.Name} defeated";
                    }

                    await _context.SaveChangesAsync();
                    var attRs = new ResultAttackDTO{
                        AttackerName = attacker.Name,
                        OpponentName = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints,
                        Damage = dmg
                    };
                    response.Data = attRs;
                }else
                {
                    throw new NullReferenceException("Non existant-character");
                }

            }
            catch (Exception err)
            {
                response.SuccessFlag = false;
                response.Message = err.Message.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<ResultAttackDTO>> SkillAttack(SkillAttackDTO attack)
        {
            var response = new ServiceResponse<ResultAttackDTO>();
            try
            {
                var attacker = await _context.Characters
                    .Include(ch => ch.Skills)
                    .FirstOrDefaultAsync(ch => ch.Id == attack.AttackerID);
                var opponent = await _context.Characters
                    .Include(ch => ch.Skills)
                    .FirstOrDefaultAsync(ch => ch.Id == attack.OponentID);
                
                
                if (attacker != null && opponent != null)
                {
                    var sk = attacker.Skills.FirstOrDefault(s => s.Id == attack.SkillId);
                    if (sk is null)
                    {
                        throw new Exception($"{attacker.Name} doesn't know that skill");
                    }
                    int dmg = attacker.CurrentWeapon.Damage + (new Random().Next(5,attacker.Intelligence));
                    dmg -= new Random().Next(0,opponent.Strenght);

                    
                    if (dmg > 0)
                    {
                        response.Message = $"{attacker.Name} dealed {dmg} damage to {opponent.Name}";
                        opponent.HitPoints -= dmg;
                    }else{
                        response.Message = $"{opponent.Name} defelcted the attack";
                    }
                    if(opponent.HitPoints == 0){
                        response.Message = $"{opponent.Name} defeated";
                    }

                    await _context.SaveChangesAsync();
                    var attRs = new ResultAttackDTO{
                        AttackerName = attacker.Name,
                        OpponentName = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints,
                        Damage = dmg
                    };
                    response.Data = attRs;
                }else{
                    throw new NullReferenceException("Non existant-character");
                }
            }
            catch (Exception err)
            {
                
                response.SuccessFlag = false;
                response.Message = err.Message.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<FightResultDTO>> Fight(FightRequestDTO attack)
        {
            var response = new ServiceResponse<FightResultDTO>{
                Data = new FightResultDTO()
            };
            try
            {
                var participants = await _context.Characters
                    .Include(c => c.CurrentWeapon)
                    .Include(c => c.Skills)
                    .Where(c => attack.CharacterIds.Contains(c.Id))
                    .ToListAsync();
                bool defeated = false;
                while(!defeated){
                    foreach (var attck in participants)
                    {
                        var opponents = participants.Where(c => c.Id != attck.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];
                        //TODO: compleate fight logic
                    }
                }
            }
            catch (Exception err)
            {
                response.SuccessFlag = false;
                response.Message = err.Message.ToString();
            }
            return response;
        }


        #endregion
    }
}