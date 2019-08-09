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
            cookie_is_empty,
            schedule_is_not_ready = 100,
            schedule_cannot_be_resolved,
            battle_is_not_ready = 200,
            battles_cannot_be_resolved,
            battle_cannot_be_resolved,
            player_cannot_be_resolved = 300,
            weapon_cannot_be_resolved = 400,
            sub_weapon_cannot_be_resolved = 500,
            special_weapon_cannot_be_resolved = 600,
            gear_cannot_be_resolved = 700,
            primary_ability_cannot_be_resolved = 800,
            secondary_ability_cannot_be_resolved = 900
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
