using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Base
    {
        public enum ErrorType
        {
            no_error = -1,
            network_cannot_be_reached,
            network_cannot_be_reached_or_cookie_is_invalid_or_expired,
            network_cannot_be_reached_or_session_token_link_is_invalid_or_expired,
            network_cannot_be_reached_or_session_token_is_invalid_or_expired,
            cookie_is_empty,
            schedule_is_not_ready = 1000,
            schedule_cannot_be_resolved,
            stage_cannot_be_resolved = 1100,
            battle_is_not_ready = 2000,
            battles_cannot_be_resolved,
            battle_cannot_be_resolved,
            player_cannot_be_resolved = 2100,
            weapon_cannot_be_resolved = 2200,
            sub_weapon_cannot_be_resolved = 2210,
            special_weapon_cannot_be_resolved = 2220,
            gear_cannot_be_resolved = 2300,
            primary_ability_cannot_be_resolved = 2310,
            secondary_ability_cannot_be_resolved = 2320,
            shift_is_not_ready = 3000,
            shift_cannot_be_resolved,
            shift_stage_cannot_be_resolved = 3100,
            shift_weapon_cannot_be_resolved = 3110,
            job_is_not_ready = 4000,
            jobs_cannot_be_resolved,
            job_cannot_be_resolved,
            wave_cannot_be_resolved = 4100,
            job_player_cannot_be_resolved = 4200,
            job_weapon_cannot_be_resolved = 4300,
            job_special_weapon_cannot_be_resolved = 4310,
            session_token_cannot_be_resolved = 10000,
            cookie_cannot_be_resolved = 10100,
            cookie_cannot_be_resolved_1_7,
            cookie_cannot_be_resolved_2_7,
            cookie_cannot_be_resolved_3_7,
            cookie_cannot_be_resolved_4_7,
            cookie_cannot_be_resolved_5_7,
            cookie_cannot_be_resolved_6_7,
        }

        public ErrorType Error { get; }

        public Base()
        {
            Error = (ErrorType)(-1);
        }
        public Base(ErrorType error)
        {
            Error = error;
        }

        public static ErrorType ParseErrorType(string s)
        {
            return (ErrorType)Enum.Parse(typeof(ErrorType), s);
        }
        public static bool TryParseErrorType(string s, out ErrorType result)
        {
            return Enum.TryParse(s, out result);
        }
    }
}
