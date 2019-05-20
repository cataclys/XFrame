
    [ConfigPath(@"ChineseMessages.ini")]
    public class ChineseMessages
    {
        [ConfigSection("Account")]
        public string BannedWrongPassword { get; set; } = "Too many wrong password login attempts.";


        public  string PaymentComplete { get; set; } = "Your payment for {0} Game Gold was successful.";
        public  string PaymentFailed { get; set; } = "You have been deduceted {0} Game Gold.";
        public  string ReferralPaymentComplete { get; set; } = "One of your referral's has purchased some game gold, You got a bonus of {0} Hunt Gold.";
        public  string ReferralPaymentFailed { get; set; } = "One of your referal's purchase has failed, You lost your bonus of {0} Hunt Gold.";
        public  string GameGoldLost { get; set; } = "Your {0} Game Gold was removed.";
        public  string GameGoldRefund { get; set; } = "Your {0} Game Gold was refunded.";
        public  string HuntGoldRefund { get; set; } = "Your {0} Hunt Gold was refunded.";


        public  string Welcome { get; set; } = "Welcome to Zircon Server.";
        public  string WelcomeObserver { get; set; } = "You are now Observing {0}, to stop, please logout.";
        public  string ObserverChangeFail { get; set; } = "You cannot change observable mode unless you are in SafeZone";
        public  string OnlineCount { get; set; } = "Users Online: {0}, Observers Online: {1}";
        public  string ObserverCount { get; set; } = "You currently have {0} observers.";
        public  string CannotFindPlayer { get; set; } = "Unable to find the player: {0}";
        public  string AlreadyBlocked { get; set; } = "{0} is already on your block list.";
        public  string BlockingWhisper { get; set; } = "You are blocking Whispers.";
        public  string PlayerBlockingWhisper { get; set; } = "Player: {0} is blocking Whispers.";
        public  string GlobalDelay { get; set; } = "You cannot global for another {0} seconds.";
        public  string GlobalLevel { get; set; } = "You need to be level 33 before you can global shout.";
        public  string ShoutDelay { get; set; } = "You cannot shout for another {0} seconds.";
        public  string ShoutLevel { get; set; } = "You need to be level 2 before you can shout.";
        public  string DiceRoll { get; set; } = "[ROLL] - {0} has rolled {1} on a {2} sided dice.";
        public  string TradingEnabled { get; set; } = "Trading Enabled.";
        public  string TradingDisabled { get; set; } = "Trading Disabled.";
        public  string WhisperEnabled { get; set; } = "Whisper Enabled.";
        public  string WhisperDisabled { get; set; } = "Whisper Disabled.";
        public  string GuildInviteEnabled { get; set; } = "Guild Invites Enabled.";
        public  string GuildInviteDisabled { get; set; } = "Guild Invites Disabled.";
        public  string ObserverNotLoggedIn { get; set; } = "You need to be logged in before you can chat";
        public  string Poisoned { get; set; } = "You have been poisoned.";
        public  string MurderedBy { get; set; } = "You have been murdered by {0}.";
        public  string Curse { get; set; } = "You have murdered {0}, Bad luck follows you around...";
        public  string Murdered { get; set; } = "You have murdered {0}.";
        public  string Protected { get; set; } = "You have been protected by the law of self defence.";
        public  string Killed { get; set; } = "You have been killed by {0} in self defence.";
        public  string Died { get; set; } = "You have died in combat.";
        public  string GroupRecallEnabled { get; set; } = "Group Recall Enabled.";
        public  string GroupRecallDisabled { get; set; } = "Group Recall Disabled.";


        public  string NeedLevel { get; set; } = "You need to be level {0} to proceed.";
        public  string NeedMaxLevel { get; set; } = "You need to be level {0} or lower to proceed.";
        public  string NeedItem { get; set; } = "You require a '{0}' to proceed.";
        public  string NeedMonster { get; set; } = "The way is blocked...";


        public  string ConquestStarted { get; set; } = "{0} Conquest has started.";
        public  string ConquestFinished { get; set; } = "{0} Conquest has finished.";
        public  string ConquestCapture { get; set; } = "{0} has Captured {1}.";
        public  string ConquestOwner { get; set; } = "{0} are the now the owners of {1}.";
        public  string ConquestLost { get; set; } = "{0} have lost {1}.";


        public  string BossSpawn { get; set; } = "An evil lurks within {0}.";
        public  string HarvestRare { get; set; } = "Something valuable is hidden inside the {0}.";
        public  string NetherGateOpen { get; set; } = "The gate to the netherworld has opened, {0}, {1}";
        public  string NetherGateClosed { get; set; } = "The gate to the netherworld has closed";
        public  string HarvestNothing { get; set; } = "Nothing was found.";
        public  string HarvestCarry { get; set; } = "Cannot carry any more.";
        public  string HarvestOwner { get; set; } = "You do not own any nearby carcasses.";
        public  string LairGateOpen { get; set; } = "The gate to the underworld has opened, {0}, {1}";
        public  string LairGateClosed { get; set; } = "The gate to the underworld has closed";


        public  string Expired { get; set; } = "Your {0} has expired.";
        public  string CannotTownTeleport { get; set; } = "Unable to Town Teleport on this Map.";
        public  string CannotRandomTeleport { get; set; } = "Unable to Random Teleport on this Map.";
        public  string ConnotResetCompanionSkill { get; set; } = "To use {0} please type '@EnableLevel{1}'";
        public  string LearnBookFailed { get; set; } = "Failed to learn skill, not enough pages";
        public  string LearnBookSuccess { get; set; } = "Congratulations, You have successfully learned {0}.";
        public  string LearnBook4Failed { get; set; } = "Failed to learn level {0} skill.";
        public  string LearnBook4Success { get; set; } = "Congratulations, You have successfully learned level {1} {0}.";
        public  string StorageSafeZone { get; set; } = "You cannot access storage outside of SafeZone.";
        public  string GuildStoragePermission { get; set; } = "You do no have the permissions to take from the guild storage";
        public  string GuildStorageSafeZone { get; set; } = "You cannot use guild storage unless you are in a safe zone";
        public  string CompanionNoRoom { get; set; } = "Your companion cannot carry this many items";
        public  string StorageLimit { get; set; } = "You cannot expand your storage anymore.";


        public  string MarryAlreadyMarried { get; set; } = "You are already married.";
        public  string MarryNeedLevel { get; set; } = "You need to be atleast level 22 to get married.";
        public  string MarryNeedGold { get; set; } = "You do not have the 500,000 Gold required to pay for this service.";
        public  string MarryNotFacing { get; set; } = "You need to be facing another player to propose.";
        public  string MarryTargetAlreadyMarried { get; set; } = "{0} is already married.";
        public  string MarryTargetHasProposal { get; set; } = "{0} already has a marriage proposal.";
        public  string MarryTargetNeedLevel { get; set; } = "{0} needs to be atleast level 22 to get married.";
        public  string MarryTargetNeedGold { get; set; } = "{0} cannot afford to get married to you.";
        public  string MarryDead { get; set; } = "You cannot marry a dead person.";
        public  string MarryComplete { get; set; } = "Congratulations, you're now married to {0}.";
        public  string MarryDivorce { get; set; } = "You have divorced {0}";
        public  string MarryDivorced { get; set; } = "{0} has divorced you.";
        public  string MarryTeleportDead { get; set; } = "You cannot teleport to your partner you are dead.";
        public  string MarryTeleportPK { get; set; } = "You cannot teleport to your partner you are Red.";
        public  string MarryTeleportDelay { get; set; } = "You cannot teleport to your partner for another {0}.";
        public  string MarryTeleportOffline { get; set; } = "You cannot teleport to your partner whilst they are offline.";
        public  string MarryTeleportPartnerDead { get; set; } = "You cannot teleport to your partner whilst they are dead.";
        public  string MarryTeleportMap { get; set; } = "You cannot teleport to your partner on that map.";
        public  string MarryTeleportMapEscape { get; set; } = "You cannot use marraige teleport on this map.";


        public  string CompanionAppearanceAlready { get; set; } = "The {0} appreanace is already available.";
        public  string CompanionNeedTicket { get; set; } = "You need to have a Companion ticket to unlock a new appearance.";
        public  string CompanionSkillEnabled { get; set; } = "Companion Skill level {0} Enabled.";
        public  string CompanionSkillDisabled { get; set; } = "Companion Skill level {0} Disabled.";
        public  string CompanionAppearanceLocked { get; set; } = "The {0} appreanace is not available to you.";
        public  string CompanionNeedGold { get; set; } = "You cannot afford to adopt this companion.";
        public  string CompanionBadName { get; set; } = "The name given for your new companion is not acceptable.";
        public  string CompanionRetrieveFailed { get; set; } = "Able able to retrieve {0} because it is currently with {1}.";
        public  string QuestSelectReward { get; set; } = "You must select a reward";
        public  string QuestNeedSpace { get; set; } = "Unable to complete quest, Not enough space in your inventory.";


        public  string MailSafeZone { get; set; } = "Unable to get item from mail, you are not in a safe zone.";
        public  string MailNeedSpace { get; set; } = "Unable to get item from mail, not enough space.";
        public  string MailHasItems { get; set; } = "Unable to delete mail that contains items.";
        public  string MailNotFound { get; set; } = "{0} does not exist.";
        public  string MailSelfMail { get; set; } = "You cannot send mail to yourself.";
        public  string MailMailCost { get; set; } = "You cannot afford to send this mail.";
        public  string MailSendSafeZone { get; set; } = "You cannot send items from your inventory if you are not in SafeZone";


        public  string ConsignSafeZone { get; set; } = "You cannot Consign items from your inventory outside of safezone";
        public  string ConsignLimit { get; set; } = "You have reached the maximum number of Consignments";
        public  string ConsignGuildFundsGuild { get; set; } = "You cannot use Guild Funds to buy from the market place because you are not in a guild.";
        public  string ConsignGuildFundsPermission { get; set; } = "You cannot use Guild Funds to buy from the market place because you do not have permission.";
        public  string ConsignGuildFundsCost { get; set; } = "Your Guild cannot afford to buy this many items.";
        public  string ConsignGuildFundsUsed { get; set; } = "{0} used {1:#,##0} gold of guild funds to consign {2} x{3} for {4} each.";
        public  string ConsignCost { get; set; } = "You cannot afford to buy this many items.";
        public  string ConsignComplete { get; set; } = "Item registered successfully.";
        public  string ConsignAlreadySold { get; set; } = "This item has already sold.";
        public  string ConsignNotEnough { get; set; } = "There is not enough of this item for sale.";
        public  string ConsignBuyOwnItem { get; set; } = "You cannot buy your own item.";
        public  string ConsignBuyGuildFundsGuild { get; set; } = "You cannot use Guild Funds to buy from a merchant because you are not in a guild.";
        public  string ConsignBuyGuildFundsPermission { get; set; } = "You cannot use Guild Funds to buy from the market place because you do not have permission.";
        public  string ConsignBuyGuildFundsCost { get; set; } = "Your Guild cannot afford to buy this many items.";
        public  string ConsignBuyGuildFundsUsed { get; set; } = "{0} used {1:#,##0} gold of guild funds to buy {2} x{3}.";
        public  string ConsignBuyCost { get; set; } = "You cannot afford to buy this many items.";


        public  string StoreNotAvailable { get; set; } = "You cannot buy this item, It is not currently available for purchase.";
        public  string StoreNeedSpace { get; set; } = "You cannot carry this many items, please make room in your inventory.";
        public  string StoreCost { get; set; } = "You cannot afford to buy this many items.";


        public  string GuildNeedHorn { get; set; } = "Failed to create guild, You do not have the Uma King's Horn.";
        public  string GuildNeedGold { get; set; } = "Failed to create guild, You do not have enough gold.";
        public  string GuildBadName { get; set; } = "Failed to create guild, guild name is not acceptable.";
        public  string GuildNameTaken { get; set; } = "Failed to create guild, guild name already in use.";
        public  string GuildNoticePermission { get; set; } = "You do not have permission to change the guild notice.";
        public  string GuildEditMemberPermission { get; set; } = "You do not have permission to change guild member information.";
        public  string GuildMemberLength { get; set; } = "Failed to change guild rank, Rank Name was too long.";
        public  string GuildMemberNotFound { get; set; } = "Unable to find guild member.";
        public  string GuildKickPermission { get; set; } = "You do not have permission to kick a member.";
        public  string GuildKickSelf { get; set; } = "Unable kick yourself from the guild.";
        public  string GuildMemberKicked { get; set; } = "{0} has been kicked from the guild by {1}.";
        public  string GuildKicked { get; set; } = "You have been kicked form the guild by {0}.";
        public  string GuildManagePermission { get; set; } = "You do not have permission to Manage the guild.";
        public  string GuildMemberLimit { get; set; } = "Guild has already reached the Maxmimum Member Limit.";
        public  string GuildMemberCost { get; set; } = "Guild does not have enough funds to increase member limit.";
        public  string GuildStorageLimit { get; set; } = "Guild has already reached the Maxmimum Storage Size.";
        public  string GuildStorageCost { get; set; } = "Guild does not have enough funds to increase storage limit.";
        public  string GuildInvitePermission { get; set; } = "You do not have permission to invite new members";
        public  string GuildInviteGuild { get; set; } = "Player: {0}, is already in another guild.";
        public  string GuildInviteInvited { get; set; } = "Player: {0}, is currently being invited to another Guild.";
        public  string GuildInviteNotAllowed { get; set; } = "Player: {0}, is not allowing guild invites.";
        public  string GuildInvitedNotAllowed { get; set; } = "{0} wishes to invite you to the guild {1}, but you are not allowing Invites. @AllowGuild";
        public  string GuildInviteRoom { get; set; } = "Your guild already has reached it's member limit.";
        public  string GuildNoGuild { get; set; } = "You are not in a guild.";
        public  string GuildWarPermission { get; set; } = "You do not have the permission to start a guild war.";
        public  string GuildNotFoundGuild { get; set; } = "Could not find the guild {0}.";
        public  string GuildWarOwnGuild { get; set; } = "You cannot war your own guild.";
        public  string GuildAlreadyWar { get; set; } = "You are already at war with {0}.";
        public  string GuildWarCost { get; set; } = "Your guild cannot afford to start a guild war.";
        public  string GuildWarFunds { get; set; } = "{0} used {1:#,##0} gold of guild funds to start a war with {2}.";
        public  string GuildConquestCastle { get; set; } = "You already own a castle, You cannot submit a conquest.";
        public  string GuildConquestExists { get; set; } = "You already have a scheduled conquest.";
        public  string GuildConquestBadCastle { get; set; } = "Invalid Castle.";
        public  string GuildConquestProgress { get; set; } = "You cannot submit whilst a conquest is in process.";
        public  string GuildConquestNeedItem { get; set; } = "You need {0} to request a {1} conquest.";
        public  string GuildConquestSuccess { get; set; } = "A guild has submitted a conquest war for your castle.";
        public  string GuildConquestDate { get; set; } = "Your guild has submitted a conquest war for {0}.";
        public  string GuildJoinGuild { get; set; } = "You are already in a guild.";
        public  string GuildJoinTime { get; set; } = "You cannot join a guild a for another {0}";
        public  string GuildJoinNoGuild { get; set; } = "Player: {0}, is no longer in a guild.";
        public  string GuildJoinPermission { get; set; } = "Player: {0}, does not have permission to add you to the guild.";
        public  string GuildJoinNoRoom { get; set; } = "{0}'s group has already reached the maximum size.";
        public  string GuildJoinWelcome { get; set; } = "Welcome to the guild: {0}.";
        public  string GuildMemberJoined { get; set; } = "{0} has invited {1} to the guild.";
        public  string GuildLeaveFailed { get; set; } = "Failed to leave guild, You cannot leave other guild members without a leader.";
        public  string GuildLeave { get; set; } = "You have left the guild.";
        public  string GuildMemberLeave { get; set; } = "{0} has left the guild.";
        public  string GuildWarDeath { get; set; } = "{0} from {1} was killed by {2} from the guild {3}.";


        public  string GroupNoGroup { get; set; } = "You are not in a group.";
        public  string GroupNotLeader { get; set; } = "You are not the leader of your group";
        public  string GroupMemberNotFound { get; set; } = "Could not find Group Membmer {0} in your group.";
        public  string GroupAlreadyGrouped { get; set; } = "Player: {0}, is already in another group.";
        public  string GroupAlreadyInvited { get; set; } = "Player: {0}, is currently being invited to another group.";
        public  string GroupInviteNotAllowed { get; set; } = "Player: {0}, is not allowing group invites.";
        public  string GroupSelf { get; set; } = "You can not group with yourself.";
        public  string GroupMemberLimit { get; set; } = "{0}'s group has already reached the maximum size.";
        public  string GroupRecallDelay { get; set; } = "You cannot group recall for another {0}";
        public  string GroupRecallMap { get; set; } = "You cannot group recall on this map";
        public  string GroupRecallNotAllowed { get; set; } = "You are not allowing group recall";
        public  string GroupRecallMemberNotAllowed { get; set; } = "{0} is now allowing group recall";
        public  string GroupRecallFromMap { get; set; } = "You cannot be recalled from this map.";
        public  string GroupRecallMemberFromMap { get; set; } = "{0} cannot be recalled from this map.";


        public  string TradeAlreadyTrading { get; set; } = "You are already Trading with Someone.";
        public  string TradeAlreadyHaveRequest { get; set; } = "You already have a request to trade with Someone.";
        public  string TradeNeedFace { get; set; } = "You need to be facing a player to request a Trade.";
        public  string TradeTargetNotAllowed { get; set; } = "{0} isn't allowing trade requests.";
        public  string TradeTargetAlreadyTrading { get; set; } = "{0} is already Trading.";
        public  string TradeTargetAlreadyHaveRequest { get; set; } = "{0} already has a trade reqeust.";
        public  string TradeNotAllowed { get; set; } = "{0} wishes to trade with you, but you are not allowing trades. @AllowTrade";
        public  string TradeTargetDead { get; set; } = "You cannot trade a dead person.";
        public  string TradeRequested { get; set; } = "You have send a trade request to {0}...";
        public  string TradeWaiting { get; set; } = "Waiting for Partner to Accept Trade...";
        public  string TradePartnerWaiting { get; set; } = "Your Partner is waiting for you to Accept Trade...";
        public  string TradeNoGold { get; set; } = "You do not have enough gold To Trade....";
        public  string TradePartnerNoGold { get; set; } = "Your partner dose not have enough gold To Trade.";
        public  string TradeTooMuchGold { get; set; } = "You cannot carry this much gold.";
        public  string TradePartnerTooMuchGold { get; set; } = "Your Partner cannot carry this much gold...";
        public  string TradeFailedItemsChanged { get; set; } = "Your Items were changed, Trade Failed.";
        public  string TradeFailedPartnerItemsChanged { get; set; } = "{0}'s Items were changed, Trade Failed.";
        public  string TradeNotEnoughSpace { get; set; } = "You can not Carry this many items, Please make space in your inventory and try again.";
        public  string TradeComplete { get; set; } = "Trade Complete..";


        public  string NPCFundsGuild { get; set; } = "You cannot use Guild Funds to buy from a merchant because you are not in a guild.";
        public  string NPCFundsPermission { get; set; } = "You cannot use Guild Funds to buy from a merchant because you do not have permission.";
        public  string NPCFundsCost { get; set; } = "Unable to buy items, Your Guild needs another {0:#,##0} Gold.";
        public  string NPCCost { get; set; } = "Unable to buy items, You need another {0:#,##0} Gold.";
        public  string NPCNoRoom { get; set; } = "You can not carry this many items.";
        public  string NPCFundsBuy { get; set; } = "{0} used {1:#,##0} gold of guild funds to buy {2} x{3}.";
        public  string NPCSellWorthless { get; set; } = "Unable to sell items that are worthless";
        public  string NPCSellTooMuchGold { get; set; } = "Unable to sell items, You would be carrying too much gold";
        public  string NPCSellResult { get; set; } = "You sold {0} item(s) for {1:#,##0} Gold.";
        public  string FragmentCost { get; set; } = "Unable to Fragment these items, You need another {0:#,##0} Gold.";
        public  string FragmentSpace { get; set; } = "Unable to Fragment these items, You need do not have enough inventory space.";
        public  string FragmentResult { get; set; } = "You fragmented {0} item(s) costing {1:#,##0}.";
        public  string AccessoryLevelCost { get; set; } = "You cannot afford to level up your item any more.";
        public  string AccessoryLeveled { get; set; } = "Congratulations your {0} has leveled up and is now ready for upgrade.";
        public  string RepairFail { get; set; } = "You cannot repair {0}.";
        public  string RepairFailRepaired { get; set; } = "You cannot repair {0}, it is already fully repaired.";
        public  string RepairFailLocation { get; set; } = "You cannot repair {0} here.";
        public  string RepairFailCooldown { get; set; } = "You cannot special repair {0} for another {1}.";
        public  string NPCRepairGuild { get; set; } = "You cannot use Guild Funds to repair because you are not in a guild.";
        public  string NPCRepairPermission { get; set; } = "You cannot use Guild Funds to repair because you do not have permission.";
        public  string NPCRepairGuildCost { get; set; } = "Unable to repair items, Your Guild needs another {0:#,##0} Gold.";
        public  string NPCRepairCost { get; set; } = "Unable to repair items, You need another {0:#,##0} Gold.";
        public  string NPCRepairResult { get; set; } = "You normal repaired {0} item(s) for {1:#,##0} Gold.";
        public  string NPCRepairSpecialResult { get; set; } = "You special repaired {0} item(s) for {1:#,##0} Gold.";
        public  string NPCRepairGuildResult { get; set; } = "{0} used {1:#,##0} gold of guild funds to repair {2} item(s).";
        public  string NPCRefinementGold { get; set; } = "You do not have enough gold.";
        public  string NPCRefinementStoneFailedRoom { get; set; } = "Failed to create Refinement Stone, Unable to gain this item";
        public  string NPCRefinementStoneFailed { get; set; } = "Failed to synthesize a Refinement stone.";
        public  string NPCRefineNotReady { get; set; } = "Unable to get refine back, it is not ready.";
        public  string NPCRefineNoRoom { get; set; } = "Unable to get refine back, not enough space in your inventory.";
        public  string NPCRefineSuccess { get; set; } = "Congratulations, Your refine was successful";
        public  string NPCRefineFailed { get; set; } = "Unfortunately, Your refine was not successful";
        public  string NPCMasterRefineGold { get; set; } = "You do not have enough gold to request a master refine evaluation, Cost: {0:#,##0}.";
        public  string NPCMasterRefineChance { get; set; } = "Your chance of success is: {0}%";


        public  string ChargeExpire { get; set; } = "The energy for {0} has left your weapon.";
        public  string ChargeFail { get; set; } = "Failed to gether the energy to charge {0}.";
        public  string CloakCombat { get; set; } = "You cannot cast Cloak during Combat";
        public  string DashFailed { get; set; } = "You were not strong enough to move what is infront of you.";
        public  string WraithLevel { get; set; } = "{0} is too high of a level to be effected by your wraith grip.";
        public  string AbyssLevel { get; set; } = "{0} is too high of a level to be effected by your Abyss.";
        public  string SkillEffort { get; set; } = "Using {0} on this map takes more effort than normal, You cannot use items for a {1}.";
        public  string SkillBadMap { get; set; } = "You are unable to use {0} on this map.";


        public  string HorseDead { get; set; } = "You cannot ride your horse when dead.";
        public  string HorseOwner { get; set; } = "You do not own a horse to ride.";
        public  string HorseMap { get; set; } = "You cannot ride your horse on this map.";
    }
