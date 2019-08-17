﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ikas.Class
{
    public class Brand
    {
        public enum Key
        {
            squidforce = 0,
            zink = 1,
            krak_on = 2,
            rockenberg = 3,
            zekko = 4,
            forge = 5,
            firefin = 6,
            skalop = 7,
            splash_mob = 8,
            inkline = 9,
            tentatek = 10,
            takoroka = 11,
            annaki = 15,
            enperry = 16,
            toni_kensa = 17,
            grizzco = 97,
            cuttlegear = 98,
            amiibo = 99
        }

        public Key Id { get; }
        public string Image { get; }

        public Brand(Key id, string image)
        {
            Id = id;
            Image = image;
        }
    }

    public class Ability
    {
        public int Id { get; }
        public string Image { get; }

        public Ability(int id, string image)
        {
            Id = id;
            Image = image;
        }
    }

    public class PrimaryAbility : Ability
    {
        public enum Key
        {
            ink_saver_main = 0,
            ink_saver_sub = 1,
            ink_recovery_up = 2,
            run_speed_up = 3,
            swim_speed_up = 4,
            special_charge_up = 5,
            special_saver = 6,
            special_power_up = 7,
            quick_respawn = 8,
            quick_super_jump = 9,
            sub_power_up = 10,
            ink_resistance_up = 11,
            bomb_defense_up = 12,
            cold_blooded = 13,
            opening_gambit = 100,
            last_ditch_effort = 101,
            tenacity = 102,
            comeback = 103,
            ninja_squid = 104,
            haunt = 105,
            thermal_ink = 106,
            respawn_punisher = 107,
            ability_doubler = 108,
            stealth_jump = 109,
            object_shredder = 110,
            drop_roller = 111,
            bomb_defense_up_dx = 200,
            main_power_up = 201
        }

        public PrimaryAbility(Key id, string image) : base((int)id, image)
        {

        }
    }

    public class SecondaryAbility : Ability
    {
        public enum Key
        {
            ink_saver_main = 0,
            ink_saver_sub = 1,
            ink_recovery_up = 2,
            run_speed_up = 3,
            swim_speed_up = 4,
            special_charge_up = 5,
            special_saver = 6,
            special_power_up = 7,
            quick_respawn = 8,
            quick_super_jump = 9,
            sub_power_up = 10,
            ink_resistance_up = 11,
            bomb_defense_up = 12,
            cold_blooded = 13,
            bomb_defense_up_dx = 200,
            main_power_up = 201,
            none = 255 // はてな
        }

        public SecondaryAbility(Key id, string image) : base((int)id, image)
        {

        }
    }

    public class Gear
    {
        public enum KindType
        {
            Head,
            Clothes,
            Shoes
        }

        public KindType Kind { get; }
        public int Id { get; }
        public string Name { get; }
        public Brand Brand { get; }
        public PrimaryAbility PrimaryAbility { get; }
        public List<SecondaryAbility> SecondaryAbilities { get; }
        public string Image { get; }

        public Gear(KindType kind, int id, string name, Brand brand, PrimaryAbility primaryAbility, List<SecondaryAbility> secondaryAbilities, string image)
        {
            Kind = kind;
            Id = id;
            Name = name;
            Brand = brand;
            PrimaryAbility = primaryAbility;
            SecondaryAbilities = secondaryAbilities;
            Image = image;
        }
    }

    public class HeadGear : Gear
    {
        public enum Key
        {
            white_headband = 1,
            urchins_cap = 1000,
            lightweight_cap = 1001,
            takoroka_mesh = 1002,
            streetstyle_cap = 1003,
            squid_stitch_cap = 1004,
            squidvader_cap = 1005,
            camo_mesh = 1006,
            five_panel_cap = 1007,
            zekko_mesh = 1008,
            backwards_cap = 1009,
            two_stripe_mesh = 1010,
            jet_cap = 1011,
            cycling_cap = 1012,
            cycle_king_cap = 1014,
            long_billed_cap = 1018,
            king_flip_mesh = 1019,
            hickory_work_cap = 1020,
            woolly_urchins_classic = 1021,
            jellyvader_cap = 1023,
            house_tag_denim_cap = 1024,
            blowfish_newsie = 1025,
            do_rag_cap_and_glasses = 1026,
            pilot_hat = 1027,
            bobble_hat = 2000,
            short_beanie = 2001,
            striped_beanie = 2002,
            sporty_bobble_hat = 2003,
            special_forces_beret = 2004,
            squid_nordic = 2005,
            sennyu_bon_bon_beanie = 2006,
            knitted_hat = 2008,
            annaki_beret = 2009,
            yamagiri_beanie = 2010,
            sneaky_beanie = 2011,
            retro_specs = 3000,
            splash_goggles = 3001,
            pilot_goggles = 3002,
            tinted_shades = 3003,
            black_arrowbands = 3004,
            snorkel_mask = 3005,
            white_arrowbands = 3006,
            fake_contacts = 3007,
            _18k_aviators = 3008,
            full_moon_glasses = 3009,
            octoglasses = 3010,
            half_rim_glasses = 3011,
            double_egg_shades = 3012,
            zekko_cap = 3013,
            sv925_circle_shades = 3014,
            annaki_beret_and_glasses = 3015,
            swim_goggles = 3016,
            ink_guard_goggles = 3017,
            toni_kensa_goggles = 3018,
            sennyu_goggles = 3019,
            sennyu_specs = 3020,
            safari_hat = 4000,
            jungle_hat = 4001,
            camping_hat = 4002,
            blowfish_bell_hat = 4003,
            bamboo_hat = 4004,
            straw_boater = 4005,
            classic_straw_boater = 4006,
            treasure_hunter = 4007,
            bucket_hat = 4008,
            patched_hat = 4009,
            tulip_parasol = 4010,
            fugu_bell_hat = 4011,
            seashell_bamboo_hat = 4012,
            hothouse_hat = 4013,
            mountie_hat = 4014,
            studio_headphones = 5000,
            designer_headphones = 5001,
            noise_cancelers = 5002,
            squidfin_hook_cans = 5003,
            squidlife_headphones = 5004,
            studio_octophones = 5005,
            sennyu_headphones = 5006,
            golf_visor = 6000,
            fishfry_visor = 6001,
            sun_visor = 6002,
            takoroka_visor = 6003,
            face_visor = 6004,
            bike_helmet = 7000,
            stealth_goggles = 7002,
            skate_helmet = 7004,
            visor_skate_helmet = 7005,
            mtb_helmet = 7006,
            hockey_helmet = 7007,
            matte_bike_helmet = 7008,
            octo_tackle_helmet_deco = 7009,
            moist_ghillie_helmet = 7010,
            deca_tackle_visor_helmet = 7011,
            gas_mask = 8000,
            paintball_mask = 8001,
            paisley_bandana = 8002,
            skull_bandana = 8003,
            painters_mask = 8004,
            annaki_mask = 8005,
            octoking_facemask = 8006,
            squid_facemask = 8007,
            firefin_facemask = 8008,
            king_facemask = 8009,
            motocross_nose_guard = 8010,
            forge_mask = 8011,
            digi_camo_forge_mask = 8012,
            koshien_bandana = 8013,
            b_ball_headband = 9001,
            squash_headband = 9002,
            tennis_headband = 9003,
            jogging_headband = 9004,
            soccer_headband = 9005,
            fishfry_biscuit_bandana = 9007,
            black_fishfry_bandana = 9008,
            kaiser_cuff = 10000,
            headlamp_helmet = 21000,
            dust_blocker_2000 = 21001,
            welding_mask = 21002,
            beekeeper_hat = 21003,
            octoleet_goggles = 21004,
            cap_of_legend = 21005,
            oceanic_hard_hat = 21006,
            workers_head_towel = 21007,
            workers_cap = 21008,
            sailor_cap = 21009,
            mecha_head_htr = 22000,
            kyonshi_hat = 24000,
            lil_devil_horns = 24001,
            hockey_mask = 24002,
            anglerfish_mask = 24003,
            festive_party_cone = 24004,
            new_years_glasses_dx = 24005,
            twisty_headband = 24006,
            eel_cake_hat = 24007,
            purple_novelty_visor = 24008,
            green_novelty_visor = 24009,
            orange_novelty_visor = 24010,
            pink_novelty_visor = 24011,
            jetflame_crest = 24012,
            fierce_fishskull = 24013,
            hivemind_antenna = 24014,
            eye_of_justice = 24015,
            squid_hairclip = 25000,
            samurai_helmet = 25001,
            power_mask = 25002,
            squid_clip_ons = 25003,
            squinja_mask = 25004,
            power_mask_mk_i = 25005,
            pearlescent_crown = 25006,
            marinated_headphones = 25007,
            enchanted_hat = 25008,
            steel_helm = 25009,
            fresh_fish_head = 25010,
            hero_headset_replica = 27000,
            armor_helmet_replica = 27004,
            hero_headphones_replica = 27101,
            octoling_shades = 27104,
            null_visor_replica = 27105,
            old_timey_hat = 27106,
            conductor_cap = 27107,
            golden_toothpick = 27108,
        }

        public HeadGear(Key id, string name, Brand brand, PrimaryAbility primaryAbility, List<SecondaryAbility> secondaryAbilities, string image) : base(KindType.Head, (int)id, name, brand, primaryAbility, secondaryAbilities, image)
        {

        }
    }

    public class ClothesGear : Gear
    {
        public enum Key
        {
            basic_tee = 2,
            fresh_octo_tee = 3,
            white_tee = 1000,
            black_squideye = 1001,
            sky_blue_squideye = 1003,
            rockenberg_white = 1004,
            rockenberg_black = 1005,
            black_tee = 1006,
            sunny_day_tee = 1007,
            rainy_day_tee = 1008,
            reggae_tee = 1009,
            fugu_tee = 1010,
            mint_tee = 1011,
            grape_tee = 1012,
            red_vector_tee = 1013,
            gray_vector_tee = 1014,
            blue_peaks_tee = 1015,
            ivory_peaks_tee = 1016,
            squid_stitch_tee = 1017,
            pirate_stripe_tee = 1018,
            sailor_stripe_tee = 1019,
            white_8_bit_fishfry = 1020,
            black_8_bit_fishfry = 1021,
            white_anchor_tee = 1022,
            black_anchor_tee = 1023,
            carnivore_tee = 1026,
            pearl_tee = 1027,
            octo_tee = 1028,
            herbivore_tee = 1029,
            black_v_neck_tee = 1030,
            white_deca_logo_tee = 1031,
            half_sleeve_sweater = 1032,
            king_jersey = 1033,
            gray_8_bit_fishfry = 1034,
            white_v_neck_tee = 1035,
            white_urchin_rock_tee = 1036,
            black_urchin_rock_tee = 1037,
            wet_floor_band_tee = 1038,
            squid_squad_band_tee = 1039,
            navy_deca_logo_tee = 1040,
            mister_shrug_tee = 1041,
            chirpy_chips_band_tee = 1042,
            hightide_era_band_tee = 1043,
            red_v_neck_limited_tee = 1044,
            green_v_neck_limited_tee = 1045,
            omega_3_tee = 1046,
            annaki_polpo_pic_tee = 1047,
            firewave_tee = 1048,
            takoroka_galactic_tie_dye = 1049,
            takoroka_rainbow_tie_dye = 1050,
            missus_shrug_tee = 1051,
            league_tee = 1052,
            friend_tee = 1053,
            tentatek_slogan_tee = 1054,
            icewave_tee = 1055,
            octoking_hk_jersey = 1056,
            dakro_nana_tee = 1057,
            dakro_golden_tee = 1058,
            black_velour_octoking_tee = 1059,
            swc_logo_tee = 1061,
            green_velour_octoking_tee = 1060,
            white_striped_ls = 2000,
            black_ls = 2001,
            purple_camo_ls = 2002,
            navy_striped_ls = 2003,
            zekko_baseball_ls = 2004,
            varsity_baseball_ls = 2005,
            black_baseball_ls = 2006,
            white_baseball_ls = 2007,
            white_ls = 2008,
            green_striped_ls = 2009,
            squidmark_ls = 2010,
            zink_ls = 2011,
            striped_peaks_ls = 2012,
            pink_easy_stripe_shirt = 2013,
            inkopolis_squaps_jersey = 2014,
            annaki_drive_tee = 2015,
            lime_easy_stripe_shirt = 2016,
            annaki_evolution_tee = 2017,
            zekko_long_carrot_tee = 2018,
            zekko_long_radish_tee = 2019,
            black_cuttlegear_ls = 2020,
            takoroka_crazy_baseball_ls = 2021,
            red_cuttlegear_ls = 2022,
            khaki_16_bit_fishfry = 2023,
            blue_16_bit_fishfry = 2024,
            white_layered_ls = 3000,
            yellow_layered_ls = 3001,
            camo_layered_ls = 3002,
            black_layered_ls = 3003,
            zink_layered_ls = 3004,
            layered_anchor_ls = 3005,
            choco_layered_ls = 3006,
            part_time_pirate = 3007,
            layered_vector_ls = 3008,
            green_tee = 3009,
            red_tentatek_tee = 3010,
            blue_tentatek_tee = 3011,
            octo_layered_ls = 3012,
            squid_yellow_layered_ls = 3013,
            shrimp_pink_polo = 4000,
            striped_rugby = 4001,
            tricolor_rugby = 4002,
            sage_polo = 4003,
            black_polo = 4004,
            cycling_shirt = 4005,
            cycle_king_jersey = 4006,
            slipstream_united = 4007,
            fc_albacore = 4008,
            olive_ski_jacket = 5000,
            takoroka_nylon_vintage = 5001,
            berry_ski_jacket = 5002,
            varsity_jacket = 5003,
            school_jersey = 5004,
            green_cardigan = 5005,
            black_inky_rider = 5006,
            white_inky_rider = 5007,
            retro_gamer_jersey = 5008,
            orange_cardigan = 5009,
            forge_inkling_parka = 5010,
            forge_octarian_jacket = 5011,
            blue_sailor_suit = 5012,
            white_sailor_suit = 5013,
            squid_satin_jacket = 5014,
            zapfish_satin_jacket = 5015,
            krak_on_528 = 5016,
            chilly_mountain_coat = 5017,
            takoroka_windcrusher = 5018,
            matcha_down_jacket = 5019,
            fa_01_jacket = 5020,
            fa_01_reversed = 5021,
            pullover_coat = 5022,
            kensa_coat = 5023,
            birded_corduroy_jacket = 5024,
            deep_octo_satin_jacket = 5025,
            zekko_redleaf_coat = 5026,
            eggplant_mountain_coat = 5027,
            zekko_jade_coat = 5028,
            light_bomber_jacket = 5029,
            brown_fa_11_bomber = 5030,
            gray_fa_11_bomber = 5031,
            king_bench_kaiser = 5032,
            navy_eminence_jacket = 5033,
            tumeric_zekko_coat = 5034,
            custom_painted_f_3 = 5035,
            dark_bomber_jacket = 5036,
            moist_ghillie_suit = 5037,
            white_leather_f_3 = 5038,
            chili_pepper_ski_jacket = 5039,
            whale_knit_sweater = 5040,
            rockin_leather_jacket = 5041,
            kung_fu_zip_up = 5042,
            panda_kung_fu_zip_up = 5043,
            sennyu_suit = 5044,
            b_ball_jersey_home = 6000,
            b_ball_jersey_away = 6001,
            white_king_tank = 6003,
            slash_king_tank = 6004,
            navy_king_tank = 6005,
            lob_stars_jersey = 6006,
            gray_college_sweat = 7000,
            squidmark_sweat = 7001,
            retro_sweat = 7002,
            firefin_navy_sweat = 7003,
            navy_college_sweat = 7004,
            reel_sweat = 7005,
            anchor_sweat = 7006,
            negative_longcuff_sweater = 7007,
            short_knit_layers = 7008,
            positive_longcuff_sweater = 7009,
            annaki_blue_cuff = 7010,
            annaki_yellow_cuff = 7011,
            annaki_red_cuff = 7012,
            n_pacer_sweat = 7013,
            octarian_retro = 7014,
            takoroka_jersey = 7015,
            lumberjack_shirt = 8000,
            rodeo_shirt = 8001,
            green_check_shirt = 8002,
            white_shirt = 8003,
            urchins_jersey = 8004,
            aloha_shirt = 8005,
            red_check_shirt = 8006,
            baby_jelly_shirt = 8007,
            baseball_jersey = 8008,
            gray_mixed_shirt = 8009,
            vintage_check_shirt = 8010,
            round_collar_shirt = 8011,
            logo_aloha_shirt = 8012,
            striped_shirt = 8013,
            linen_shirt = 8014,
            shirt_and_tie = 8015,
            hula_punk_shirt = 8017,
            octobowler_shirt = 8018,
            inkfall_shirt = 8019,
            crimson_parashooter = 8020,
            baby_jelly_shirt_and_tie = 8021,
            prune_parashooter = 8022,
            red_hula_punk_with_tie = 8023,
            chili_octo_aloha = 8024,
            annaki_flannel_hoodie = 8025,
            ink_wash_shirt = 8026,
            dots_on_dots_shirt = 8027,
            toni_k_baseball_jersey = 8028,
            online_jersey = 8029,
            mountain_vest = 9000,
            forest_vest = 9001,
            dark_urban_vest = 9002,
            yellow_urban_vest = 9003,
            squid_pattern_waistcoat = 9004,
            squidstar_waistcoat = 9005,
            fishing_vest = 9007,
            front_zip_vest = 9008,
            silver_tentatek_vest = 9009,
            camo_zip_hoodie = 10000,
            green_zip_hoodie = 10001,
            zekko_hoodie = 10002,
            shirt_with_blue_hoodie = 10004,
            grape_hoodie = 10005,
            gray_hoodie = 10006,
            hothouse_hoodie = 10007,
            pink_hoodie = 10008,
            olive_zekko_parka = 10009,
            black_hoodie = 10010,
            octo_support_hoodie = 10011,
            squiddor_polo = 21000,
            anchor_life_vest = 21001,
            juice_parka = 21002,
            garden_gear = 21003,
            crustwear_xxl = 21004,
            north_country_parka = 21005,
            octoleet_armor = 21006,
            record_shop_look_ep = 21007,
            dev_uniform = 21008,
            office_attire = 21009,
            srl_coat = 21010,
            mecha_body_akm = 22000,
            splatfest_tee_replica = 23000,
            school_uniform = 25000,
            samurai_jacket = 25001,
            power_armor = 25002,
            school_cardigan = 25003,
            squinja_suit = 25004,
            power_armor_mk_i = 25005,
            pearlescent_hoodie = 25006,
            marinated_top = 25007,
            enchanted_robe = 25008,
            steel_platemail = 25009,
            fresh_fish_gloves = 25010,
            splatfest_tee = 26000,
            hero_jacket_replica = 27000,
            armor_jacket_replica = 27004,
            hero_hoodie_replica = 27101,
            neo_octoling_armor = 27104,
            null_armor_replica = 27105,
            old_timey_clothes = 27106
        }

        public ClothesGear(Key id, string name, Brand brand, PrimaryAbility primaryAbility, List<SecondaryAbility> secondaryAbilities, string image) : base(KindType.Clothes, (int)id, name, brand, primaryAbility, secondaryAbilities, image)
        {

        }
    }

    public class ShoesGear : Gear
    {
        public enum Key
        {
            cream_basics = 1,
            blue_lo_tops = 1000,
            banana_basics = 1001,
            le_lo_tops = 1002,
            white_seahorses = 1003,
            orange_lo_tops = 1004,
            black_seahorses = 1005,
            clownfish_basics = 1006,
            yellow_seahorses = 1007,
            strapping_whites = 1008,
            strapping_reds = 1009,
            soccer_shoes = 1010,
            le_soccer_shoes = 1011,
            sunny_climbing_shoes = 1012,
            birch_climbing_shoes = 1013,
            green_laceups = 1014,
            white_laceless_dakroniks = 1015,
            blue_laceless_dakroniks = 1016,
            suede_gray_lace_ups = 1017,
            suede_nation_lace_ups = 1018,
            suede_marine_lace_ups = 1019,
            toni_kensa_soccer_shoes = 1020,
            red_hi_horses = 2000,
            zombie_hi_horses = 2001,
            cream_hi_tops = 2002,
            purple_hi_horses = 2003,
            hunter_hi_tops = 2004,
            red_hi_tops = 2005,
            gold_hi_horses = 2006,
            shark_moccasins = 2008,
            mawcasins = 2009,
            chocolate_dakroniks = 2010,
            mint_dakroniks = 2011,
            black_dakroniks = 2012,
            piranha_moccasins = 2013,
            white_norimaki_750s = 2014,
            black_norimaki_750s = 2015,
            sunset_orca_hi_tops = 2016,
            red_and_black_squidkid_iv = 2017,
            blue_and_black_squidkid_iv = 2018,
            gray_sea_slug_hi_tops = 2019,
            orca_hi_tops = 2020,
            imperial_kaiser = 2021,
            navy_enperrials = 2022,
            amber_sea_slug_hi_tops = 2023,
            yellow_iromaki_750s = 2024,
            red_and_white_squidkid_v = 2025,
            honey_and_orange_squidkid_v = 2026,
            sun_and_shade_squidkid_iv = 2027,
            orca_woven_hi_tops = 2028,
            green_iromaki_750s = 2029,
            purple_iromaki_750s = 2030,
            red_iromaki_750s = 2031,
            blue_iromaki_750s = 2032,
            orange_iromaki_750s = 2033,
            red_power_stripes = 2034,
            blue_power_stripes = 2035,
            toni_kensa_black_hi_tops = 2036,
            sesame_salt_270s = 2037,
            black_and_blue_squidkid_v = 2038,
            orca_passion_hi_tops = 2039,
            truffle_canvas_hi_tops = 2040,
            online_squidkid_v = 2041,
            pink_trainers = 3000,
            orange_arrows = 3001,
            neon_sea_slugs = 3002,
            white_arrows = 3003,
            cyan_trainers = 3004,
            blue_sea_slugs = 3005,
            red_sea_slugs = 3006,
            purple_sea_slugs = 3007,
            crazy_arrows = 3008,
            black_trainers = 3009,
            violet_trainers = 3010,
            canary_trainers = 3011,
            yellow_mesh_sneakers = 3012,
            arrow_pull_ons = 3013,
            red_mesh_sneakers = 3014,
            n_pacer_cao = 3015,
            n_pacer_ag = 3016,
            n_pacer_au = 3017,
            sea_slug_volt_95s = 3018,
            athletic_arrows = 3019,
            oyster_clogs = 4000,
            choco_clogs = 4001,
            blueberry_casuals = 4002,
            plum_casuals = 4003,
            neon_delta_straps = 4007,
            black_flip_flops = 4008,
            snow_delta_straps = 4009,
            luminous_delta_straps = 4010,
            red_fishfry_sandals = 4011,
            yellow_fishfry_sandals = 4012,
            musselforge_flip_flops = 4013,
            trail_boots = 5000,
            custom_trail_boots = 5001,
            pro_trail_boots = 5002,
            moto_boots = 6000,
            tan_work_boots = 6001,
            red_work_boots = 6002,
            blue_moto_boots = 6003,
            green_rain_boots = 6004,
            acerola_rain_boots = 6005,
            punk_whites = 6006,
            punk_cherries = 6007,
            punk_yellows = 6008,
            bubble_rain_boots = 6009,
            snowy_down_boots = 6010,
            icy_down_boots = 6011,
            hunting_boots = 6012,
            punk_blacks = 6013,
            deepsea_leather_boots = 6014,
            moist_ghillie_boots = 6015,
            annaki_arachno_boots = 6016,
            new_leaf_leather_boots = 6017,
            tea_green_hunting_boots = 6018,
            blue_slip_ons = 7000,
            red_slip_ons = 7001,
            squid_stitch_slip_ons = 7002,
            polka_dot_slip_ons = 7003,
            white_kicks = 8000,
            cherry_kicks = 8001,
            turquoise_kicks = 8002,
            squink_wingtips = 8003,
            roasted_brogues = 8004,
            kid_clams = 8005,
            smoky_wingtips = 8006,
            navy_red_soled_wingtips = 8007,
            gray_yellow_soled_wingtips = 8008,
            inky_kid_clams = 8009,
            annaki_habaneros = 8010,
            annaki_tigers = 8011,
            sennyu_inksoles = 8012,
            angry_rain_boots = 21001,
            non_slip_senseis = 21002,
            octoleet_boots = 21003,
            friendship_bracelet = 21004,
            flipper_floppers = 21005,
            wooden_sandals = 21006,
            mecha_legs_lbs = 22000,
            pearl_scout_lace_ups = 23000,
            pearlescent_squidkid_iv = 23001,
            pearl_punk_crowns = 23002,
            new_day_arrows = 23003,
            marination_lace_ups = 23004,
            rina_squidkid_iv = 23005,
            trooper_power_stripes = 23006,
            midnight_slip_ons = 23007,
            school_shoes = 25000,
            samurai_shoes = 25001,
            power_boots = 25002,
            fringed_loafers = 25003,
            squinja_boots = 25004,
            power_boots_mk_i = 25005,
            pearlescent_kicks = 25006,
            marinated_slip_ons = 25007,
            enchanted_boots = 25008,
            steel_greaves = 25009,
            fresh_fish_feet = 25010,
            hero_runner_replicas = 27000,
            armor_boot_replicas = 27004,
            hero_snowboots_replicas = 27101,
            neo_octoling_boots = 27104,
            null_boots_replica = 27105,
            old_timey_shoes = 27106
        }

        public ShoesGear(Key id, string name, Brand brand, PrimaryAbility primaryAbility, List<SecondaryAbility> secondaryAbilities, string image) : base(KindType.Shoes, (int)id, name, brand, primaryAbility, secondaryAbilities, image)
        {

        }
    }
}
