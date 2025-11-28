using System.Collections.Generic;
using System;

namespace Udon
{
    public class Commons
    {
        #region PlayerData
        [Serializable]
        public enum JobType
        {
            Knight = 0,
            Archer,
            Paladin,
            Mage,
            Count
        }

        [Serializable]
        public enum SkillType
        {
            Hummer = 0,
            Toxic,
            Stan,
            FireStorm,
            SwordRain,
            Heal,
            Count
        }

        [Serializable]
        public class SaveData
        {
            public string playerName;                   // プレイヤーの名前
            public int resources;                       // プレイヤーの資源
            public TrainingCentre trainingCentre;       // 訓練所データ
            public PlayerTC playerTC;                   // プレイヤーの塔データ
        }

        [Serializable]
        public class TrainingCentre
        {
            public int buildingLevel;                           // 訓練所のレベル
            public TCLevelUp tcLevelUp;                           // 各職業のレベルアップ情報
        }

        [Serializable]
        public class TCLevelUp
        {
            public int KnightLevel;                             // ナイトのレベル
            public int ArcherLevel;                             // アーチャーのレベル
            public int PaladinLevel;                            // パラディンのレベル
            public int MageLevel;                               // メイジのレベル

            public int GetJobLevelText(JobType jobType)
            {
                return jobType switch
                {
                    JobType.Knight => KnightLevel,
                    JobType.Archer => ArcherLevel,
                    JobType.Paladin => PaladinLevel,
                    JobType.Mage => MageLevel,
                    _ => throw new ArgumentOutOfRangeException(nameof(jobType), jobType, null),
                };
            }
        }

        [Serializable]
        public class PlayerTC
        {
            public int buildingLevel;                           // プレイヤーの塔のレベル
            public PlayerTCLevelUp ptcLevelUp;                  // 各スキルのレベルアップ情報
        }

        [Serializable]
        public class PlayerTCLevelUp
        {
            public int HummerLevel;                             // ハンマーのレベル
            public int ToxicLevel;                              // トキシックのレベル
            public int StanLevel;                               // スタンのレベル
            public int FireStormLevel;                          // ファイアストームのレベル
            public int SwordRainLevel;                          // ソードレインのレベル
            public int HealLevel;                               // ヒールのレベル

            public int GetSkillLevelText(SkillType skillType)
            {
                return skillType switch
                {
                    SkillType.Hummer => HummerLevel,
                    SkillType.Toxic => ToxicLevel,
                    SkillType.Stan => StanLevel,
                    SkillType.FireStorm => FireStormLevel,
                    SkillType.SwordRain => SwordRainLevel,
                    SkillType.Heal => HealLevel,
                    _ => throw new ArgumentOutOfRangeException(nameof(skillType), skillType, null),
                };
            }
        }



        /// <summary>
        /// 初期データ作成クラス
        /// </summary>
        public class InitData
        {
            public SaveData CreateInitialPlayerData(string playerName)
            {
                return new SaveData
                {
                    playerName = playerName,
                    resources = 1000,
                    trainingCentre = new TrainingCentre
                    {
                        buildingLevel = 1,
                        tcLevelUp = new TCLevelUp
                        {
                            KnightLevel = 1,
                            ArcherLevel = 0,
                            PaladinLevel = 0,
                            MageLevel = 0
                        }
                    },
                    playerTC = new PlayerTC
                    {
                        buildingLevel = 1,
                        ptcLevelUp = new PlayerTCLevelUp
                        {
                            HummerLevel = 1,
                            ToxicLevel = 0,
                            StanLevel = 0,
                            FireStormLevel = 0,
                            SwordRainLevel = 0,
                            HealLevel = 0
                        }
                    }
                };
            }
        }

        #endregion

        #region  States

        public enum StatesType
        {
            Mob,
            Player
        }

        [Serializable]
        public class States
        {
            public int hp;
            public int attack;
            public int speed;
            public int skillAttack;

            /// <summary>
            /// Mobステータス設定
            /// </summary>
            public void SetStates(StatesType type, JobType jobType, int level)
            {
                if (type == StatesType.Mob)
                {
                    hp = mobHpLevelStatesTable[level];
                    attack = allAttackLevelStatesTable[level];
                    skillAttack = 0;                                                // モブはスキル攻撃なし

                    switch (jobType)
                    {
                        case JobType.Knight:
                        case JobType.Paladin:
                            // 普通速度モブ
                            this.speed = mobNormalSpeedLevelStatesTable[level];
                            break;
                        case JobType.Archer:
                            // 速いモブ
                            this.speed = mobFastSpeedLevelStatesTable[level];
                            break;
                        case JobType.Mage:
                            // 遅いモブ
                            this.speed = mobSlowSpeedLevelStatesTable[level];
                            break;
                    }
                }
            }

            /// <summary>
            /// プレイヤーステータス設定
            /// </summary>
            public void SetStates(StatesType type, int level)
            {
                if (type == StatesType.Player)
                {
                    hp = pHpLevelStatesTable[level];
                    attack = pSkillPowerLevelStatesTable[level];
                    skillAttack = pSkillPowerLevelStatesTable[level];
                    speed = pSpeedLevelStatesTable[level];
                }
            }
        }


        /// <summary>
        /// モブのレベルごとのステータス上昇値テーブル
        /// </summary>
        public static readonly int[] mobHpLevelStatesTable = new int[]
        {
            1,      // レベル0
            100,    // レベル1
            150,    // レベル2
            210,    // レベル3
            280,    // レベル4
            360,    // レベル5
            450,    // レベル6
            550,    // レベル7
            660,    // レベル8
            780,    // レベル9
            910     // レベル10
        };

        /// <summary>
        /// 全職業共通のレベルごとの攻撃力上昇値テーブル
        /// </summary>
        public static readonly int[] allAttackLevelStatesTable = new int[]
        {
            1,      // レベル0
            20,    // レベル1
            30,    // レベル2
            42,    // レベル3
            56,    // レベル4
            72,    // レベル5
            90,    // レベル6
            110,   // レベル7
            132,   // レベル8
            156,   // レベル9
            182    // レベル10
        };

        /// <summary>
        /// プレイヤースキルのレベルごとの攻撃力上昇値テーブル
        /// </summary>
        public static readonly int[] pSkillPowerLevelStatesTable = new int[]
        {
            1,      // レベル0
            40,    // レベル1
            60,    // レベル2
            84,    // レベル3
            112,   // レベル4
            144,   // レベル5
            180,   // レベル6
            220,   // レベル7
            264,   // レベル8
            312,   // レベル9
            364    // レベル10
        };

        /// <summary>
        /// プレイヤーのレベルごとのHP上昇値テーブル
        /// </summary>
        public static readonly int[] pHpLevelStatesTable = new int[]
        {
            1,      // レベル0
            200,    // レベル1
            300,    // レベル2
            420,    // レベル3
            560,    // レベル4
            720,    // レベル5
            900,    // レベル6
            1100,   // レベル7
            1320,   // レベル8
            1560,   // レベル9
            1820    // レベル10
        };

        const int declineSpeed  = 5;   // スピード低下値定数
        const int increaseSpeed = 5;   // スピード上昇値定数

        /// <summary>
        /// プレイヤーのレベルごとのスピード上昇値テーブル
        /// </summary>
        public static readonly int[] pSpeedLevelStatesTable = new int[]
        {
            1,      // レベル0
            10,    // レベル1
            12,    // レベル2
            14,    // レベル3
            16,    // レベル4
            18,    // レベル5
            20,    // レベル6
            22,    // レベル7
            24,    // レベル8
            26,    // レベル9
            28     // レベル10
        };

        /// <summary>
        /// 速度普通モブのレベルごとのスピード上昇値テーブル
        /// </summary>
        public static readonly int[] mobNormalSpeedLevelStatesTable = new int[]
        {
            1,      // レベル0
            10,    // レベル1
            11,    // レベル2
            12,    // レベル3
            13,    // レベル4
            14,    // レベル5
            15,    // レベル6
            16,    // レベル7
            17,    // レベル8
            18,    // レベル9
            19     // レベル10
        };

        /// <summary>
        /// 遅いモブのレベルごとのスピード上昇値テーブル
        /// </summary>
        public static readonly int[] mobSlowSpeedLevelStatesTable = new int[]
        {
            mobNormalSpeedLevelStatesTable[0],                      // レベル0
            mobNormalSpeedLevelStatesTable[1]   - declineSpeed,       // レベル1
            mobNormalSpeedLevelStatesTable[2]   - declineSpeed,       // レベル2
            mobNormalSpeedLevelStatesTable[3]   - declineSpeed,       // レベル3
            mobNormalSpeedLevelStatesTable[4]   - declineSpeed,       // レベル4
            mobNormalSpeedLevelStatesTable[5]   - declineSpeed,       // レベル5
            mobNormalSpeedLevelStatesTable[6]   - declineSpeed,       // レベル6
            mobNormalSpeedLevelStatesTable[7]   - declineSpeed,       // レベル7
            mobNormalSpeedLevelStatesTable[8]   - declineSpeed,       // レベル8
            mobNormalSpeedLevelStatesTable[9]   - declineSpeed,       // レベル9
            mobNormalSpeedLevelStatesTable[10]  - declineSpeed        // レベル10
        };

        /// <summary>
        /// 速いモブのレベルごとのスピード上昇値テーブル
        /// </summary>
        public static readonly int[] mobFastSpeedLevelStatesTable = new int[]
        {
            mobNormalSpeedLevelStatesTable[0],                      // レベル0
            mobNormalSpeedLevelStatesTable[1]   + increaseSpeed,    // レベル1
            mobNormalSpeedLevelStatesTable[2]   + increaseSpeed,    // レベル2
            mobNormalSpeedLevelStatesTable[3]   + increaseSpeed,    // レベル3
            mobNormalSpeedLevelStatesTable[4]   + increaseSpeed,    // レベル4
            mobNormalSpeedLevelStatesTable[5]   + increaseSpeed,    // レベル5
            mobNormalSpeedLevelStatesTable[6]   + increaseSpeed,    // レベル6
            mobNormalSpeedLevelStatesTable[7]   + increaseSpeed,    // レベル7
            mobNormalSpeedLevelStatesTable[8]   + increaseSpeed,    // レベル8
            mobNormalSpeedLevelStatesTable[9]   + increaseSpeed,    // レベル9
            mobNormalSpeedLevelStatesTable[10]  + increaseSpeed     // レベル10
        };

        #endregion

        #region LevelUpTables

        public const int buildingMaxLevel = 10; // 最大レベル
        public static readonly int[] buildingLevelUpResourcesTable = new int[]
        {
            10000,          // レベル1
            1000,           // レベル2
            3000,           // レベル3
            6000,           // レベル4
            10000,          // レベル5
            15000,          // レベル6
            21000,          // レベル7
            28000,          // レベル8
            36000,          // レベル9
            45000           // レベル10
        };

        public const int mobMaxLevel = 10; // モブ最大レベル
        public static readonly int[] mobLevelUpResourcesTable = new int[]
        {
            10000,          // レベル1
            800,            // レベル2
            2500,           // レベル3
            5000,           // レベル4
            9000,           // レベル5
            14000,          // レベル6
            20000,          // レベル7
            27000,          // レベル8
            35000,          // レベル9
            44000           // レベル10
        };

        public const int pSkillMaxLevel = 10; // スキル最大レベル
        public static readonly int[] pSkillLevelUpResourcesTable = new int[]
        {
            10000,          // レベル1
            500,            // レベル2
            2000,           // レベル3
            4000,           // レベル4
            7000,           // レベル5
            11000,          // レベル6
            16000,          // レベル7
            22000,          // レベル8
            29000,          // レベル9
            37000           // レベル10
        };
        #endregion
    }
}
