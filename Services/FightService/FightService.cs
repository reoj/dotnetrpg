using AutoMapper;
using dotnetrpg.Data;
using dotnetrpg.DTOs.Fights;
using Microsoft.EntityFrameworkCore;

namespace dotnetrpg.Services.FightService
{
    public class FightService:IFightService
    {
        #region Private Fields
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        
        #endregion
        #region Constructor
        public FightService(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
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
                    string msg;
                    int dmg;
                    DoWeaponAttack(attacker, opponent, out msg, out dmg);

                    response.Message = msg;
                    if (opponent.HitPoints == 0)
                    {
                        response.Message = $"{opponent.Name} defeated";
                    }

                    await _context.SaveChangesAsync();
                    var attRs = new ResultAttackDTO
                    {
                        AttackerName = attacker.Name,
                        OpponentName = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints,
                        Damage = dmg
                    };
                    response.Data = attRs;
                }
                else
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
                    string msg = string.Empty;
                    var sk = attacker.Skills.FirstOrDefault(s => s.Id == attack.SkillId);
                    if (sk is null)
                    {
                        throw new Exception($"{attacker.Name} doesn't know that skill");
                    }
                    int dmg;
                    DoSkillAttack(attacker, sk, opponent, out msg, out dmg);
                    response.Message = msg;

                    if (opponent.HitPoints == 0)
                    {
                        response.Message = $"{opponent.Name} defeated";
                    }

                    await _context.SaveChangesAsync();
                    var attRs = new ResultAttackDTO
                    {
                        AttackerName = attacker.Name,
                        OpponentName = opponent.Name,
                        AttackerHP = attacker.HitPoints,
                        OpponentHP = opponent.HitPoints,
                        Damage = dmg
                    };
                    response.Data = attRs;
                }
                else
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
                    foreach (var attacker in participants)
                    {
                        var opponents = participants.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int dmg;
                        bool useWeapon = new Random().Next(2) == 0;
                        var msg = string.Empty;

                        if (useWeapon) // Code for using weapon
                        {
                            DoWeaponAttack(attacker,opponent,out msg,out dmg);
                            
                        }else{ //Use Skill
                            
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];
                            DoSkillAttack(attacker,skill,opponent,out msg, out dmg);      
                        }
                        
                        response.Data.Log.Add(msg);

                        if(opponent.HitPoints <= 0)
                        {
                            defeated = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add
                                ($"{opponent.Name} was defeated by {attacker.Name}");
                            break;
                        }
                    }
                }

                participants.ForEach( ch => 
                {
                    ch.Fights++;
                    ch.HitPoints = 100;
                });

                await _context.SaveChangesAsync();
                
            }
            catch (Exception err)
            {
                response.SuccessFlag = false;
                response.Message = err.Message.ToString();
            }
            return response;
        }
        
         public async Task<ServiceResponse<List<HighScoreDTO>>> GetHighScores()
        {
            var response = new ServiceResponse<List<HighScoreDTO>>();

            var participants = await _context.Characters
                .Where(c => c.Fights > 0)
                .OrderByDescending(c => c.Victories)
                .ThenBy(c => c.Defeats)
                .ToListAsync();
            
            response.Data = participants.Select(c=>_mapper.Map<HighScoreDTO>(c)).ToList();
            return response;
        }

        #endregion

        #region Private Methods
         private static void DoSkillAttack
            (Character attacker, Skill skill, Character opponent, out string msg, out int dmg)
        {
            dmg = skill.Damage + (new Random().Next(5, attacker.Intelligence));
            dmg -= new Random().Next(0, opponent.Strenght);
            if (dmg > 0)
            {
                msg = $"{attacker.Name} dealed {dmg} damage to {opponent.Name} using {skill.Name}";
                opponent.HitPoints -= dmg;
            }
            else
            {
                msg = $"{opponent.Name} defelcted the attack";
            }
        }   
        private static void DoWeaponAttack
            (Character attacker, Character opponent, out string msg, out int dmg)
        {
            msg = string.Empty;
            dmg = attacker.CurrentWeapon.Damage + (new Random().Next(5, attacker.Strenght));
            dmg -= new Random().Next(0, opponent.Strenght);

            if (dmg > 0)
            {
                msg = 
                $"{attacker.Name} dealed {dmg} damage to {opponent.Name} with {attacker.CurrentWeapon.Name}";
                opponent.HitPoints -= dmg;
            }else{
                if (attacker.CurrentWeapon.Name == "")
                {
                    msg = 
                    $"{attacker.Name} was disarmed";
                }
                else
                {
                msg = 
                $"{attacker.Name} couldn't use {attacker.CurrentWeapon.Name} effectively";
                }
            }
        }

       
        #endregion
    }
}